namespace lab5
{     
    /// <summary>
    /// Represents a pair of two integers, X and Y.
    /// </summary>
    /// <param name="x">Row, the X cordinate.</param>
    /// <param name="y">Column, the Y coordinate.</param>
    
    class Coords(int x = 0, int y = 0)
    {
        /// <summary>
        /// Row coordinate.
        /// </summary>
        
        public int x = x;
        
        /// <summary>
        /// Column coordinate.
        /// </summary>
        
        public int y = y;

        /// <summary>
        /// Checks if there is a point with given coordinates (<c>X</c>, <c>Y</c>)
        /// on a square field with N length of one dimension.
        /// </summary>
        /// <param name="n">One-dimenstional length of the field.</param>
        /// <returns><c>true</c> if there is a point, otherwise <c>false</c>.</returns>
        
        public bool Exist(int n)
            => x >= 0 && y >= 0 && x < n && y < n;
        
        /// <summary>
        /// Creates a full copy of given coordinates.
        /// </summary>
        /// <returns>new instance of class <c>Coords</c>.</returns>
        public Coords Copy() => new(x, y);

        public static bool operator <(Coords one, Coords other)
        {
            if (one.y < other.y && one.x <= other.x) return true;
            if (one.y <= other.y && one.x < other.x) return true;
            return false;
        }

        public static bool operator >(Coords one, Coords other)
        {
            if (one.y > other.y && one.x >= other.x) return true;
            if (one.y >= other.y && one.x > other.x) return true;
            return false;
        }

        /// <summary>
        /// Converts the string in specific format
        /// in a pair of coordinates: <c>"b14" -> (0, 13)</c> for example.
        /// The conversion fails if the input parameter
        /// is not of the correct format.
        /// </summary>
        /// <param name="input">A string with coordinates.</param>
        /// <param name="coords">
        /// An object of Coords class containing pair of coordinates if succeeded,
        /// or a new instance of Coords class <c>(0, 0)</c> if failed.
        /// 
        /// This parameter is passed uninitialized;
        /// any value originally supplied in result will be overwritten.
        /// </param>
        /// <returns><c>true</c> if the conversion is successful; otherwise, <c>false</c>.</returns>
        
        public static bool TryParse(string input, out Coords coords)
        {
            coords = new();

            if (char.IsAsciiLetter(input[0]))
                coords.x = CoordsHelper.CharToNumber(input[0]);
            else
                return false;

            if (!int.TryParse(
                    input.AsSpan(1, input.Length - 1).ToString(),
                    out coords.y))

                return false;

            coords.y--;
            return true;
        }

        /// <summary>
        /// Returns a string that represents the current object:
        /// <c>(0, 12) -> a13</c> for example.
        /// </summary>
        /// <returns>a string that represents the current object.</returns>
        public override string ToString()
            => CoordsHelper.NumberToChar(this.x).ToString() + (this.y + 1).ToString();
    }
}

