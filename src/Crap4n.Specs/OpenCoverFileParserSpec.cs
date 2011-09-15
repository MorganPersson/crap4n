using System.Collections.Generic;
using System.Linq;
using Crap4n.FileParser;
using NBehave.Spec.NUnit;
using NUnit.Framework;

namespace Crap4n.Specs
{
    //turn of shadowcopy when you run these tests
    public abstract class OpenCoverFileParserSpec : SpecBase<OpenCoverFileParser>
    {
        public const string OpenCoverFile = @"OpenCoverResult1.xml";

        public abstract class When_playing_role_of_IFileParser_of_CodeCoverage : SpecBase<OpenCoverFileParser>
        {
            protected override OpenCoverFileParser Establish_context()
            {
                return new OpenCoverFileParser(new XmlFileLoader());
            }

            public class When_file_used_is_OpenCover_file : When_playing_role_of_IFileParser_of_CodeCoverage
            {
                [Specification]
                public void Should_say_file_is_OpenCover_result_file()
                {
                    Sut.CanParseFile(OpenCoverFile).ShouldBeTrue();
                }
            }

            public class When_file_used_is_not_OpenCover_file : When_playing_role_of_IFileParser_of_CodeCoverage
            {
                [Specification]
                public void Should_say_file_is_not_OpenCover_result_file()
                {
                    Sut.CanParseFile(SourceMonitorFileParserSpec.SourceMonitorFile).ShouldBeFalse();
                }
            }

            public class When_parsing_a_file : When_playing_role_of_IFileParser_of_CodeCoverage
            {
                private IEnumerable<CodeCoverage> _coverage;

                protected override void Because_of()
                {
                    _coverage = Sut.ParseFile(OpenCoverFile);
                }

                [Specification]
                public void Should_get_namespace()
                {
                    var ns = from m in _coverage
                             where m.Method == "CompleteCoverage"
                             select m.NameSpace;
                    ns.Count().ShouldEqual(1);
                    ns.First().ShouldEqual("Example");
                }

                [Specification]
                public void Should_get_class_name()
                {
                    var ns = from m in _coverage
                             where m.Method == "CompleteCoverage"
                             select m.Class;
                    ns.Count().ShouldEqual(1);
                    ns.First().ShouldEqual("Example");
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
                    // Is there a way to detct brace-only lines and remove them?
                    //Assert.AreEqual(42.86, coverage.CoveragePercent, 0.01);
                    Assert.AreEqual(50.0, coverage.CoveragePercent, 0.01);
                }

                [Specification]
                public void Should_get_signature()
                {
                    var methodName = from m in _coverage
                                     where m.Method == "CompleteCoverage"
                                     select m.MethodSignature;
                    methodName.First().ShouldEqual("System.Int32 CompleteCoverage(System.Int32,System.Int32,System.Int32)");
                }
       
                [Specification]
                public void Should_not_include_ctor_if_no_FileRef()
                {
                    var ctor = _coverage.FirstOrDefault(m => m.Method == "ctor");
                    Assert.IsNull(ctor);
                }
            }

            public class When_parsing_a_file_with_explicit_interface_implementation : When_playing_role_of_IFileParser_of_CodeCoverage
            {
                private IEnumerable<CodeCoverage> _coverage;

                protected override void Because_of()
                {
                    _coverage = Sut.ParseFile(OpenCoverFile);
                }

                [Specification]
                public void Should_get_namespace()
                {
                    var ns = from m in _coverage
                             where m.Method == "FooMethod"
                             select m.NameSpace;
                    ns.Count().ShouldEqual(1);
                    ns.First().ShouldEqual("Example");
                }

                [Specification]
                public void Should_get_class_name()
                {
                    var ns = from m in _coverage
                             where m.Method == "FooMethod"
                             select m.Class;
                    ns.Count().ShouldEqual(1);
                    ns.First().ShouldEqual("Example");
                }

                [Specification]
                public void Should_get_Method_name()
                {
                    var methodName = from m in _coverage
                                     where m.Method == "FooMethod"
                                     select m.Method;
                    methodName.Count().ShouldEqual(1);
                }

                [Specification]
                public void Should_get_signature()
                {
                    var methodName = from m in _coverage
                                     where m.Method == "FooMethod"
                                     select m.MethodSignature;
                    methodName.Count().ShouldEqual(1);
                    methodName.First().ShouldEqual("System.Int32 FooMethod(System.Int32)");
                }
            }
        }

