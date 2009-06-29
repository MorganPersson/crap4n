namespace TestAssembly
{
    public class Tested
    {
        public int NoCoverage(int a, int b, int c)
        {
            if (a > b)
            {
                if (a > c)
                    return a;
                return c;
            }
            if (b > c)
                return b;
            return c;
        }

        public int SemiCoverage(int a, int b, int c)
        {
            if (a > b)
            {
                if (a > c)
                    return a;
                return c;
            }
            if (b > c)
                return b;
            return c;
        }

        public int CompleteCoverage(int a, int b, int c)
        {
            if (a > b)
            {
                if (a > c)
                    return a;
                return c;
            }
            if (b > c)
                return b;
            return c;
        }
    }
}