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
    }
}