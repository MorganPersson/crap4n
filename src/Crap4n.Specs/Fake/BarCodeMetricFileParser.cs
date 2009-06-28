using System;
using System.Collections.Generic;

namespace Crap4n.Specs.Fake
{
    public class BarCodeMetricFileParser : IFileParser<CodeMetrics>
    {
        bool IFileParser<CodeMetrics>.CanParseFile(string fileName)
        {
            return fileName == "Bar.xml";
        }

        IEnumerable<CodeMetrics> IFileParser<CodeMetrics>.ParseFile(string fileName)
        {
            throw new NotImplementedException();
        }
    }
}