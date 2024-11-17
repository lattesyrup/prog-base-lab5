namespace lab5
{
    /// <summary>
    /// Battleship game class
    /// </summary>
    internal static class Battleship
    {
        private const int CURSOR_LEFT = 14;
        
        enum CellState
        {
            Unknown,
            Ship1,
            Ship2,
            Ship3,
            Kill,
            Miss,
        }

        enum ShipPlaceResult
        {
            Success,
            AnchorDoesNotExist,
            AnchorIsSurrounded,
            EdgeDoesNotExist,
            EdgeIsSurrounded
        }

        private static char GetCellStateChar(CellState state)
        {
            return state switch
            {
                CellState.Unknown => '0',
                CellState.Ship1 => '1',
                CellState.Ship2 => '2',
                CellState.Ship3 => '3',
                CellState.Kill => 'X',
                CellState.Miss => '*',
                _ => throw new ArgumentOutOfRangeException("внутренняя ошибка: не существует state " + state.ToString())
            };
        }

        class Coords(int row = 0, int column = 0)
        {
            public int row = row;
            public int column = column;

            public bool Exist()
            {
                if (row < 0 || column < 0) return false;
                if (row > 4 || column > 4) return false;
                return true;
            }

            public Coords Copy()
            {
                return new(row, column);
            }

            public static bool operator <(Coords one, Coords other)
            {
                if (one.column < other.column && one.row <= other.row) return true;
                if (one.column <= other.column && one.row < other.row) return true;
                return false;
            }

            public static bool operator >(Coords one, Coords other)
            {
                if (one.column > other.column && one.row >= other.row) return true;
                if (one.column >= other.column && one.row > other.row) return true;
                return false;
            }
        }

        class Player
        {
            public CellState[,] shipsBoard = new CellState[5, 5];
            public CellState[,] enemyBoard = new CellState[5, 5];

            public void PrintShips()
            {
                PrintField(shipsBoard);
            }

            public void PrintEnemy()
            {
                PrintField(enemyBoard);
            }
        }

        private static void PrintField(CellState[,] field)
        {
            Console.CursorLeft += 2;
            for (int i = 1; i <= field.GetLength(1); i++)
                Console.Write(i + " ");
            Console.WriteLine();

            for (int i = 0; i < field.GetLength(0); i++)
            {
                Console.Write(new List<char> { 'a', 'b', 'c', 'd', 'e' }[i] + " ");
                for (int j = 0; j < field.GetLength(1); j++)
                    Console.Write(GetCellStateChar(field[i, j]) + " ");
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        private static void PrintCoordsInputTutorial()
        {
            Console.WriteLine("координаты пиши в формате \"a5\", \"b3\":");
            Console.WriteLine("буква - это строка, цифра - столбец.\n");
        }

        private static void PrintAnchorPointTutorial()
        {
            Console.WriteLine("опорная точка - левая или верхняя точка корабля.");
            Console.WriteLine("у двухпалубника (b4, b5) опорная точка - b4.\n");

            Console.WriteLine("по умолчанию корабли будут строиться прямо вниз.");
            Console.WriteLine("при желании построить их вправо добавь \'r\' в конце.\n");
        }

        private static int CharToCoord(char coord)
        {
            coord = coord.ToString().ToLower()[0];
            return (int)(coord - 'a');
        }

        private static char CoordToChar(int coord)
        {
            coord += 97;
            return (char)coord;
        }

        private static bool TryParseCoords(string input, out Coords coords, out bool toRight)
        {
            coords = new(); toRight = default;
            if (input.Length != 2 && input.Length != 3) return false;
            if (input.Length == 3 && !input.EndsWith('r')) return false;

            if (int.TryParse(input[0].ToString(), out int test))
                coords.row = test - 1;

            else if (Misc.IsAlpha(input[0]))
                coords.row = CharToCoord(input[0]);

            else
                return false;

            if (!int.TryParse(input[1].ToString(), out coords.column)) return false;
            coords.column--;

            toRight = input.EndsWith('r');

            return true;
        }

        private static bool CheckAround(CellState[,] field, Coords coords)
        {
            // reminder:
            // we already know that field[coords.row, coords.column] exists.

            for (int i = coords.row - 1; i <= coords.row + 1; i++)
                for (int j = coords.column - 1; j <= coords.column + 1; j++)
                {
                    Coords check = new(i, j);
                    if (!check.Exist()) { }
                    else if (field[check.row, check.column] != CellState.Unknown) return false;
                }
            return true;
        }

        private static ShipPlaceResult PlaceShip(CellState[,] field, Coords anchor, bool toRight, int i)
        {
            // god save return

            if (!anchor.Exist())
                return ShipPlaceResult.AnchorDoesNotExist;

            if (!CheckAround(field, anchor))
                return ShipPlaceResult.AnchorIsSurrounded;

            Coords edge = anchor.Copy();
            if (toRight) edge.column += i;
            else edge.row += i;

            if (!edge.Exist())
                return ShipPlaceResult.EdgeDoesNotExist;

            if (!CheckAround(field, edge))
                return ShipPlaceResult.EdgeIsSurrounded;

            while (!(anchor > edge))
            {
                field[edge.row, edge.column] = (CellState)(i + 1);
                if (toRight) edge.column--;
                else edge.row--;
            }

            return ShipPlaceResult.Success;

            /* реализация метода в ArrangeAiShips - мало ли, будет полезно
            
            if (anchor.Exist() && CheckAround(ai.shipsBoard, anchor))
            {
                Coords edge = anchor.Copy();
                if (toRight) edge.column += i;
                else edge.row += i;

                if (edge.Exist() && CheckAround(ai.shipsBoard, edge))
                {
                    while (!(anchor > edge))
                    {
                        ai.shipsBoard[edge.row, edge.column] = (CellState)(i + 1);
                        if (toRight) edge.column--;
                        else edge.row--;
                    }
                    i--;
                }
            }

            */
        }

        private static void ArrangeHumanShips(Player human)
        {
            // i + 1 = ship.length
            for (int i = 2; i >= 0;)
            {
                Console.Clear();
                human.PrintShips();
                PrintCoordsInputTutorial();
                PrintAnchorPointTutorial();
                Coords anchor;
                bool toRight;

                Console.WriteLine("итак, введи опорную точку корабля.");
                do Console.Write(">> ");
                while (!TryParseCoords(Console.ReadLine(), out anchor, out toRight));

                ShipPlaceResult placeResult = PlaceShip(human.shipsBoard, anchor, toRight, i);
                if (placeResult == ShipPlaceResult.Success) i--;
                Console.CursorTop--;
                Console.WriteLine(placeResult switch
                {
                    ShipPlaceResult.Success => "полёт нормальный.",
                    ShipPlaceResult.AnchorDoesNotExist => "таких координат не существует.",
                    ShipPlaceResult.AnchorIsSurrounded => "опорная точка корабля занята. попробуй ещё.",
                    ShipPlaceResult.EdgeDoesNotExist => "а корабль за поле выходит.",
                    ShipPlaceResult.EdgeIsSurrounded => "а место корабля уже занято.",
                    _ => "херь какая-то. попробуй ещё."
                });
                Console.ReadKey();
            }

            Console.Clear();
            human.PrintShips();
            Console.WriteLine("это твоя доска.");
            Console.ReadKey();
        }

        private static void ArrangeAIShips(Player ai)
        {
            Console.Clear();
            Random rnd = new();
            for (int i = 2; i >= 0;)
            {
                Coords anchor = new(rnd.Next(5), rnd.Next(5));
                bool toRight = rnd.Next(2) == 1;

                if (PlaceShip(ai.shipsBoard, anchor, toRight, i) == ShipPlaceResult.Success) i--;
            }

            Console.ReadKey();
        }

        private static bool HumanPlays(Player human, Player ai, ref bool forgotCoordsInput)
        {
            Coords coords = new();
            string answer = "";
            for (int i = 0; i < 3; i++)
            {
                Console.CursorLeft = CURSOR_LEFT;
                new Action(i switch
                {
                    0 => new Action(() => Console.WriteLine("введи координаты.")),
                    1 => new Action(() => answer = InputHandler.Input<string>()),
                    2 => new Action(() => Console.CursorVisible = false)
                }).Invoke();
            }

            if (!TryParseCoords(answer, out coords, out bool toRight))
            {
                Console.WriteLine($"кто-то не {((forgotCoordsInput) ? "видит" : "помнит")}, как вводить координаты?");
                forgotCoordsInput = true;
            }
            else
            {
                if (!coords.Exist())
                    Console.WriteLine("ты чево за границы вышел...");

                else if (human.enemyBoard[coords.row, coords.column] != CellState.Unknown)
                    Console.WriteLine("уже ходил сюда, держу в курсе.");

                else
                {
                    human.enemyBoard[coords.row, coords.column] = ai.shipsBoard[coords.row, coords.column] switch
                    {
                        CellState.Unknown => CellState.Miss,
                        CellState.Ship1 => CellState.Kill,
                        CellState.Ship2 => CellState.Kill,
                        CellState.Ship3 => CellState.Kill,
                    };
                    ai.shipsBoard[coords.row, coords.column] = human.enemyBoard[coords.row, coords.column];
                    Console.WriteLine(human.enemyBoard[coords.row, coords.column] switch
                    {
                        CellState.Miss => "пу-пу-пу, промазал.",
                        CellState.Kill => "ну куда-то попал. плюс ход."
                    });

                    if (human.enemyBoard[coords.row, coords.column] == CellState.Kill)
                        return true;
                }
            }
            return false;
        }

        private static bool AIPlays(Player human, Player ai)
        {
            Coords coords = new();
            bool correctMove = false;

            while (!correctMove)
            {
                Random rnd = new();
                coords = new(rnd.Next(5), rnd.Next(5));
                Console.CursorTop = 0;
                for (int i = 0; i < 3; i++)
                {
                    Console.CursorLeft = CURSOR_LEFT;
                    new Action(i switch
                    {
                        0 => new Action(() => Console.WriteLine("ии вводит координаты...")),
                        1 => new Action(() => Console.WriteLine($">> {CoordToChar(coords.row)}{coords.column + 1}")),
                        2 => new Action(() => Console.CursorVisible = false)
                    }).Invoke();
                }

                if (ai.enemyBoard[coords.row, coords.column] == CellState.Unknown)
                {
                    ai.enemyBoard[coords.row, coords.column] = human.shipsBoard[coords.row, coords.column] switch
                    {
                        CellState.Unknown => CellState.Miss,
                        CellState.Ship1 => CellState.Kill,
                        CellState.Ship2 => CellState.Kill,
                        CellState.Ship3 => CellState.Kill
                    };
                    human.shipsBoard[coords.row, coords.column] = ai.enemyBoard[coords.row, coords.column];
                    Console.WriteLine(ai.enemyBoard[coords.row, coords.column] switch
                    {
                        CellState.Miss => "пу-пу-пу, промазал...",
                        CellState.Kill => "ура, попал! плюс ход!"
                    });
                    correctMove = true;
                }
            }
            if (ai.enemyBoard[coords.row, coords.column] == CellState.Kill)
                return true;
            return false;
        }

        private static bool CheckWin(Player human, Player ai)
        {
            int[] shipInts = [1, 2, 3];
            bool flagAI = true, flagHuman = true;

            for (int i = 0; i < ai.shipsBoard.GetLength(0) && (flagAI || flagHuman); i++)
                for (int j = 0; j < ai.shipsBoard.GetLength(1) && (flagAI || flagHuman); j++)
                {
                    if (shipInts.Contains((int)ai.shipsBoard[i, j]))
                    {
                        flagAI = false;
                    }
                    if (shipInts.Contains((int)human.shipsBoard[i, j]))
                    {
                        flagHuman = false;
                    }
                }

            if (!(flagAI || flagHuman))
                return false;
            return true;
        }

        private static bool PlayGame(Player human, Player ai)
        {
            bool hasSomeoneWon = false, isHumanWinner = false;
            bool humanPlays = true, switchMove = true;
            bool debug = false, first = true, forgotCoordsInput = false;

            while (!hasSomeoneWon)
            {
                Console.Clear();
                Console.CursorVisible = true;
                human.PrintShips();
                human.PrintEnemy();
                if (debug)
                {
                    ai.PrintShips();
                    ai.PrintEnemy();
                }
                if (forgotCoordsInput) PrintCoordsInputTutorial();

                if (first)
                {
                    string answer = InputHandler.Input<string>(textOut: "секретное слово - debug.");
                    if (answer == "debug") debug = true;
                    first = false;
                }

                Console.SetCursorPosition(CURSOR_LEFT, 0);

                if (humanPlays)
                    switchMove = !HumanPlays(human, ai, ref forgotCoordsInput);
                else
                    switchMove = !AIPlays(human, ai);

                Console.ReadKey();
                hasSomeoneWon = CheckWin(human, ai);
                if (hasSomeoneWon)
                    isHumanWinner = humanPlays;
                if (switchMove) humanPlays = !humanPlays;
            }
            return isHumanWinner;
        }

        private static void PrintEndingMessage(bool isHumanWinner)
        {
            Console.Clear();
            Console.CursorVisible = false;
            Console.SetCursorPosition(Console.WindowWidth / 2 - 12, Console.WindowHeight / 2);

            if (isHumanWinner) Console.WriteLine("о нет, комп тебе всрал!!!");
            else Console.WriteLine(" ееее, ты всрал компу!!! ");

            Console.ReadKey();
            Console.CursorVisible = true;
        }

        /// <summary>
        /// Starts the Battleship game.
        /// </summary>
        public static void StartBattleship()
        {
            Console.WriteLine("это морской бой - классическая бумажная игра.");
            Console.WriteLine("странно, почему ты здесь. можно было бумагу потратить.");
            Console.ReadKey();
            Console.Clear();

            Player human = new(), ai = new();
            ArrangeHumanShips(human);
            ArrangeAIShips(ai);

            bool isHumanWinner = PlayGame(human, ai);

            PrintEndingMessage(isHumanWinner);
        }
    }
}