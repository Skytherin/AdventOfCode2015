using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using JetBrains.Annotations;
using AdventOfCode2015.Utils;

namespace AdventOfCode2015.Days.Day22
{
    [UsedImplicitly]
    public static class Day22
    {
        [UsedImplicitly]
        public static void Run()
        {
            Console.WriteLine();
            Part1();
            Console.WriteLine();
            Part2();
        }

        private static void Part1()
        {
            CheapestSoFar = int.MaxValue;
            var gameState = new GameState
            {
                Effects = new List<Spell>(),
                BossDmg = 9,
                MyArmor = 0,
                BossHp = 51,
                MyHp = 50,
                MyMana = 500
            };

            var result = MyTurn(gameState);

            Console.WriteLine("====");
            Console.WriteLine(result!.SpellsCast.Select(it => it.Description).Join(","));
            result.TotalCostOfSpellsCast.Should().Be(900);
        }

        private static void Part2()
        {
            CheapestSoFar = int.MaxValue;
            var gameState = new GameState
            {
                Effects = new List<Spell>(),
                BossDmg = 9,
                MyArmor = 0,
                BossHp = 51,
                MyHp = 50,
                MyMana = 500,
                HardMode = true
            };

            var result = MyTurn(gameState);

            Console.WriteLine("====");
            Console.WriteLine(result!.SpellsCast.Select(it => it.Description).Join(","));
            result.TotalCostOfSpellsCast.Should().Be(1216);
        }

        private static int CheapestSoFar = int.MaxValue;

        private static GameState? MyTurn(GameState gameState)
        {
            if (gameState.HardMode)
            {
                gameState = gameState.ApplyDamage(1);
                if (gameState.MyHp <= 0) return null;
            }

            gameState = gameState.ApplySpellEffects();

            GameState? cheapestGameState = null;

            foreach (var spell in Spells
                .Where(s => s.Cost <= gameState.MyMana)
                .Where(s => !gameState.Effects.Select(it => it.Description).Contains(s.Description)))
            {
                var newState = gameState.AddSpell(spell);

                if (cheapestGameState is { } sgs && sgs.TotalCostOfSpellsCast < newState.TotalCostOfSpellsCast) continue;
                if (CheapestSoFar < newState.TotalCostOfSpellsCast) continue;
                
                if (newState.BossHp <= 0)
                {
                    cheapestGameState = newState;
                    continue;
                }

                var recursiveGameState = BossTurn(newState);
                if (recursiveGameState == null) continue;
                if (CheapestSoFar < recursiveGameState.TotalCostOfSpellsCast) continue;
                CheapestSoFar = recursiveGameState.TotalCostOfSpellsCast;

                if (cheapestGameState is null) cheapestGameState = recursiveGameState;


                if (recursiveGameState.TotalCostOfSpellsCast < cheapestGameState.TotalCostOfSpellsCast)
                {
                    cheapestGameState = recursiveGameState;
                }
            }

            return cheapestGameState;
        }

        private static GameState? BossTurn(GameState gameState)
        {
            //gameState.WriteLine(indent);
            gameState = gameState.ApplySpellEffects();
            if (gameState.BossHp <= 0) return gameState;

            gameState = gameState.ApplyBossDamage();

            if (gameState.MyHp <= 0) return null;
            return MyTurn(gameState);
        }

        private static readonly IReadOnlyList<Spell> Spells = new List<Spell>
        {
            new("Magic Missile", 53) { InstantDamage = 4},
            new("Drain", 73) { InstantDamage = 2, InstantHeal = 2 },
            new("Shield", 113) { Duration = 6, RecurringArmor = 7 },
            new("Poison", 173) { Duration = 6, RecurringDamage = 3 },
            new("Recharge", 229) { Duration = 5, Recharge = 101}
        };
    }

    public class GameState
    {
        public int BossHp { get; init; }
        public int BossDmg { get; init; }
        public int MyHp { get; init; }
        public int MyMana { get; init; }
        public IReadOnlyList<Spell> Effects { get; init; } = new List<Spell>();
        public IReadOnlyList<Spell> SpellsCast { get; init; } = new List<Spell>();
        public int MyArmor { get; init; }
        public int TotalCostOfSpellsCast { get; private set; }
        public bool HardMode { get; init; }

        public GameState ApplyDamage(int damage)
        {
            return new GameState
            {
                BossHp = BossHp,
                BossDmg = BossDmg,
                Effects = Effects,
                MyArmor = MyArmor,
                MyHp = MyHp - damage,
                MyMana = MyMana,
                SpellsCast = SpellsCast,
                TotalCostOfSpellsCast = TotalCostOfSpellsCast,
                HardMode = HardMode
            };
        }

        public GameState ApplyBossDamage() => ApplyDamage(Math.Max(1, BossDmg - MyArmor));

        public GameState AddSpell(Spell spell)
        {
            var effects = Effects;
            if (spell.Duration > 0)
            {
                effects = Effects.Append(spell).ToList();
            }
            return new GameState
            {
                BossHp = BossHp - spell.InstantDamage,
                BossDmg = BossDmg,
                Effects = effects,
                SpellsCast = SpellsCast.Append(spell).ToList(),
                MyArmor = MyArmor,
                MyHp = MyHp + spell.InstantHeal,
                MyMana = MyMana - spell.Cost,
                TotalCostOfSpellsCast = TotalCostOfSpellsCast + spell.Cost,
                HardMode = HardMode
            };
        }

        public GameState ApplySpellEffects()
        {
            return new GameState
            {
                BossHp = BossHp - Effects.Sum(it => it.RecurringDamage),
                BossDmg = BossDmg,
                Effects = Effects.Select(it => it.Age()).Where(it => it.Duration > 0).ToList(),
                MyArmor = 0 + Effects.Sum(it => it.RecurringArmor),
                MyHp = MyHp,
                MyMana = MyMana + Effects.Sum(it => it.Recharge),
                SpellsCast = SpellsCast,
                TotalCostOfSpellsCast = TotalCostOfSpellsCast,
                HardMode = HardMode
            };
        }

        public void WriteLine(int indent)
        {
            var tabs = Enumerable.Repeat("  ", indent).Join();
            var el = Effects.Select(it => it.Description).Join(",");
            Console.WriteLine($"{tabs}{BossHp} {MyHp} {MyMana} {el}");
        }
    }

    public class Spell
    {
        public readonly string Description;
        public readonly int Cost;
        public int InstantDamage { get; init; }
        public int Duration { get; init; }
        public int RecurringDamage { get; init; }
        public int RecurringArmor { get; init; }
        public int Recharge { get; init; }
        public int InstantHeal { get; set; }

        public Spell(string description, int cost)
        {
            Description = description;
            Cost = cost;
        }

        public Spell Age()
        {
            return new Spell(Description, Cost)
            {
                InstantDamage = InstantDamage,
                Duration = Duration - 1,
                Recharge = Recharge,
                RecurringDamage = RecurringDamage,
                InstantHeal = InstantHeal,
                RecurringArmor = RecurringArmor
            };
        }
    }
}