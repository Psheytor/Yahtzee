using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yahtzee
{
    internal class YahtzeeScorer
    {
        //метод для подсчета очков
        public static int CalculateScore(int[] dice, string category)
        {
            if (dice.Length != 5)
                throw new ArgumentException("Должно быть ровно 5 кубиков");

            Array.Sort(dice);

            var groupedDice = dice.GroupBy(d => d).ToList();

            switch (category)
            {
                case "Ones":
                    return dice.Count(d => d == 1);
                case "Twos":
                    return dice.Count(d => d == 2) * 2;                
                case "Threes":
                    return dice.Count(d => d == 3) * 3;
                case "Fours":
                    return dice.Count(d => d == 4) * 4;
                case "Fives":
                    return dice.Count(d => d == 5) * 5;
                case "Sixes":
                    return dice.Count(d => d == 6) * 6;
                case "Pair":
                    var pairs = groupedDice.Where(g => g.Count() >= 2);
                    return pairs.Any() ? pairs.Max(g => g.Key) * 2 : 0;
                case "TwoPairs":
                    var twoPairs = groupedDice.Where(g => g.Count() >= 2).Take(2);
                    return twoPairs.Count() == 2 ? twoPairs.Sum(g => g.Key * 2) : 0;
                case "ThreeOfAKind":
                    var three = groupedDice.FirstOrDefault(g => g.Count() >= 3);
                    return three != null ? three.Key * 3 : 0;
                case "FourOfAKind":
                    var four = groupedDice.FirstOrDefault(g => g.Count() >= 4);
                    return four != null ? four.Key * 4 : 0;
                case "FullHouse":              
                    bool isFullHouse = groupedDice.Count == 2 &&
                                     (groupedDice[0].Count() == 2 || groupedDice[0].Count() == 3);
                    return isFullHouse ? 25 : 0;
                case "SmallStraight":
                    return IsSmallStraight(dice) ? 30 : 0;
                case "LargeStraight":
                    return IsLargeStraight(dice) ? 40 : 0;
                case "Chance":
                    return dice.Sum();
                case "Yahtzee":
                    return groupedDice.Count == 1 ? 50 : 0;
                default:
                    throw new ArgumentException("Неизвестная категория");
            }
        }
        //малый стрит
        private static bool IsSmallStraight(int[] dice)
        {            
            var uniqueDice = dice.Distinct().ToArray();

            return uniqueDice.Length >= 4 && (
                (uniqueDice.Contains(1) && uniqueDice.Contains(2) &&
                 uniqueDice.Contains(3) && uniqueDice.Contains(4)) ||

                (uniqueDice.Contains(2) && uniqueDice.Contains(3) &&
                 uniqueDice.Contains(4) && uniqueDice.Contains(5)) ||

                (uniqueDice.Contains(3) && uniqueDice.Contains(4) &&
                 uniqueDice.Contains(5) && uniqueDice.Contains(6)));
        }
        //большой стрит
        private static bool IsLargeStraight(int[] dice)
        {
            return dice.SequenceEqual(new[] { 1, 2, 3, 4, 5 }) ||
                   dice.SequenceEqual(new[] { 2, 3, 4, 5, 6 });
        }
        //бонус за верхнюю часть таблицы
        public static int CalculateUpperBonus(int[] scores)
        {
            if (scores.Length != 6)
                throw new ArgumentException("Нужно 6 значений для верхней части");
            return scores.Sum() >= 63 ? 35 : 0;
        }
    }

}
