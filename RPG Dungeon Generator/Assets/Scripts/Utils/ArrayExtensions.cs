using System.Linq;
using System.Text;

namespace CaptainCoder
{
    public static class ArrayExtensions
    {
        public static T[,] Rotate90<T>(this T[,] toRotate)
        {
            int newWidth = toRotate.GetLength(0);
            int newHeight = toRotate.GetLength(1);
            T[,] copy = new T[newHeight,newWidth];
            
            for (int r = 0; r < toRotate.GetLength(0); r++)
            {
                for (int c = 0; c < toRotate.GetLength(1); c++)
                {
                    int newR = c;
                    int newC = newWidth - r - 1;
                    copy[newR, newC] = toRotate[r,c];
                }
            }
            return copy;
        }

        public static char[,] To2DCharArray(this string toConvert)
        {
            string[] rows = toConvert.Split("\n");
            int columns = rows.Max(r => r.Length);
            char[,] asArray = new char[rows.Length, columns];
            for (int r = 0; r < rows.Length; r++)
            {
                for (int c = 0; c < columns; c++)
                {
                    char ch = c < rows[r].Length ? rows[r][c] : ' ';
                    asArray[r,c] = ch;
                }
            }
            return asArray;
        }

        public static string Rotate90(this string toConvert)
        {
            char[,] rotated = toConvert.To2DCharArray().Rotate90();
            StringBuilder builder = new();
            for (int r = 0; r < rotated.GetLength(0); r++)
            {
                for (int c = 0; c < rotated.GetLength(1); c++)
                {
                    builder.Append(rotated[r,c]);
                }
                if (r < rotated.GetLength(0) - 1)
                {
                    builder.Append('\n');
                }
            }
            return builder.ToString();
        }
    }
}