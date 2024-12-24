namespace lab5
{
    /// <summary>
    /// A class containing some useful methods and  that
    /// don't belong to any of the existing classes.
    /// </summary>
    internal class Misc
    {
        /// <summary>
        /// A char array containing all letters in latin alphabet.
        /// Good for printing.
        /// </summary>
        public static readonly char[] Alphabet = {'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z'};

        /// <summary>
        /// Checks if the object is null.
        /// </summary>
        /// <param name="obj">Object on check.</param>
        /// <param name="objText">Type of object to write if object is null.</param>
        /// <returns>true if object is null; otherwise, false.</returns>
        public static bool PrintIfObjectNull(Object obj, string objText)
        {
            if (obj != null)
                return false;

            Console.WriteLine($"внутренняя ошибка: {objText} не существует");
            return true;
        }
    }
}
