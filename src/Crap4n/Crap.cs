using System;

namespace Crap4n
{
    public class Crap : MetricsBase
    {
        private readonly Func<int, double, double> _crap = (comp, cov) => comp * comp * Math.Pow((1 - cov / 100), 3) + comp;

        public Percent CodeCoverage { get; set; }
        public int CyclomaticComplexity { get; set; }

        public double Value
        {
            get { return _crap(CyclomaticComplexity, CodeCoverage); }
        }

        public override string ToString()
        {
            return string.Format("{0}.{1} : CRAP: {2} (CC: {3}, COV: {4}%)", Class, Method, Value, CyclomaticComplexity, CodeCoverage);
        }
    }
}