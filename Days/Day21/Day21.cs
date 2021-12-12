using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using JetBrains.Annotations;

namespace AdventOfCode2015.Days.Day21
{
    [UsedImplicitly]
    public static class Day21
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
            var bossHp = 104;
            var bossDmg = 8;
            var bossArmor = 1;

            var myHp = 100;

            var minWinGold = int.MaxValue;

            foreach (var weapon in Weapons)
            {
                var weaponCost = weapon.Cost;
                if (weaponCost >= minWinGold) break;
                foreach (var armor in Armor)
                {
                    var awCost = weaponCost + armor.Cost;
                    if (awCost >= minWinGold) break;
                    foreach (var ring1 in Rings)
                    {
                        var awrCost = awCost + ring1.Cost;
                        if (awrCost >= minWinGold) break;
                        foreach (var ring2 in Rings.Where(r => r != ring1 || r.Cost == 0))
                        {
                            var awrrCost = awrCost + ring2.Cost;
                            if (awrrCost >= minWinGold) break;
                            var dmg = weapon.Attack + ring1.Attack + ring2.Attack;
                            var myArmor = armor.Defense + ring1.Defense + ring2.Defense;
                            var turnsToKillBoss = RoundUp(bossHp, Math.Max(1, dmg - bossArmor));
                            var turnsToKillMe = RoundUp(myHp, Math.Max(1, bossDmg - myArmor));
                            if (turnsToKillBoss <= turnsToKillMe)
                            {
                                minWinGold = awrrCost;
                            }
                        }
                    }
                }
            }

            minWinGold.Should().Be(78);
        }

        private static void Part2()
        {
            var bossHp = 104;
            var bossDmg = 8;
            var bossArmor = 1;

            var myHp = 100;

            var maxLoseGold = 0;

            foreach (var weapon in Weapons)
            {
                foreach (var armor in Armor)
                {
                    foreach (var ring1 in Rings)
                    {
                        foreach (var ring2 in Rings.Where(r => r != ring1 || r.Cost == 0 ))
                        {
                            var equipment = new List<Equipment> { weapon, armor, ring1, ring2 };
                            var cost = equipment.Sum(e => e.Cost);
                            if (cost <= maxLoseGold) continue;
                            var myDamage = equipment.Sum(e => e.Attack);
                            var myArmor = equipment.Sum(e => e.Defense);
                            var turnsToKillBoss = RoundUp(bossHp, Math.Max(1, myDamage - bossArmor));
                            var turnsToKillMe = RoundUp(myHp, Math.Max(1, bossDmg - myArmor));
                            if (turnsToKillBoss > turnsToKillMe)
                            {
                                maxLoseGold = cost;
                            }
                        }
                    }
                }
            }

            maxLoseGold.Should().Be(148);
        }

        private static int RoundUp(int numerator, int denominator)
        {
            return numerator / denominator + ((numerator % denominator) > 0 ? 1 : 0);
        }

        private static List<Equipment> Weapons = new()
        {
            new("Dagger", 8, 4, 0),
            new("Shortsword", 10, 5, 0),
            new("Warhammer", 25, 6, 0),
            new("Longsword", 40, 7, 0),
            new("Greataxe", 74, 8, 0)
        };

        private static List<Equipment> Armor = new()
        {
            new("Unarmored", 0, 0, 0),
            new("Leather", 13, 0, 1),
            new("Chainmail", 31, 0, 2),
            new("Splintmail", 53, 0, 3),
            new("Bandedmail", 75, 0, 4),
            new("Platemail", 102, 0, 5)
        };

        private static List<Equipment> Rings = new()
        {
            new("No Ring", 0, 0, 0),
            new("Attack +1", 25, 1, 0),
            new("Attack +2", 50, 2, 0),
            new("Attack +3", 100, 3, 0),
            new("Defense +1", 20, 0, 1),
            new("Defense +1", 40, 0, 2),
            new("Defense +1", 80, 0, 3)
        };
    }

    public record Equipment(string Description, int Cost, int Attack, int Defense);
}