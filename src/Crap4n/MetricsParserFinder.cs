using System;
using System.Collections.Generic;

namespace Crap4n
{
    public interface IMetricsParserFinder
    {
        IFileParser<T> FindFileParser<T>(string fileName);
    }

    public class MetricsParserFinder : IMetricsParserFinder
    {
        private readonly IEnumerable<IFileParser<CodeCoverage>> _codeCoverageFileParsers;
        private readonly IEnumerable<IFileParser<CodeMetrics>> _codeMetricsFileParsers;

        public MetricsParserFinder(IEnumerable<IFileParser<CodeMetrics>> codeMetricsFileParsers,
                                   IEnumerable<IFileParser<CodeCoverage>> codeCoverageFileParsers)
        {
            _codeMetricsFileParsers = codeMetricsFileParsers;
            _codeCoverageFileParsers = codeCoverageFileParsers;
        }

        public IFileParser<T> FindFileParser<T>(string fileName) // where T : IFileParser<T>
        {
            if (typeof (T) == typeof (CodeMetrics))
                return FindFileParser(fileName, _codeMetricsFileParsers as IEnumerable<IFileParser<T>>);
            if (typeof (T) == typeof (CodeCoverage))
                return FindFileParser(fileName, _codeCoverageFileParsers as IEnumerable<IFileParser<T>>);
            throw new ArgumentException(string.Format("unknown type T, {0}", typeof (T).Name));
        }

        private IFileParser<T> FindFileParser<T>(string fileName, IEnumerable<IFileParser<T>> parsers)
        {
            foreach (IFileParser<T> parser in parsers)
            {
                if (parser.CanParseFile(fileName))
                    return parser;
            }
            throw new NotSupportedException(string.Format("No implementation found that can parse file {0}", fileName));
        }
    }
}