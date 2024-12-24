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
        /// <param name="condition">Condition to check input on.</param>
        /// <param name="pattern">Text to write before actual input.</param>
        /// <returns></returns>
        public static T Input<T>(Func<T, bool> condition = default, string pattern = "")
        {
            int cursorLeft = Console.CursorLeft;
            if (pattern != "") Console.WriteLine(pattern);

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
            // Поиск метода TryParse у типа T
            var method = typeof(T).GetMethod("TryParse", [typeof(string), typeof(T).MakeByRefType()]);

            if (method != null)
                try
                {
                    object[] parameters = [input, null];
                    bool success = (bool)method.Invoke(null, parameters)!; // null, так как TryParse статический
                    result = (T)parameters[1];
                    return success;
                }
                catch (Exception)
                {

                }

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
