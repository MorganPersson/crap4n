using System;

namespace Crap4n
{
    public class Crap : MetricsBase
    {
        private readonly Func<int, double, double> _crap = (comp, cov) => comp * comp * Math.Pow((1 - cov / 100), 3) + comp;

        private readonly double _crapThreshold;

        public Crap(int crapThreshold)
        {
            _crapThreshold = crapThreshold;
        }

        public Percent CodeCoverage { get; set; }

        public int CyclomaticComplexity { get; set; }

        public string MethodSignature { get; set; }
        public string SourceFile { get; set; }
        public int SourceFileLineNumber { get; set; }


        public double Value
        {
            get { return _crap(CyclomaticComplexity, CodeCoverage); }
        }

        public double CrapLoad()
        {
            if (Value < _crapThreshold)
                return 0;

            double crapLoad = CyclomaticComplexity * (1.0 - CodeCoverage / 100);
            crapLoad += CyclomaticComplexity / _crapThreshold;
            return crapLoad;
        }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(NameSpace))
                return string.Format("{0}.{1} : CRAP: {2:0.0} (CC: {3}, COV: {4}%)", Class, Method, Value, CyclomaticComplexity, CodeCoverage);
            return string.Format("{0}.{1}.{2} : CRAP: {3:0.0} (CC: {4}, COV: {5}%)", NameSpace, Class, Method, Value, CyclomaticComplexity, CodeCoverage);
        }
    }
}