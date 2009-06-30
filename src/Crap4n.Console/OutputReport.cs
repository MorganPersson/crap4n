using System.Collections.Generic;
using System.Linq;

namespace Crap4n.Console
{
    public class OutputReport
    {
        private readonly IEnumerable<Crap> _crapResult;
        private readonly PlainTextOutput _output;

        public OutputReport(IEnumerable<Crap> crapResult, PlainTextOutput output)
        {
            _crapResult = crapResult;
            _output = output;
        }

        public void OutputMethodsAboveCrapThreshold(double crapThreshold)
        {
            IOrderedEnumerable<Crap> aboveThreshold = GetAboveThreshold(crapThreshold);

            foreach (var crap in aboveThreshold)
                _output.WriteLine(crap.ToString());
        }

        public void OutputCrapSummary(double crapThreshold)
        {
            double crapLoad = GetCrapLoad(crapThreshold);
            var crapMethodPercentage = (GetAboveThreshold(crapThreshold).Count() / 1.0 / _crapResult.Count()).ToPercent();
            _output.WriteLine(string.Format("{0:0.0}% methods are CRAP. The CRAPload number for this project is {1:0.0}", crapMethodPercentage.Value, crapLoad));
        }

        private double GetCrapLoad(double crapThreshold)
        {
            double crapLoad = 0;
            foreach (var crap in GetAboveThreshold(crapThreshold))
                crapLoad += crap.CrapLoad(crapThreshold);
            return crapLoad;
        }

        private IOrderedEnumerable<Crap> GetAboveThreshold(double crapThreshold)
        {
            return from c in _crapResult
                   where c.Value >= crapThreshold
                   orderby c.Value descending
                   select c;
        }
    }
}