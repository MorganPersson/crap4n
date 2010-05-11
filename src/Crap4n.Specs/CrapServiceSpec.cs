using System;
using System.Collections.Generic;
using System.Linq;
using Crap4n.FileParser;
using NBehave.Spec.NUnit;
using NUnit.Framework;

namespace Crap4n.Specs
{
    [TestFixture]
    public abstract class CrapServiceSpec : SpecBase<ICrapService>
    {
        private const int CrapThreshold = 30;
        private const string PartCoverResultFile = @"PartCoverResult.xml";
        private const string SourceMonitorResultFile = @"SourceMonitorResult.xml";

        public class When_no_CodeMetric_implementation_found : CrapServiceSpec
        {
            protected override ICrapService Establish_context()
            {
                var metricsParserFinder = new MetricsParserFinder(new List<IFileParser<CodeMetrics>>(), new List<IFileParser<CodeCoverage>>());
                return new CrapService(metricsParserFinder);                
            }

            [Specification, ExpectedException(typeof(NotSupportedException))]
            public void Should_Throw_exception()
            {
                Sut.BuildResult(PartCoverResultFile, SourceMonitorResultFile, CrapThreshold);
            }
        }

        public class When_building_the_result : CrapServiceSpec
        {
            protected override ICrapService Establish_context()
            {
                var codeMetrics = new List<IFileParser<CodeMetrics>> { new SourceMonitorFileParser(new XmlFileLoader()) };
                IEnumerable<IFileParser<CodeCoverage>> codeCoverage = new List<IFileParser<CodeCoverage>> { new PartCoverFileParser(new XmlFileLoader()) };
                var metricsParserFinder = new MetricsParserFinder(codeMetrics, codeCoverage);
                return new CrapService(metricsParserFinder);
            }

            private IEnumerable<Crap> _crapResult;
            protected override void Because_of()
            {
                _crapResult = Sut.BuildResult(PartCoverResultFile, SourceMonitorResultFile, CrapThreshold);
            }

            [Specification]
            public void Should_have_all_results()
            {
                _crapResult.Count().ShouldEqual(10);
            }
        }
    }
}