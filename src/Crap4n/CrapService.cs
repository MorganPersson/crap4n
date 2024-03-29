using System;
using System.Collections.Generic;

namespace Crap4n
{
    public interface ICrapService
    {
        IEnumerable<Crap> BuildResult(string codeCoverageFileName, string codeMetricsFileName, int crapThreshold);
    }

    public class CrapService : ICrapService
    {
        private readonly IMetricsParserFinder _metricsParser;

        public CrapService(IMetricsParserFinder metricsParser)
        {
            _metricsParser = metricsParser;
        }

        IEnumerable<Crap> ICrapService.BuildResult(string codeCoverageFileName, string codeMetricsFileName,
                                                   int crapThreshold)
        {
            IEnumerable<CodeCoverage> codeCoverage = null;
            Action async = () => codeCoverage = GetCodeCoverage(codeCoverageFileName);
            var asyncResult = async.BeginInvoke(null, null);
            IEnumerable<CodeMetrics> codeMetrics = GetCodeMetrics(codeMetricsFileName);
            async.EndInvoke(asyncResult);
            var merger = new ResultMerger();
            return merger.GetMetrics(codeCoverage, codeMetrics, crapThreshold);
        }

        private IEnumerable<CodeCoverage> GetCodeCoverage(string codeCoverageFileName)
        {
            IFileParser<CodeCoverage> cc = _metricsParser.FindFileParser<CodeCoverage>(codeCoverageFileName);
            var methods = cc.ParseFile(codeCoverageFileName);

            return methods;
        }

        private IEnumerable<CodeMetrics> GetCodeMetrics(string codeMetricsFileName)
        {
            IFileParser<CodeMetrics> cm = _metricsParser.FindFileParser<CodeMetrics>(codeMetricsFileName);
            return cm.ParseFile(codeMetricsFileName);
        }
    }
}