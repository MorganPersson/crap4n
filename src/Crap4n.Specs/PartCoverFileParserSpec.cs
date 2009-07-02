using System.Collections.Generic;
using System.Linq;
using Crap4n.FileParser;
using NBehave.Spec.NUnit;
using NUnit.Framework;

namespace Crap4n.Specs
{
    //turn of shadowcopy when you run these tests
    public abstract class PartCoverFileParserSpec : SpecBase<PartCoverFileParser>
    {
        public const string PartCoverFile = @"PartCoverResult.xml";

        protected override PartCoverFileParser Establish_context()
        {
            return new PartCoverFileParser(new XmlFileLoader());
        }

        public class When_file_used_is_PartCover_file : PartCoverFileParserSpec
        {
            [Specification]
            public void Should_say_file_is_PartCover_result_file()
            {
                Sut.CanParseFile(PartCoverFile).ShouldBeTrue();
            }
        }

        public class When_file_used_is_not_PartCover_file : PartCoverFileParserSpec
        {
            [Specification]
            public void Should_say_file_is_PartCover_result_file()
            {
                Sut.CanParseFile(SourceMonitorFileParserSpec.SourceMonitorFile).ShouldBeFalse();
            }
        }

        public class When_parsing_a_file : PartCoverFileParserSpec
        {
            private IEnumerable<CodeCoverage> _coverage;

            protected override void Because_of()
            {
                _coverage = Sut.ParseFile(PartCoverFile);
            }

            [Specification]
            public void Should_get_namespace()
            {
                var ns = from m in _coverage
                         where m.Method == "CompleteCoverage"
                         select m.NameSpace;
                ns.Count().ShouldEqual(1);
                ns.First().ShouldEqual("TestAssembly");
            }

            [Specification]
            public void Should_get_class_name()
            {
                var ns = from m in _coverage
                         where m.Method == "CompleteCoverage"
                         select m.Class;
                ns.Count().ShouldEqual(1);
                ns.First().ShouldEqual("Tested");
            }

            [Specification]
            public void Should_get_Method_name()
            {
                var methodName = from m in _coverage
                                 where m.Method == "CompleteCoverage"
                                 select m.Method;
                methodName.Count().ShouldEqual(1);
            }

            [Specification]
            public void Should_get_codeCoverage()
            {
                var coverage = (from m in _coverage
                                where m.Method == "SemiCoverage"
                                select m).FirstOrDefault();
                coverage.ShouldNotBeNull();
                Assert.AreEqual(42.86,coverage.CoveragePercent, 0.01);
            }

            [Specification]
            public void Should_get_signature()
            {
                var methodName = from m in _coverage
                                 where m.Method == "CompleteCoverage"
                                 select m.MethodSignature;
                methodName.First().ShouldEqual("int  (int, int, int)");
            }
        }
    }
}