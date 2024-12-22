namespace lab5
{
    /// <summary>
    /// Battleship game class
    /// </summary>
    internal static class Battleship
    {
        private const int CursorLeftPosition = 14;

        private const int BoardSize = 5;

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

        class Player
        {
            public CellState[,] shipsBoard = new CellState[BoardSize, BoardSize];
            public CellState[,] enemyBoard = new CellState[BoardSize, BoardSize];

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
            // writing "  1 2 3 4 ..." line

            Console.CursorLeft += 2;
            for (int i = 1; i <= BoardSize; i++)
                Console.Write(i + " ");
            Console.WriteLine();

            // writing the field:
            // "A * * * ...",
            // "B * * * ..." etc.

            for (int i = 0; i < BoardSize; i++)
            {
                Console.Write(Misc.Alphabet[i] + " ");
                for (int j = 0; j < BoardSize; j++)
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

        private static bool TryParseCoords(string input, out Coords coords, out bool buildShipToRight)
        {
            buildShipToRight = default;

            if (!Coords.TryParse(
                    input.AsSpan(0, 2).ToString(),
                    out coords))
    
                return false;

            buildShipToRight = input.EndsWith('r');
            return true;
        }

        private static bool CheckAround(CellState[,] field, Coords coords)
        {
            // reminder:
            // we already know that field[coords.x, coords.y] exists.

            for (int i = coords.x - 1; i <= coords.x + 1; i++)
                for (int j = coords.y - 1; j <= coords.y + 1; j++)
                {
                    Coords check = new(i, j);
                    if (!check.Exist(BoardSize)) { }
                    else if (field[check.x, check.y] != CellState.Unknown) return false;
                }
            return true;
        }

        private static ShipPlaceResult PlaceShip(CellState[,] field, Coords anchor, bool buildShipToRight, int shipType)
        {
            // god save return

            if (!anchor.Exist(BoardSize))
                return ShipPlaceResult.AnchorDoesNotExist;

            if (!CheckAround(field, anchor))
                return ShipPlaceResult.AnchorIsSurrounded;

            Coords edge = anchor.Copy();
            if (buildShipToRight) edge.y += shipType - 1;
            else edge.x += shipType - 1;

            if (!edge.Exist(BoardSize))
                return ShipPlaceResult.EdgeDoesNotExist;

            if (!CheckAround(field, edge))
                return ShipPlaceResult.EdgeIsSurrounded;

            while (!(anchor > edge))
            {
                field[edge.x, edge.y] = (CellState)(shipType);
                if (buildShipToRight) edge.y--;
                else edge.x--;
            }

            return ShipPlaceResult.Success;
        }

        private static (Coords, bool) GetCoords(string pattern = "")
        {
            Coords anchor = new();
            bool buildShipToRight = default;
            if (pattern != "") Console.WriteLine(pattern);

            do Console.Write(">> ");
            while (!TryParseCoords(Console.ReadLine(), out anchor, out buildShipToRight));

            return (anchor, buildShipToRight);
        }

        private static void PrintPlaceResult(ShipPlaceResult placeResult)
        {
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

        private static void ArrangeHumanShips(Player human)
        {
            // i + 1 = ship.length
            for (int shipLength = 3; shipLength > 0;)
            {
                Console.Clear();
                human.PrintShips();
                PrintCoordsInputTutorial();
                PrintAnchorPointTutorial();
                (Coords anchor, bool buildShipToRight) = GetCoords("итак, введи опорную точку корабля.");

                ShipPlaceResult placeResult = PlaceShip(human.shipsBoard, anchor, buildShipToRight, shipLength);
                if (placeResult == ShipPlaceResult.Success) shipLength--;
                
                PrintPlaceResult(placeResult);
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
                Coords anchor = new(rnd.Next(BoardSize), rnd.Next(BoardSize));
                bool buildShipToRight = rnd.Next(2) == 1;

                if (PlaceShip(ai.shipsBoard, anchor, buildShipToRight, i) == ShipPlaceResult.Success) i--;
            }

            Console.Clear();
            Console.WriteLine("ии построил свою доску.");
            Console.ReadKey();
        }

        private static bool HumanTurn(Player human, Player ai)
        {
            Coords coords = new();
            string answer = "";
            for (int i = 0; i < 3; i++)
            {
                Console.CursorLeft = CursorLeftPosition;
                new Action(i switch
                {
                    0 => new Action(() => Console.WriteLine("введи координаты.")),
                    1 => new Action(() => answer = InputHandler.Input<string>()),
                    2 => new Action(() => Console.CursorVisible = false)
                }).Invoke();
            }

            if (!TryParseCoords(answer, out coords, out bool buildShipRight))
                Console.WriteLine($"кто-то не знает, как вводить координаты?");

            else if (!coords.Exist(5))
                    Console.WriteLine("ты чево за границы вышел...");

            else if (human.enemyBoard[coords.x, coords.y] != CellState.Unknown)
                Console.WriteLine("уже ходил сюда, держу в курсе.");

            else
            {
                human.enemyBoard[coords.x, coords.y] = ai.shipsBoard[coords.x, coords.y] switch
                {
                    CellState.Unknown => CellState.Miss,
                    CellState.Ship1 => CellState.Kill,
                    CellState.Ship2 => CellState.Kill,
                    CellState.Ship3 => CellState.Kill,
                };
                ai.shipsBoard[coords.x, coords.y] = human.enemyBoard[coords.x, coords.y];
                Console.WriteLine(human.enemyBoard[coords.x, coords.y] switch
                {
                    CellState.Miss => "пу-пу-пу, промазал.",
                    CellState.Kill => "ну куда-то попал. плюс ход."
                });

                if (human.enemyBoard[coords.x, coords.y] == CellState.Kill)
                    return true;
            }
            return false;
        }

        private static bool AITurn(Player human, Player ai)
        {
            Coords coords = new();
            bool correctMove = false;

            while (!correctMove)
            {
                Random rnd = new();
                coords = new(rnd.Next(BoardSize), rnd.Next(BoardSize));
                Console.CursorTop = 0;
                for (int i = 0; i < 3; i++)
                {
                    Console.CursorLeft = CursorLeftPosition;
                    new Action(i switch
                    {
                        0 => new Action(() => Console.WriteLine("ии вводит координаты...")),
                        1 => new Action(() => Console.WriteLine($">> {coords}")),
                        2 => new Action(() => Console.CursorVisible = false)
                    }).Invoke();
                }

                if (ai.enemyBoard[coords.x, coords.y] == CellState.Unknown)
                {
                    ai.enemyBoard[coords.x, coords.y] = human.shipsBoard[coords.x, coords.y] switch
                    {
                        CellState.Unknown => CellState.Miss,
                        CellState.Ship1 => CellState.Kill,
                        CellState.Ship2 => CellState.Kill,
                        CellState.Ship3 => CellState.Kill
                    };
                    human.shipsBoard[coords.x, coords.y] = ai.enemyBoard[coords.x, coords.y];
                    Console.WriteLine(ai.enemyBoard[coords.x, coords.y] switch
                    {
                        CellState.Miss => "пу-пу-пу, промазал...",
                        CellState.Kill => "ура, попал! плюс ход!"
                    });
                    correctMove = true;
                }
            }
            if (ai.enemyBoard[coords.x, coords.y] == CellState.Kill)
                return true;
            return false;
        }

        private static bool CheckWin(Player human, Player ai)
        {
            int[] shipInts = [1, 2, 3];
            bool AIDefeated = true, HumanDefeated = true;

            for (int i = 0; i < BoardSize && (AIDefeated || HumanDefeated); i++)
                for (int j = 0; j < BoardSize && (AIDefeated || HumanDefeated); j++)
                {
                    if (shipInts.Contains((int)ai.shipsBoard[i, j]))
                    {
                        AIDefeated = false;
                    }
                    if (shipInts.Contains((int)human.shipsBoard[i, j]))
                    {
                        HumanDefeated = false;
                    }
                }

            if (AIDefeated || HumanDefeated)
                return true;
            return false;
        }

        private static bool PlayGame(Player human, Player ai)
        {
            bool hasSomeoneWon = false, isHumanWinner = false;
            bool humanPlays = true, switchMove = true;
            bool debug = false, first = true;

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

                if (first)
                {
                    string answer = InputHandler.Input<string>(pattern: "секретное слово - debug.");
                    if (answer == "debug") debug = true;
                    first = false;
                }

                Console.SetCursorPosition(CursorLeftPosition, 0);

                if (humanPlays)
                    switchMove = !HumanTurn(human, ai);
                else
                    switchMove = !AITurn(human, ai);

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

/*

todo:
- HumanTurn: big while, not okay
- AITurn: big while, not okay
- PlayGame: big while, not okay

*/