        public abstract class When_playing_role_of_IFileParser_of_CodeMetrics : SpecBase<IFileParser<CodeMetrics>>
        {
            protected override IFileParser<CodeMetrics> Establish_context()
            {
                return new OpenCoverFileParser(new XmlFileLoader());
            }

            public class When_file_used_is_NCover3_result_file : When_playing_role_of_IFileParser_of_CodeMetrics
            {
                [Specification]
                public void Should_say_file_is_OpenCover_result_file()
                {
                    Sut.CanParseFile(OpenCoverFile).ShouldBeTrue();
                }
            }

            public class When_file_used_is_not_OpenCover_file : When_playing_role_of_IFileParser_of_CodeMetrics
            {
                [Specification]
                public void Should_say_file_is_not_NCoverFile_result_file()
                {
                    Sut.CanParseFile(PartCoverFileParserSpec.PartCoverFile).ShouldBeFalse();
                }
            }

            public class When_parsing_a_file : When_playing_role_of_IFileParser_of_CodeMetrics
            {
                private IEnumerable<CodeMetrics> _coverage;

                protected override void Because_of()
                {
                    _coverage = Sut.ParseFile(OpenCoverFile);
                }

                [Specification]
                public void Should_get_namespace()
                {
                    var ns = from m in _coverage
                             where m.Method == "CompleteCoverage"
                             select m.NameSpace;
                    ns.Count().ShouldEqual(1);
                    ns.First().ShouldEqual("Example");
                }

                [Specification]
                public void Should_get_class_name()
                {
                    var name = from m in _coverage
                               where m.Method == "CompleteCoverage"
                               select m.Class;
                    name.Count().ShouldEqual(1);
                    name.First().ShouldEqual("Example");
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
                public void Should_get_SourceFile()
                {
                    var name = from m in _coverage
                               where m.Method == "CompleteCoverage"
                               select m.SourceFile;
                    name.Count().ShouldEqual(1);
                    name.First().ShouldEndWith("Example.cs");
                }

                [Specification]
                public void Should_get_SourceFile_line()
                {
                    var name = from m in _coverage
                               where m.Method == "CompleteCoverage"
                               select m.SourceFileLineNumber;
                    name.Count().ShouldEqual(1);
                    name.First().ShouldEqual(57);
                }

                [Specification]
                public void Should_get_CyclomaticComplexity()
                {
                    var complexity = _coverage
                        .Where(m => m.Method == "CompleteCoverage")
                        .First().CyclomaticComplexity;
                    complexity.ShouldEqual(4);
                }
            }

            public class When_parsing_a_file_with_class_that_has_explicit_interface_implementation :
                When_playing_role_of_IFileParser_of_CodeMetrics
            {
                private IEnumerable<CodeMetrics> _coverage;

                protected override void Because_of()
                {
                    _coverage = Sut.ParseFile(OpenCoverFile);
                }

                [Specification]
                public void Should_get_class_name()
                {
                    var ns = from m in _coverage
                             where m.Method == "CompleteCoverage"
                             select m.Class;
                    ns.Count().ShouldEqual(1);
                    ns.First().ShouldEqual("Example");
                }

                [Specification]
                public void Should_get_Method_name()
                {
                    var methodName = from m in _coverage
                                     where m.Method == "CompleteCoverage"
                                     select m.Method;
                    methodName.Count().ShouldEqual(1);
                }
            }

            public class When_parsing_a_file_with_class_that_has_a_property :
                When_playing_role_of_IFileParser_of_CodeMetrics
            {
                private IEnumerable<CodeMetrics> _coverage;

                protected override void Because_of()
                {
                    _coverage = Sut.ParseFile(OpenCoverFile);
                }

                [Specification]
                public void Should_get_class_name()
                {
                    var ns = from m in _coverage
                             where m.Method == "get_BarProp"
                             select m.Class;
                    ns.Count().ShouldEqual(1);
                    ns.First().ShouldEqual("Example");
                }

                [Specification]
                public void Should_get_get_Property_name()
                {
                    var name = from m in _coverage
                               where m.Method == "get_BarProp"
                               select m.Method;
                    name.Count().ShouldEqual(1);
                }

                [Specification]
                public void Should_get_set_Property_name()
                {
                    var name = from m in _coverage
                               where m.Method == "set_BarProp"
                               select m.Method;
                    name.Count().ShouldEqual(1);
                }
            }
        }

    }
}