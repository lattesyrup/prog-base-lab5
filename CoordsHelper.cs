namespace lab5
{
    static class CoordsHelper
    {
        public static char NumberToChar(int number)
        {
            return (char)('a' + number);
        }

        public static int CharToNumber(char symbol)
        {
            return (int)(char.ToLower(symbol) - 'a');
        }
    }
}