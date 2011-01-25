using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Crap4n.DataContract;

namespace Crap4n
{
    public class OutputReport
    {
        private readonly IEnumerable<Crap> _crapResult;
        private readonly ITextOutput _output;

        public OutputReport(IEnumerable<Crap> crapResult, ITextOutput output)
        {
            _crapResult = crapResult;
            _output = output;
        }

        public void OutputMethodsAboveCrapThreshold(int crapThreshold)
        {
            IEnumerable<Crap> aboveThreshold = GetAboveThreshold(crapThreshold);

            foreach (var crap in aboveThreshold)
                _output.WriteLine(crap.ToString());
        }

        public void OutputCrapSummary(int crapThreshold)
        {
            double crapLoad = GetCrapLoad(crapThreshold);
            var crapMethodPercentage =
                CalculateMethodPercentage(crapThreshold);
            _output.WriteLine(string.Format("{0:0.0}% methods are CRAP. The CRAPload for this project is {1:0.0}",
                                            crapMethodPercentage.Value, crapLoad));
        }

        private Percent CalculateMethodPercentage(int crapThreshold)
        {
            var count = _crapResult.Count();
            if (count == 0)
                return 0.Percent();
            return (GetAboveThreshold(crapThreshold).Count() / 1.0 / count).ToPercent();
        }

        private double GetCrapLoad(int crapThreshold)
        {
            double crapLoad = 0;
            foreach (var crap in GetAboveThreshold(crapThreshold))
                crapLoad += crap.CrapLoad();
            return crapLoad;
        }

        private IEnumerable<Crap> GetAboveThreshold(int crapThreshold)
        {
            return from c in _crapResult
                   where c.Value > crapThreshold
                   orderby c.Value descending
                   select c;
        }

        public string GetReportAsXml(int crapThreshold)
        {
            CrapResult crapResult = CrapResult.Build(_crapResult, GetAboveThreshold, crapThreshold);

            var serializer = new XmlSerializer(typeof(CrapResult), CrapResult.Namespace);
            var sb = new StringBuilder();
            var stream = XmlWriter.Create(sb);
            serializer.Serialize(stream, crapResult);
            stream.Flush();
            return sb.ToString();
        }
    }
}