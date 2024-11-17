using System.Diagnostics;
using System.Runtime.CompilerServices; // for Stopwaich

namespace lab5
{
    /// <summary>
    /// Array helper class
    /// </summary>
    /// <param name="len"></param>
    internal class ArrayHelper(int len = 10)
    {
        /// <summary>
        /// Length of the array
        /// </summary>
        public int Length { get; } = len;
        /// <summary>
        /// Generated array
        /// </summary>
        public int[] Array { get; } = FillArrayRandom(len);
        
        private void SwapInArray(int x, int y)
        {
            (this.Array[x], this.Array[y]) = (this.Array[y], this.Array[x]);
        }

        /// <summary>
        /// Creates copy of its own array
        /// and sorts it using the bubble sort method.
        /// </summary>
        /// <returns>sorted array of integers.</returns>
        public int[] BubbleSort()
        {
            if (Misc.CheckIsObjectNull(this.Array, "массив")) return [];
            int[] array = this.CopyArray(this.Array);

            bool isSorted = false;
            for (int i = array.Length - 1; i > 0 && !isSorted; i--)
            {
                isSorted = true;
                for (int j = 0; j < i; j++)
                    if (array[j] > array[j + 1])
                    {
                        isSorted = false;
                        this.SwapInArray(j, j + 1);
                    }
            }
            return array;
        }

        /// <summary>
        /// Creates copy of its own array
        /// and sorts it using the shaker sort method.
        /// </summary>
        /// <returns>sorted array of integers.</returns>
        public int[] ShakerSort()
        {
            if (Misc.CheckIsObjectNull(this.Array, "массив")) return [];

            int[] array = this.CopyArray(this.Array);
            int begin = 0, end = array.Length - 1;
            bool leftToRight = true, isSorted = false;

            while (begin != end && !isSorted)
            {
                isSorted = true;
                if (leftToRight)
                {
                    for (int i = begin; i < end; i++)
                        if (array[i] > array[i + 1])
                        {
                            isSorted = false;
                            this.SwapInArray(i, i + 1);
                        }
                    end--;
                }
                else
                {
                    for (int i = end; i > begin; i--)
                        if (array[i] < array[i - 1])
                        {
                            isSorted = false;
                            this.SwapInArray(i, i - 1);
                        }
                    begin++;
                }
                leftToRight = !leftToRight;
            }

            return array;
        }

        /// <summary>
        /// Creates a new array with desired length
        /// and fills it with random numbers.
        /// </summary>
        /// <param name="n">Length for the array to create.</param>
        /// <returns>an array of integers.</returns>
        public static int[] FillArrayRandom(int n)
        {
            int[] array = new int[n];
            Random rnd = new();
            for (int i = 0; i < n; i++)
                array[i] = rnd.Next(300);
            return array;
        }

        /// <summary>
        /// Copies desired array.
        /// </summary>
        /// <param name="arrOld">Array to copy.</param>
        /// <returns>a new array of integers.</returns>
        public int[] CopyArray(int[] arrOld)
        {
            if (Misc.CheckIsObjectNull(arrOld, "массив")) return [];

            int[] array = new int[arrOld.Length];
            for (int i = 0; i < arrOld.Length; i++)
                array[i] = arrOld[i];
            return array;
        }

        /// <summary>
        /// Prints an array of type T.
        /// </summary>
        /// <typeparam name="T">Type of elements of the array.</typeparam>
        /// <param name="array">Array to print.</param>
        /// <param name="textOut">Text to write before actual print.</param>
        public static void PrintArray<T>(T[] array, string textOut = "")
        {
            if (Misc.CheckIsObjectNull(array, "массив")) return;

            if (textOut != "") Console.WriteLine(textOut);

            for (int i = 0; i < array.Length; i++)
                Console.Write($"{array[i]}\t");
            Console.WriteLine();
        }

        /// <summary>
        /// Returns an array sorted by desired sort method
        /// and time elapsed by this sort method.
        /// </summary>
        /// <param name="sortMethod">Sort function.</param>
        /// <param name="sortName">Sort method name to write.</param>
        /// <returns>a tuple (sorted array, elapsed time).</returns>
        public (int[], TimeSpan) SortArray(Func<int[]> sortMethod, string sortName)
        {
            Console.WriteLine($"сортировка {sortName}...");

            long start = Stopwatch.GetTimestamp();
            int[] sortedArray = sortMethod();
            TimeSpan elapsed = Stopwatch.GetElapsedTime(start);

            return (sortedArray, elapsed);
        }

        /// <summary>
        /// Prints time and ticks elapsed by a sort.
        /// </summary>
        /// <param name="sortName">Sort method name to write.</param>
        /// <param name="elapsed">Time elapsed by the sort.</param>
        public void PrintSortResults(string sortName, TimeSpan elapsed)
        {
            Console.WriteLine($"\nвремя сортировки {sortName}: {elapsed:mm\\:ss\\.FFFFF}");
            Console.WriteLine($"тики сортировки {sortName}: {elapsed.Ticks}");
        }

        /// <summary>
        /// Gets desired array length.
        /// </summary>
        /// <param name="textOut">Text to write before actual input.</param>
        /// <returns>an integer which is greater than 0.</returns>
        public static int SetArrayLength(string textOut)
        {
            return InputHandler.Input<int>(
                condition: (a) => (a >= 0),
                textOut: textOut
            );
        }
    }
}
