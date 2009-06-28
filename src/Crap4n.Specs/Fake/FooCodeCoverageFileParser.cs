using System;
using System.Collections.Generic;

namespace Crap4n.Specs.Fake
{
    public class FooCodeCoverageFileParser : IFileParser<CodeCoverage>
    {
        bool IFileParser<CodeCoverage>.CanParseFile(string fileName)
        {
            return fileName == "Foo.xml";
        }

        public IEnumerable<CodeCoverage> ParseFile(string fileName)
        {
            throw new NotImplementedException();
        }
    }
}