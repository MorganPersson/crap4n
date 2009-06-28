using System.Linq;
using System.Collections.Generic;

namespace Crap4n
{
    public class ResultMerger
    {
        public IEnumerable<Crap> GetMetrics(IEnumerable<CodeCoverage> codeCoverage, IEnumerable<CodeMetrics> codeMetrics)
        {
            var metrics = from cc in codeCoverage
                          join cm in codeMetrics on cc.Class equals cm.Class
                          where
                              cc.Method == cm.Method
                          select new Crap
                                     {
                                         Class = cc.Class,
                                         Method = cc.Method,
                                         CodeCoverage = cc.CoveragePercent,
                                         CyclomaticComplexity = cm.CyclomaticComplexity
                                     };
            return metrics;
        }
    }
}
