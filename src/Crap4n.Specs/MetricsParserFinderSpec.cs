using System.Collections.Generic;
using Crap4n.Specs.Fake;
using NBehave.Spec.NUnit;
using NUnit.Framework;

namespace Crap4n.Specs
{
    [TestFixture]
    public abstract class MetricsParserFinderSpec : SpecBase<MetricsParserFinder>
    {
        //private const string SourceMonitorFileName = "SourceMonitorDetails.xml";
        //private const string PartCoverFileName = "PartCoverResult.xml";

        public class When_resolving_CodeMetricsFileParser : MetricsParserFinderSpec
        {
            protected override MetricsParserFinder Establish_context()
            {
                var codeMetrics = new List<IFileParser<CodeMetrics>>(new[] {new FooCodeMetricFileParser()});
                return new MetricsParserFinder(codeMetrics, new List<IFileParser<CodeCoverage>>());
            }

            [Specification]
            public void Should_get_a_CodeMetricFileParser_instance()
            {
                var codeMetricsFileParser = Sut.FindFileParser<CodeMetrics>("Foo.xml");
                codeMetricsFileParser.ShouldBeInstanceOfType(typeof (FooCodeMetricFileParser));
            }
        }

        public class When_resolving_CodeCoverageFileParser : MetricsParserFinderSpec
        {
            protected override MetricsParserFinder Establish_context()
            {
                IEnumerable<IFileParser<CodeCoverage>> codeCoverage =
                    new List<IFileParser<CodeCoverage>>(new[] {new FooCodeCoverageFileParser()});
                return new MetricsParserFinder(new List<IFileParser<CodeMetrics>>(), codeCoverage);
            }

            [Specification]
            public void Should_get_a_CodeCoverageFileParser_instance()
            {
                var codeCoverageFileParser = Sut.FindFileParser<CodeCoverage>("Foo.xml");
                codeCoverageFileParser.ShouldBeInstanceOfType(typeof (FooCodeCoverageFileParser));
            }
        }

        public class When_having_multiple_CodeMetricFileParsers_registered : MetricsParserFinderSpec
        {
            protected override MetricsParserFinder Establish_context()
            {
                var codeMetrics =
                    new List<IFileParser<CodeMetrics>>(new IFileParser<CodeMetrics>[]
                                                           {
                                                               new FooCodeMetricFileParser(), new BarCodeMetricFileParser()
                                                           });
                return new MetricsParserFinder(codeMetrics, new List<IFileParser<CodeCoverage>>());
            }

            [Specification]
            public void Should_get_a_FooCodeMetricFileParser()
            {
                var codeMetricsFileParser = Sut.FindFileParser<CodeMetrics>("Foo.xml");
                codeMetricsFileParser.ShouldBeInstanceOfType(typeof (FooCodeMetricFileParser));
            }

            [Specification]
            public void Should_get_a_BarCodeMetricFileParser()
            {
                var codeMetricsFileParser = Sut.FindFileParser<CodeMetrics>("Bar.xml");
                codeMetricsFileParser.ShouldBeInstanceOfType(typeof (BarCodeMetricFileParser));
            }
        }
    }
}