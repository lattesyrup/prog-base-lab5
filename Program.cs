using lab5;

class Program
{
    private static bool _exitProgram = false;

    private static readonly Dictionary<int, (Action, string)> _menu = new() {
        { 1, (MathGuessing.PlayFormulaSolve, "1) формулу посчитать") },
        { 2, (MathGuessing.PlayMath, "2) математику порешать") },
        { 3, (Application.StartArraySort, "3) массив отсортировать") },
        { 4, (Battleship.StartBattleship, "4) в корабли поиграть") },
        { 5, (Application.About, "5) об авторе узнать") },
        { 6, (Application.Credits, "6) благодарности почитать") },
        { 7, (new Action(() => _exitProgram = Application.Exit()), "7) выйти") }
    };

    private static void Main()
    {
        while (!_exitProgram)
        {
            Console.Clear();
            Console.WriteLine("еcть много действий:");
            foreach (int key in _menu.Keys)
            {
                Console.WriteLine(_menu[key].Item2);
            }
            int answer = InputHandler.Input<int>(pattern: "\nчто выберешь?");

            Action menuAction;
            if (_menu.ContainsKey(answer))
                menuAction = _menu[answer].Item1;
            else
                menuAction = () =>
                {
                    Console.WriteLine("такого нет, выбирай из меню.");
                    Console.ReadKey();
                };
            
            Console.Clear();
            menuAction?.Invoke();
        }
    }
}