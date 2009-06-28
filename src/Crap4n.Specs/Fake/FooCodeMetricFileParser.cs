using System;
using System.Collections.Generic;

namespace Crap4n.Specs.Fake
{
    public class FooCodeMetricFileParser : IFileParser<CodeMetrics>
    {
        bool IFileParser<CodeMetrics>.CanParseFile(string fileName)
        {
            return fileName == "Foo.xml";
        }

        public IEnumerable<CodeMetrics> ParseFile(string fileName)
        {
            throw new NotImplementedException();
        }
    }
}