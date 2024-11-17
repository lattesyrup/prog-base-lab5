namespace lab5
{
    /// <summary>
    /// A class containing some useful methods that
    /// don't belong to any of the existing classes.
    /// </summary>
    internal class Misc
    {
        /// <summary>
        /// Checks if the object is null.
        /// </summary>
        /// <param name="obj">Object on check.</param>
        /// <param name="objText">Type of object to write if object is null.</param>
        /// <returns>true if object is null; otherwise, false.</returns>
        public static bool CheckIsObjectNull(Object obj, string objText)
        {
            if (obj != null)
                return false;

            Console.WriteLine($"внутренняя ошибка: {objText} не существует");
            return true;
        }

        /// <summary>
        /// Checks if the character is a letter from the latin alphabet.
        /// </summary>
        /// <param name="c">Char on check.</param>
        /// <returns>true if character is a letter; otherwise, false.</returns>
        public static bool IsAlpha(char c)
        {
            c = c.ToString().ToLower()[0];
            return (c >= 'a' && c <= 'z');
        }

        /// <summary>
        /// Checks if the character is a letter from the cyrillic alphabet.
        /// </summary>
        /// <param name="c">Char on check.</param>
        /// <returns>true if character is a cyrillic letter; otherwise, false.</returns>
        public static bool IsCyrillic(char c)
        {
            c = c.ToString().ToLower()[0];
            return (c >= 'а' && c <= 'я');
        }
    }
}
