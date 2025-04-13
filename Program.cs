using System;
using System.Text;

namespace Yahtzee
{
    class Program
    {
        static void Main(string[] args)
        {
            int[,] steps = new int[13, 5];
            Random rnd = new Random();
            int[] currentDice = new int[5];
            int totalScore = 0;
            int[] upperSectionScores = new int[6];

            Console.WriteLine("ДОБРО ПОЖАЛОВАТЬ В ИГРУ YAHTZEE!");

            for (int round = 0; round < 13; round++)//основной цикл
            {
                Console.WriteLine($"\n=== Раунд {round + 1} ===");

                // Первый бросок
                for (int dice = 0; dice < 5; dice++)
                {
                    steps[round, dice] = rnd.Next(1, 7);
                    currentDice[dice] = steps[round, dice];
                }

                PrintDice(currentDice);
                //переброс
                RerollDice(steps, currentDice, round, rnd);

                // Вывод результата раунда
                Console.WriteLine("\nРезультат раунда:");
                PrintDice(currentDice);
                //категории и подсчет очков
                Console.WriteLine("\nДоступные категории:");
                Console.WriteLine("Верхняя секция: Ones, Twos, Threes, Fours, Fives, Sixes");
                Console.WriteLine("Нижняя секция: Pair, TwoPairs, ThreeOfAKind, FourOfAKind, FullHouse, SmallStraight, LargeStraight, Chance, Yahtzee");
                Console.Write("Выберите категорию для записи очков: ");
                string category = Console.ReadLine();

                try
                {
                    int score = YahtzeeScorer.CalculateScore(currentDice, category);
                    if (category == "Ones") upperSectionScores[0] = score;
                    else if (category == "Twos") upperSectionScores[1] = score;
                    else if (category == "Threes") upperSectionScores[2] = score;
                    else if (category == "Fours") upperSectionScores[3] = score;
                    else if (category == "Fives") upperSectionScores[4] = score;
                    else if (category == "Sixes") upperSectionScores[5] = score;

                    totalScore += score;
                    Console.WriteLine($"Вы получили {score} очков за категорию {category}");
                    Console.WriteLine($"Общий счет: {totalScore}");
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"Ошибка: {ex.Message}");
                    round--;
                }
            }

            //подсчет бонуса за верхнюю секцию
            int upperBonus = YahtzeeScorer.CalculateUpperBonus(upperSectionScores);
            totalScore += upperBonus;

            Console.WriteLine("\n=== История бросков ===");
            for (int round = 0; round < 13; round++)
            {
                Console.Write($"Раунд {round + 1}: ");
                for (int dice = 0; dice < 5; dice++)
                {
                    Console.Write(steps[round, dice]);
                }
                Console.WriteLine();
            }

            //итоговый результат
            Console.WriteLine("\n=== ИТОГОВЫЙ СЧЕТ ===");
            Console.WriteLine($"Сумма очков верхней секции: {upperSectionScores.Sum()}");
            Console.WriteLine($"Бонус за верхнюю секцию: {upperBonus}");
            Console.WriteLine($"ОБЩИЙ СЧЕТ: {totalScore}");
        }

        static void RerollDice(int[,] steps, int[] currentDice, int round, Random rnd)
        {
            for (int attempt = 1; attempt <= 3; attempt++)
            {
                Console.Write("\nХотите перебросить кубики? (1-да, 0-нет): ");
                if (Console.ReadLine() != "1") break;

                Console.WriteLine("Какие кубики перебросить? (номера 1-5 через пробел):");
                string[] input = Console.ReadLine().Split(' ');

                foreach (string s in input)
                {
                    if (int.TryParse(s, out int diceNum) && diceNum >= 1 && diceNum <= 5)
                    {
                        steps[round, diceNum - 1] = rnd.Next(1, 7);
                        currentDice[diceNum - 1] = steps[round, diceNum - 1];
                    }
                }

                Console.WriteLine("\nПосле переброса:");
                PrintDice(currentDice);
            }
        }

        static void PrintDice(int[] dice)
        {
            Console.Write("Кубики: ");
            for (int i = 0; i < dice.Length; i++)
            {
                Console.Write($"{i + 1}) {dice[i]}; ");
            }
            Console.WriteLine();
        }
    }
}