using static System.Math;

namespace lab5  
{
    /// <summary>
    /// Math games class
    /// </summary>
    internal static class MathGuessing
    {
        private static int Attempt(Random rnd, int randomCap)
        {
            string opr = rnd.Next(3) switch
            {
                0 => "+",
                1 => "-",
                2 => "*",
                _ => "+"
            };

            if (opr == "*") randomCap /= 3;
            int a = rnd.Next(randomCap / 3, randomCap), b = rnd.Next(randomCap);
            if (opr == "*") randomCap *= 3;

            int ans = InputHandler.Input<int>(pattern: $"{a} {opr} {b} = ??");
            int res = opr switch
            {
                "+" => a + b,
                "-" => a - b,
                "*" => a * b,
                _ => a + b
            };

            if (res == ans)
            {
                Console.WriteLine(rnd.Next(3) switch
                {
                    0 => "ага.",
                    1 => "правильно.",
                    _ => "неплохо.",
                });
                return 1;
            }
            else
            {
                Console.Write(rnd.Next(2) switch
                {
                    0 => "ну не.",
                    _ => "неправильно.",
                });
                Console.WriteLine($" правильно: {res}.");
                return 0;
            }
        }

        /// <summary>
        /// Starts a math game with rounds.
        /// </summary>
        public static void PlayMath()
        {
            const int RoundsEasy = 3, RandomCapEasy = 15;
            const int RoundsHard = 7, RandomCapHard = 45;

            bool isChosenDifficultyHard = InputHandler.Input<int>(
                condition: (x) => (x == 0 || x == 1),
                pattern: "выбирай сложность: лёгкая (0) или сложная (1)."
            ) == 1;

            int rounds, randomCap;
            if (isChosenDifficultyHard)
                (rounds, randomCap) = (RoundsHard, RandomCapHard);
            else
                (rounds, randomCap) = (RoundsEasy, RandomCapEasy);

            Random rnd = new();
            int roundsRight = 0;
            for (int i = 1; i <= rounds; i++)
            {
                Console.Clear();
                Console.WriteLine("несколько раундов, в каждом - мат.выражение,");
                Console.WriteLine("на которое нужно напечатать правильный ответ.");

                Console.WriteLine($"\nРаунд {i}/{rounds}.");
                roundsRight += Attempt(rnd, randomCap);
                Thread.Sleep(700);
            }

            Console.Clear();
            Console.WriteLine($"\nу тебя {roundsRight}/{rounds} правильных ответов.");
            if (roundsRight == rounds) Console.WriteLine("можешь собой гордиться.");
            Console.ReadKey();
        }

        private static double Formula1(double a, double b)
        {
            bool flagLog = (b > 0);
            bool flagDivide = (Cos(a) - 1 != 0);

            if (!flagLog)
                throw new ArithmeticException("ошибка: число b под логарифмом неположительное.");
            else if (!flagDivide)
                throw new ArithmeticException("ошибка: здесь деление на ноль невозможно.");
            else
                return Log(b) * Log(b) / (Cos(a) - 1);
        }

        /* 
        
        /// ошибка прошлого (изучение гиммика out).
        /// хотя почему ошибка. TryParse так-то также работает.
        /// проблема больше в том, что это служебная функция,
        /// не связанная с выводом.
        /// 
        /// а ещё здесь используется using static System.Console. фи.
        
        private static bool formula2(double a, double b, out double res)
        {
            bool flagLog = (b > 0);
            bool flagDivide = (Sin(a) + 1 != 0);
            res = 0;

            if (!flagLog)
                WriteLine("Ошибка: число под логарифмом неположительное");
            else if (!flagDivide)
                WriteLine("Ошибка: деление на ноль");
            else
                res = Log(Pow(b, 5)) / (Sin(a) + 1);

            return (flagLog && flagDivide);
        }

        */

        private static void GuessAnswer(double result)
        {
            Console.WriteLine("как думаешь, что получится?");
            bool isAnswerRight = false;
            for (int att = 2; att >= 0; att--)
            {
                double answer = InputHandler.Input<double>();

                if (Round(answer, 2) == result)
                {
                    Console.WriteLine("ага. примерно так.");
                    isAnswerRight = true;
                    att -= 2;
                }
                else
                {
                    string attWord =
                        (att > 1) ? "попытки" :
                        (att == 1) ? "попытка" :
                        "попыток";
                    Console.WriteLine($"неа. ещё {att} {attWord}.");
                }
            }
            if (!isAnswerRight)
                Console.WriteLine($"мда, не угадал. вот он, правильный ответ: {result}");
        }

        /// <summary>
        /// Starts a guessing formula output game.
        /// </summary>
        public static void PlayFormulaSolve()
        {
            double a = 1, b = 1;
            Console.WriteLine("начнём маленькую игру.");
            Console.WriteLine("функция: (ln^2 b) / (cos a - 1).");
            Console.WriteLine("одз: b > 0, a != 2пk, k ∈ Z\n");

            a = InputHandler.Input<double>((x) => (x != 0), "выбери a. можешь написать дробь.\nно не ноль.");
            b = InputHandler.Input<double>((x) => (x > 0), "выбери b. тоже можешь дробь написать.\nно больше нуля.");

            double res;
            try
            {
                res = Round(Formula1(a, b), 2);
            }
            catch (ArithmeticException exception)
            {
                Console.WriteLine(exception.Message);
                return;
            }

            GuessAnswer(res);
            Console.ReadKey();
        }
    }
}
