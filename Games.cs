using static System.Math;

namespace lab5  
{
    enum Difficulty
    {
        Easy,
        Hard
    };

    /// <summary>
    /// Difficulty choose handler class
    /// </summary>
    internal static class DifficultyChoose
    {
        private static readonly Dictionary<int, (Difficulty, string)> _difficulties = new()
        {
            { 1, (Difficulty.Easy, "x) 1 - лёгкая") },
            { 2, (Difficulty.Hard, "x) 2 - сложная") }
        };

        /// <summary>
        /// Gets desired difficulty.
        /// </summary>
        /// <returns>a difficulty.</returns>
        public static Difficulty Choose()
        {
            Console.WriteLine("выбор сложности.");
            foreach (int key in _difficulties.Keys)
            {
                Console.WriteLine(_difficulties[key].Item2);
            }
            Console.WriteLine("осторожно: лучше выбрать лёгкую сложность!");

            Difficulty difficulty = InputHandler.Input<int>(
                (x) => (_difficulties.ContainsKey(x)),
                "что выберешь ты?"
            ) switch
            {
                1 => Difficulty.Easy,
                2 => Difficulty.Hard,
            };

            return difficulty;
        }
    }

    /// <summary>
    /// Math games class
    /// </summary>
    internal static class MathGuessing
    {
        private static Difficulty _difficulty;

        private static int Attempt(Random rnd, int randomCap)
        {
            string opr = rnd.Next(3) switch
            {
                0 => "+",
                1 => "-",
                2 => "*",
            };

            if (opr == "*") randomCap /= 3;
            int a = rnd.Next(randomCap / 3, randomCap), b = rnd.Next(randomCap);
            if (opr == "*") randomCap *= 3;

            int ans = InputHandler.Input<int>(textOut: $"{a} {opr} {b} = ??");
            int res = opr switch
            {
                "+" => a + b,
                "-" => a - b,
                "*" => a * b,
            };

            if (res == ans)
            {
                Console.WriteLine(rnd.Next(3) switch
                {
                    0 => "ага.",
                    1 => "правильно.",
                    2 => "неплохо.",
                });
                return 1;
            }
            else
            {
                Console.Write(rnd.Next(2) switch
                {
                    0 => "ну не.",
                    1 => "неправильно.",
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
            _difficulty = DifficultyChoose.Choose();

            int attempts, randomCap;
            (attempts, randomCap) = _difficulty switch
            {
                Difficulty.Easy => (3, 15),
                Difficulty.Hard => (7, 45),
                _ => (0, 0)
            };

            Random rnd = new();
            int attemptsRight = 0;
            for (int i = 1; i <= attempts; i++)
            {
                Console.Clear();
                Console.WriteLine("несколько раундов, в каждом - мат.выражение,");
                Console.WriteLine("на которое нужно напечатать правильный ответ.");

                Console.WriteLine($"\nРаунд {i}/{attempts}.");
                attemptsRight += Attempt(rnd, randomCap);
                Thread.Sleep(700);
            }

            Console.Clear();
            Console.WriteLine($"\nу тебя {attemptsRight}/{attempts} правильных ответов.");
            if (attemptsRight == attempts) Console.WriteLine("можешь собой гордиться.");
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
        
        /// ошибка прошлого (изучение гиммика out)
        /// хотя почему ошибка. TryParse так-то также работает.
        /// проблема больше в том, что это служебная функция,
        /// не связанная с выводом.
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
