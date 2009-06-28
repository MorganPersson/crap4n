using System.Collections.Generic;

namespace Crap4n
{
    /// <summary>
    /// http://blog.objectmentor.com/articles/2009/05/20/clean-code-and-battle-scarred-architecture
    /// </summary>
    public class CrapService
    {
        private readonly IMetricsParserFinder _metricsParser;

        public CrapService(IMetricsParserFinder metricsParser)
        {
            _metricsParser = metricsParser;
        }

        public IEnumerable<Crap> BuildResult(string codeCoverageFileName, string codeMetricsFileName)
        {
            IFileParser<CodeCoverage> cc = _metricsParser.FindFileParser<CodeCoverage>(codeCoverageFileName);
            IEnumerable<CodeCoverage> codeCoverage = cc.ParseFile(codeCoverageFileName);

            IFileParser<CodeMetrics> cm = _metricsParser.FindFileParser<CodeMetrics>(codeMetricsFileName);
            IEnumerable<CodeMetrics> codeMetrics = cm.ParseFile(codeMetricsFileName);

            var merger = new ResultMerger();
            return merger.GetMetrics(codeCoverage, codeMetrics);
        }
    }
}