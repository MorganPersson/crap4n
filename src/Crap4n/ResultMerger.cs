using System.Collections.Generic;
using System.Linq;

namespace Crap4n
{
    public class ResultMerger
    {
        public IEnumerable<Crap> GetMetrics(IEnumerable<CodeCoverage> codeCoverage, IEnumerable<CodeMetrics> codeMetrics,
                                            int crapThreshold)
        {
            var crapMetrics = new List<Crap>();
            foreach (var coverage in codeCoverage)
            {
                var cm = (codeMetrics.Where(c => c.Class == coverage.Class
                                                 && c.Method == coverage.Method)).FirstOrDefault();

                var crap = new Crap(crapThreshold)
                               {
                                   Class = coverage.Class,
                                   Method = coverage.Method,
                                   NameSpace = coverage.NameSpace,
                                   CodeCoverage = coverage.CoveragePercent,
                                   MethodSignature = coverage.MethodSignature
                               };
                if (cm != null)
                {
                    crap.CyclomaticComplexity = cm.CyclomaticComplexity;
                    crap.SourceFile = cm.SourceFile;
                    crap.SourceFileLineNumber = cm.SourceFileLineNumber;
                }
                crapMetrics.Add(crap);
            }

            return crapMetrics;
        }
    }
}