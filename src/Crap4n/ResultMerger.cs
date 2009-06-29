using System.Linq;
using System.Collections.Generic;

namespace Crap4n
{
    public class ResultMerger
    {
        public IEnumerable<Crap> GetMetrics(IEnumerable<CodeCoverage> codeCoverage, IEnumerable<CodeMetrics> codeMetrics)
        {
            var crapMetrics = new List<Crap>();
            foreach (var coverage in codeCoverage)
            {
                var cm = (codeMetrics.Where(c => c.Class == coverage.Class
                                                 && c.Method == coverage.Method)).FirstOrDefault();

                var crap = new Crap
                               {
                                   Class = coverage.Class,
                                   Method = coverage.Method,
                                   NameSpace = coverage.NameSpace,
                                   CodeCoverage = coverage.CoveragePercent,
                               };
                if (cm != null)
                    crap.CyclomaticComplexity = cm.CyclomaticComplexity;
                crapMetrics.Add(crap);

            }

            return crapMetrics;
        }
    }
}
