using System.Collections.Generic;
using System.Linq;
using NBehave.Spec.NUnit;

namespace Crap4n.Specs
{
    public abstract class ResultMergerSpec : SpecBase
    {
        private const int CrapThreshold = 30;
        private ResultMerger _resultMerger;
        private IEnumerable<Crap> _metrics;
        private List<CodeCoverage> _codeCoverage;
        private List<CodeMetrics> _codeMetrics;

        public class When_Loading_result_files : ResultMergerSpec
        {
            protected override void Establish_context()
            {
                _codeCoverage = new List<CodeCoverage>();
                _codeCoverage.AddRange(new[]
                                           {
                                               new CodeCoverage
                                                   {
                                                       CoveragePercent = 50.Percent(),
                                                       Class = "FooClass",
                                                       Method = "FooMethod"
                                                   },
                                               new CodeCoverage
                                                   {
                                                       CoveragePercent = 50.Percent(),
                                                       Class = "BarClass",
                                                       Method = "BarMethod"
                                                   }
                                           });
                _codeMetrics = new List<CodeMetrics>();
                _codeMetrics.AddRange(new[]
                                          {
                                              new CodeMetrics
                                                  {
                                                      CyclomaticComplexity = 1,
                                                      Class = "FooClass",
                                                      Method = "FooMethod"
                                                  },
                                              new CodeMetrics
                                                  {
                                                      CyclomaticComplexity = 2,
                                                      Class = "BarClass",
                                                      Method = "BarMethod"
                                                  }
                                          });

                _resultMerger = new ResultMerger();
            }

            protected override void Because_of()
            {
                _metrics = _resultMerger.GetMetrics(_codeCoverage, _codeMetrics, CrapThreshold);
            }

            [Specification]
            public void Should_have_2_matched_methods()
            {
                _metrics.Count().ShouldEqual(2);
            }

            [Specification]
            public void Should_find_result_for_method_in_a_class()
            {
                var method = _metrics.Where(c => c.Class == "FooClass" && c.Method == "FooMethod").FirstOrDefault();
                method.ShouldNotBeNull();
                method.Method.ShouldEqual("FooMethod");
            }
        }

        public class When_class_has_more_than_one_method : ResultMergerSpec
        {
            protected override void Establish_context()
            {
                _codeCoverage = new List<CodeCoverage>();
                _codeCoverage.AddRange(new[]
                                           {
                                               new CodeCoverage
                                                   {
                                                       CoveragePercent = 50.Percent(),
                                                       Class = "FooClass",
                                                       Method = "FooMethod"
                                                   },
                                               new CodeCoverage
                                                   {
                                                       CoveragePercent = 100.Percent(),
                                                       Class = "FooClass",
                                                       Method = "BarMethod"
                                                   },
                                               new CodeCoverage
                                                   {
                                                       CoveragePercent = 50.Percent(),
                                                       Class = "BarClass",
                                                       Method = "BarMethod"
                                                   }
                                           });
                _codeMetrics = new List<CodeMetrics>();
                _codeMetrics.AddRange(new[]
                                          {
                                              new CodeMetrics
                                                  {
                                                      CyclomaticComplexity = 1,
                                                      Class = "FooClass",
                                                      Method = "FooMethod"
                                                  },
                                              new CodeMetrics
                                                  {
                                                      CyclomaticComplexity = 1,
                                                      Class = "FooClass",
                                                      Method = "BarMethod"
                                                  },
                                              new CodeMetrics
                                                  {
                                                      CyclomaticComplexity = 2,
                                                      Class = "BarClass",
                                                      Method = "BarMethod"
                                                  }
                                          });

                _resultMerger = new ResultMerger();
            }

            protected override void Because_of()
            {
                _metrics = _resultMerger.GetMetrics(_codeCoverage, _codeMetrics, CrapThreshold);
            }

            [Specification]
            public void Should_have_3_matched_methods()
            {
                _metrics.Count().ShouldEqual(3);
            }

            [Specification]
            public void Should_find_result_for_FooMethod_in_a_FooClass()
            {
                var method = _metrics.Where(c => c.Class == "FooClass" && c.Method == "FooMethod").FirstOrDefault();
                method.ShouldNotBeNull();
            }

            [Specification]
            public void Should_find_result_for_BarMethod_in_a_FooClass()
            {
                var method = _metrics.Where(c => c.Class == "FooClass" && c.Method == "BarMethod").FirstOrDefault();
                method.ShouldNotBeNull();
            }

            [Specification]
            public void Should_find_result_for_BarMethod_in_a_BarClass()
            {
                var method = _metrics.Where(c => c.Class == "FooClass" && c.Method == "BarMethod").FirstOrDefault();
                method.ShouldNotBeNull();
            }
        }
    }
}