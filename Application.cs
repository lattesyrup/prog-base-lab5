namespace lab5
{
    /// <summary>
    /// Holds some methods for menu
    /// </summary>
    internal static class Application
    {
        /// <summary>
        /// Starts array sorting methods. Only for use in menu.
        /// </summary>
        public static void StartArraySort()
        {
            Console.WriteLine("чтобы отсортировать массив,\nнужно сначала его создать.");
            int len = ArrayHelper.SetArrayLength();
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

        /// <summary>
        /// Prints author info. Only for use in menu.
        /// </summary>
        public static void About()
        {
            Console.WriteLine("программу написал");
            Console.WriteLine("карбовничий геннадий (мурзик) вячеславович,");
            Console.WriteLine("группа 6102-090301D.");
            Console.WriteLine("вариант всё-таки 1.");
            Console.ReadKey();
        }

        /// <summary>
        /// Prints author's deep gratitude. Only for use in menu.
        /// </summary>
        public static void Credits()
        {
            Console.WriteLine("благодарю...");
            Console.WriteLine("никиту, который ещё не разбил бошку об стену, пока мне помогал.");
            Console.WriteLine("миху, который каждый раз был в шоке с меня.");
            Console.WriteLine("виктора из 1 группы - он вдохновил меня делать морской бой именно таким.");
            Console.WriteLine("ну и, конечно, вас, геннадий андреевич.\nпросто потому что.");
            Console.ReadKey();
        }

        /// <summary>
        /// Returns whether user wants to exit the program.
        /// </summary>
        /// <returns><c>true</c> if user wants to exit; otherwise, <c>false</c>.</returns>
        public static bool Exit()
        {
            string ans = InputHandler.Input<string>(
                (x) => (x.ToLower() == "д" || x.ToLower() == "н"),
                "выйти? (д/н)"
            );
            return (ans == "д");
        }
    }
}
