namespace lab5
{
    /// <summary>
    /// Input handler class
    /// </summary>
    internal static class InputHandler
    {
        /// <summary>
        /// Returns value of type T from user input.
        /// </summary>
        /// <typeparam name="T">Type of value.</typeparam>
        /// <param name="condition"></param>
        /// <param name="textOut">Text to write before actual input.</param>
        /// <returns></returns>
        public static T Input<T>(Func<T, bool> condition = default, string textOut = "")
        {
            int cursorLeft = Console.CursorLeft;
            if (textOut != "") Console.WriteLine(textOut);

            T input;
            do
            {
                Console.CursorLeft = cursorLeft;
                Console.Write(">> ");
            }
            while (!TryParse(Console.ReadLine(), out input) || !(condition == default || condition(input)));

            return input;
        }

        private static bool TryParse<T>(string input, out T result)
        {
            try
            {
                result = (T)Convert.ChangeType(input, typeof(T));
                return true;
            }
            catch (Exception)
            {
                result = default;
                return false;
            }
        }
    }
}
