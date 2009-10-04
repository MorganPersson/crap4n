using System.Collections.Generic;
using System.Linq;
using Crap4n.FileParser;
using NBehave.Spec.NUnit;
using NUnit.Framework;

namespace Crap4n.Specs
{
	public abstract class NCover3FileParserSpec
	{
		public const string NCoverFile = @"NCover32Result.xml";
		
		public abstract class When_playing_role_of_IFileParser_of_CodeCoverage : SpecBase<IFileParser<CodeCoverage>>
		{
			protected override IFileParser<CodeCoverage> Establish_context()
			{
				return new NCover3FileParser(new XmlFileLoader());
			}
			
			public class When_file_used_is_NCover3_file : When_playing_role_of_IFileParser_of_CodeCoverage
			{
				protected override IFileParser<CodeCoverage> Establish_context()
				{
					return base.Establish_context();
				}
				
				[Specification]
				public void Should_say_file_is_NCover3_result_file()
				{
					Sut.CanParseFile(NCoverFile).ShouldBeTrue();
				}
			}

			public class When_file_used_is_not_NCover3_file : When_playing_role_of_IFileParser_of_CodeCoverage
			{
				[Specification]
				public void Should_say_file_is_not_NCover3_result_file()
				{
					Sut.CanParseFile(PartCoverFileParserSpec.PartCoverFile).ShouldBeFalse();
				}
			}

			public class When_parsing_a_file : When_playing_role_of_IFileParser_of_CodeCoverage
			{
				private IEnumerable<CodeCoverage> _coverage;

				protected override void Because_of()
				{
					_coverage = Sut.ParseFile(NCoverFile);
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
					Assert.AreEqual(42.86,coverage.CoveragePercent, 0.01);
				}

				[Specification]
				public void Should_get_signature()
				{
					var methodName = from m in _coverage
						where m.Method == "CompleteCoverage"
						select m.MethodSignature;
					methodName.First().ShouldEqual("CompleteCoverage(System.Int32 a,System.Int32 b,System.Int32 c) : System.Int32");
				}
			}

			public class When_parsing_a_file_with_explicit_interface_implementation : When_playing_role_of_IFileParser_of_CodeCoverage
			{
				private IEnumerable<CodeCoverage> _coverage;

				protected override void Because_of()
				{
					_coverage = Sut.ParseFile(NCoverFile);
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
					methodName.First().ShouldEqual("Example.IFoo.FooMethod(System.Int32 a) : System.Int32");
				}
			}
		}
		
		public abstract class When_playing_role_of_IFileParser_of_CodeMetrics : SpecBase<IFileParser<CodeMetrics>>
		{
			protected override IFileParser<CodeMetrics> Establish_context()
			{
				return new NCover3FileParser(new XmlFileLoader());
			}
			
			public class When_file_used_is_NCover3_result_file : When_playing_role_of_IFileParser_of_CodeMetrics
			{
				[Specification]
				public void Should_say_file_is_PartCover_result_file()
				{
					Sut.CanParseFile(NCoverFile).ShouldBeTrue();
				}
			}

			public class When_file_used_is_not_NCover3_file : When_playing_role_of_IFileParser_of_CodeMetrics
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
					_coverage = Sut.ParseFile(NCoverFile);
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
					name.First().ShouldEqual("Tested.cs");
				}

				[Specification]
				public void Should_get_SourceFile_line()
				{
					var name = from m in _coverage
						where m.Method == "CompleteCoverage"
						select m.SourceFileLineNumber;
					name.Count().ShouldEqual(1);
					name.First().ShouldEqual(31);
				}
			}

			public class When_parsing_a_file_with_class_that_has_explicit_interface_implementation : When_playing_role_of_IFileParser_of_CodeMetrics
			{
				private IEnumerable<CodeMetrics> _coverage;

				protected override void Because_of()
				{
					_coverage = Sut.ParseFile(NCoverFile);
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
			
			public class When_parsing_a_file_with_class_that_has_a_property : When_playing_role_of_IFileParser_of_CodeMetrics
			{
				private IEnumerable<CodeMetrics> _coverage;

				protected override void Because_of()
				{
					_coverage = Sut.ParseFile(NCoverFile);
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