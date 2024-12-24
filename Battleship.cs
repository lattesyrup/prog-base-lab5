using System.Security.Cryptography;

namespace lab5
{
    /// <summary>
    /// Battleship game class
    /// </summary>
    
    internal static class Battleship
    {
        private const int SecondColumnCursorPosition = 14;
        private const int BoardSize = 5;

        enum CellState
        {
            Unknown = ' ',
            Ship1 = '1',
            Ship2 = '2',
            Ship3 = '3',
            Kill = 'X',
            Miss = '*',
        }

        enum ShipPlaceResult
        {
            Success,
            AnchorDoesNotExist,
            AnchorIsSurrounded,
            EdgeDoesNotExist,
            EdgeIsSurrounded
        }

        class Player
        {
            public CellState[,] shipsBoard = new CellState[BoardSize, BoardSize];
            public CellState[,] enemyBoard = new CellState[BoardSize, BoardSize];

            public void PrintShips(int cursorLeftPosition = 0)
            {
                PrintField(shipsBoard, cursorLeftPosition);
            }

            public void PrintEnemy(int cursorLeftPosition)
            {
                PrintField(enemyBoard, cursorLeftPosition);
            }
        }

        private static void PrintField(CellState[,] field, int cursorLeftPosition = 0)
        {
            // writing "  1 2 3 4 ..." line

            Console.CursorLeft = cursorLeftPosition + 2;
            for (int i = 1; i <= BoardSize; i++)
                Console.Write(i + " ");
            Console.WriteLine();

            // writing the field:
            // "A * * * ...",
            // "B * * * ..." etc.

            for (int i = 0; i < BoardSize; i++)
            {
                Console.CursorLeft = cursorLeftPosition;
                Console.Write(Misc.Alphabet[i] + " ");
                for (int j = 0; j < BoardSize; j++)
                    Console.Write((char)field[i, j] + " ");
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        private static void PrintPlayer(Player player, string name, int cursorLeftPosition = 0)
        {
            Console.SetCursorPosition(cursorLeftPosition + 2, 0);
            Console.WriteLine(name);
            player.PrintShips(cursorLeftPosition);
            player.PrintEnemy(cursorLeftPosition);
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
            buildShipToRight = input.EndsWith('r');
            if (input == "")
            {
                coords = new();
                return false;
            }
            else return Coords.TryParse(
                input.AsSpan(0, 2).ToString(),
                out coords);
        }

        private static bool CheckAround(CellState[,] field, Coords coords)
        {
            // reminder:
            // we already know that field[coords.x, coords.y] exists.

            for (int i = coords.x - 1; i <= coords.x + 1; i++)
                for (int j = coords.y - 1; j <= coords.y + 1; j++)
                {
                    Coords check = new(i, j);
                    if (!check.Exist(BoardSize)) continue;
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
                field[edge.x, edge.y] = (CellState)shipType;
                if (buildShipToRight) edge.y--;
                else edge.x--;
            }

            return ShipPlaceResult.Success;
        }

        private static (Coords, bool) GetCoordsWithDirection(string pattern = "")
        {
            Coords anchor;
            bool buildShipToRight;
            if (pattern != "") Console.WriteLine(pattern);

            do Console.Write(">> ");
            while (!TryParseCoords(Console.ReadLine(), out anchor, out buildShipToRight));

            return (anchor, buildShipToRight);
        }

        private static void PrintPlaceResult(ShipPlaceResult placeResult)
        {
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
            for (int shipLength = 3; shipLength > 0;)
            {
                Console.Clear();
                human.PrintShips();
                PrintCoordsInputTutorial();
                PrintAnchorPointTutorial();
                (Coords anchor, bool buildShipToRight) = GetCoordsWithDirection("итак, введи опорную точку корабля.");

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
            for (int shipLength = 3; shipLength > 0;)
            {
                Coords anchor = new( rnd.Next(BoardSize), rnd.Next(BoardSize) );
                bool buildShipToRight = rnd.Next(2) == 1;

                if (PlaceShip(ai.shipsBoard, anchor, buildShipToRight, shipLength) == ShipPlaceResult.Success) shipLength--;
            }

            Console.Clear();
            Console.WriteLine("ии построил свою доску.");
            Console.ReadKey();
        }

        /// <summary>
        /// Marks a cell with given coordinated as 'killed' or 'missed'.
        /// </summary>
        /// <param name="enemyBoard">Board with X's and *'s.</param>
        /// <param name="checkBoard">Board with ships.</param>
        /// <param name="coords">Given coordinates of the blasted cell.</param>
        /// <returns><c>true</c> if killed, <c>false</c> if missed.</returns>
        private static bool BlastCell(CellState[,] enemyBoard, CellState[,] checkBoard, Coords coords)
        {
            enemyBoard[coords.x, coords.y] = checkBoard[coords.x, coords.y] switch
            {
                CellState.Unknown => CellState.Miss,
                CellState.Ship1 => CellState.Kill,
                CellState.Ship2 => CellState.Kill,
                CellState.Ship3 => CellState.Kill,
            };
            checkBoard[coords.x, coords.y] = enemyBoard[coords.x, coords.y];

            if (enemyBoard[coords.x, coords.y] == CellState.Kill) return true;
            return false;
        }

        private static bool HumanTurn(Player human, Player ai)
        {
            Coords coords = InputHandler.Input<Coords>(pattern: "введи координаты.");

            if (!coords.Exist(5))
            {
                Console.WriteLine("ты чево за границы вышел...");
                return false;
            }    

            if (human.enemyBoard[coords.x, coords.y] != CellState.Unknown)
            {
                Console.WriteLine("уже ходил сюда, держу в курсе.");
                return false;
            }

            bool blastResult = BlastCell(human.enemyBoard, ai.shipsBoard, coords);

            if (blastResult) Console.WriteLine("ну куда-то попал. плюс ход.");
            else Console.WriteLine("пу-пу-пу, промазал.");

            return blastResult;
        }

        private static bool AITurn(Player human, Player ai)
        {
            Coords coords;
            Random rnd = new();

            do coords = new( rnd.Next(BoardSize), rnd.Next(BoardSize) );
            while (!(ai.enemyBoard[coords.x, coords.y] == CellState.Unknown));
                
            Console.WriteLine("ии вводит координаты.");
            Console.WriteLine($">> {coords}");
            Thread.Sleep(200);

            bool blastResult = BlastCell(ai.enemyBoard, human.shipsBoard, coords);
            if (blastResult) Console.WriteLine("ура, попал! плюс ход!");
            else Console.WriteLine("пу-пу-пу, промазал...");

            return blastResult;
        }

        private static bool CheckWin(Player human, Player ai)
        {
            int[] shipInts = [1, 2, 3];
            bool AIDefeated = true, HumanDefeated = true;

            for (int i = 0; i < BoardSize && (AIDefeated || HumanDefeated); i++)
                for (int j = 0; j < BoardSize && (AIDefeated || HumanDefeated); j++)
                {
                    if (shipInts.Contains((int)ai.shipsBoard[i, j]))
                        AIDefeated = false;
                    
                    if (shipInts.Contains((int)human.shipsBoard[i, j]))
                        HumanDefeated = false;
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
                PrintPlayer(human, "чел (ты)");
                if (debug) PrintPlayer(ai, "комп (ии)", SecondColumnCursorPosition);

                if (first)
                {
                    string answer = InputHandler.Input<string>(pattern: "секретное слово - debug.");
                    if (answer == "debug") debug = true;
                    first = false;
                }

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