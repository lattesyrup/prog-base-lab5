namespace lab5
{
    /// <summary>
    /// Matrix helper class
    /// </summary>
    internal class MatrixHelper
    {
        /// <summary>
        /// Prints a matrix.
        /// </summary>
        /// <typeparam name="T">Type of elements of matrix.</typeparam>
        /// <param name="matrix">Matrix to print.</param>
        public void PrintMatrix<T>(T[,] matrix)
        {
            if (Misc.CheckIsObjectNull(matrix, "матрица")) return;

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                    Console.Write(matrix[i, j] + " ");
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Prints a matrix on desired X-coordinate in console.
        /// </summary>
        /// <typeparam name="T">Type of elements of matrix.</typeparam>
        /// <param name="matrix">Matrix to print.</param>
        /// <param name="x">Coordinate where to print.</param>
        public static void PrintMatrixCoord<T>(T[,] matrix, int x)
        {
            if (Misc.CheckIsObjectNull(matrix, "матрица")) return;

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                Console.SetCursorPosition(x, i);
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    Console.Write(matrix[i, j] + " ");
                }
            }
        }

        /*
        public static bool CompareMatrix(int[,] self, int[,] other)
        {
            if (self.GetLength(0) != other.GetLength(0) ||
                self.GetLength(1) != other.GetLength(1))
                return false;
            for (int i = 0; i < self.GetLength(0); i++)
                for (int j = 0; j < self.GetLength(1); j++)
                    if (self[i, j] != other[i, j])
                        return false;
            return true;
        }
        */
    }
}
