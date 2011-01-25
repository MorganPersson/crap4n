using System.Collections.Generic;
using System.Linq;

namespace Example
{
    public class Example : IFoo, IBar
    {
        #region IBar Members

        public int BarProp { get; set; }

        public int BarMethod(int a)
        {
            return a - 1;
        }

        #endregion

        #region IFoo Members

        int IFoo.FooProp { get; set; }

        int IFoo.FooMethod(int a)
        {
            return a + 1;
        }

        #endregion

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

        public T GenericMethod<T>(int a, IEnumerable<T> items)
        {
            return items.Skip(a).FirstOrDefault();
        }
    }
}