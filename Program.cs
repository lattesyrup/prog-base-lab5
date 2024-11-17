using lab5;

class Program
{
    private static bool _exitProgram = false;

    private static readonly Dictionary<int, (Action, string)> _menu = new() {
        { 1, (MathGuessing.PlayFormulaSolve, "1) формулу посчитать") },
        { 2, (MathGuessing.PlayMath, "2) математику порешать") },
        { 3, (StartArraySort, "3) массив отсортировать") },
        { 4, (Battleship.StartBattleship, "4) в корабли поиграть") },
        { 5, (About, "5) об авторе узнать") },
        { 6, (Credits, "6) благодарности почитать") },
        { 7, (new Action(() => _exitProgram = Exit()), "7) выйти") }
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
            int answer = InputHandler.Input<int>(textOut: "\nчто выберешь?");
            Action menuAction =
                _menu.ContainsKey(answer) ?
                _menu[answer].Item1 :
                () =>
                {
                    Console.WriteLine("такого нет, выбирай из меню.");
                    Console.ReadKey();
                };
            Console.Clear();
            menuAction.Invoke();
        }
    }

    private static void StartArraySort()
    {
        Console.WriteLine("чтобы отсортировать массив,\nнужно сначала его создать.");
        int len = ArrayHelper.SetArrayLength("укажи размер массива. (оставь 0 для вызова конструктора по умолчанию.)");
        ArrayHelper arrHelper = (len == 0)
            ? new()
            : new(len);

        var (arrayBubble, elapsedBubble) = arrHelper.SortArray(arrHelper.BubbleSort, "пузырьком");
        var (arrayShaker, elapsedShaker) = arrHelper.SortArray(arrHelper.ShakerSort, "перемешиванием");

        arrHelper.PrintSortResults("пузырьком", elapsedBubble);
        arrHelper.PrintSortResults("перемешиванием", elapsedShaker);

        if (arrHelper.Length <= 10) // task condition
        {
            ArrayHelper.PrintArray(arrHelper.Array, "\nизначальный массив");
            ArrayHelper.PrintArray(arrayBubble, "\nмассив после пузырька");
            ArrayHelper.PrintArray(arrayShaker, "\nмассив после перемешивания");
        }
        else
            Console.WriteLine("хнык-хнык, не могу вывести,\nусловие не позволяет :(");
        Console.ReadKey();
    }

    private static void About()
    {
        Console.WriteLine("программу написал");
        Console.WriteLine("карбовничий геннадий (мурзик) вячеславович,");
        Console.WriteLine("группа 6102-090301D.");
        Console.WriteLine("вариант всё-таки 1.");
    }

    private static void Credits()
    {
        Console.WriteLine("благодарю...");
        Console.WriteLine("никиту, который ещё не разбил бошку об стену, пока мне помогал.");
        Console.WriteLine("миху, который каждый раз был в шоке с меня.");
        Console.WriteLine("виктора из 1 группы - он вдохновил меня делать морской бой именно таким.");
        Console.WriteLine("\n\tну и, конечно, вас, геннадий андреевич.\n\tпросто потому что.");
        Console.ReadKey();
    }

    private static bool Exit()
    {
        string ans = InputHandler.Input<string>(
            (x) => (new List<string> { "д", "н", "y", "n" }.Contains(x.ToLower())),
            "выйти? (д/н)"
        );
        return (new List<string> { "д", "y" }.Contains(ans));
    }
}