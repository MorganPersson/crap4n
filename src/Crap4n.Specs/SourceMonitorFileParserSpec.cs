using System.Collections.Generic;
using System.Linq;
using Crap4n.FileParser;
using NBehave.Spec.NUnit;
using NUnit.Framework;

namespace Crap4n.Specs
{
	public abstract class SourceMonitorFileParserSpec : SpecBase<SourceMonitorFileParser>
	{
		public const string SourceMonitorFile = @"SourceMonitorResult.xml";

		protected override SourceMonitorFileParser Establish_context()
		{
			return new SourceMonitorFileParser(new XmlFileLoader());
		}

		public class When_file_used_is_SourceMonitor_result_file : SourceMonitorFileParserSpec
		{
			[Specification]
			public void Should_say_file_is_PartCover_result_file()
			{
				Sut.CanParseFile(SourceMonitorFile).ShouldBeTrue();
			}
		}

		public class When_file_used_is_not_SourceMonitor_file : SourceMonitorFileParserSpec
		{
			[Specification]
			public void Should_say_file_is_SourceMonitor_result_file()
			{
				Sut.CanParseFile(PartCoverFileParserSpec.PartCoverFile).ShouldBeFalse();
			}
		}

		public class When_parsing_a_file : SourceMonitorFileParserSpec
		{
			private IEnumerable<CodeMetrics> _coverage;

			protected override void Because_of()
			{
				_coverage = Sut.ParseFile(SourceMonitorFile);
			}

			[Specification, Ignore("SourceMonitor doesnt provide the namespace")]
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
				name.First().ShouldEqual("Example.cs");
			}

			[Specification]
			public void Should_get_SourceFile_line()
			{
				var name = from m in _coverage
					where m.Method == "CompleteCoverage"
					select m.SourceFileLineNumber;
				name.Count().ShouldEqual(1);
				name.First().ShouldEqual(34);
			}
		}

		public class When_parsing_a_file_with_class_that_has_explicit_interface_implementation : SourceMonitorFileParserSpec
		{
			private IEnumerable<CodeMetrics> _coverage;

			protected override void Because_of()
			{
				_coverage = Sut.ParseFile(SourceMonitorFile);
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
		
		public class When_parsing_a_file_with_class_that_has_a_property : SourceMonitorFileParserSpec
		{
			private IEnumerable<CodeMetrics> _coverage;

			protected override void Because_of()
			{
				_coverage = Sut.ParseFile(SourceMonitorFile);
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
		
		public class When_parsing_a_file_with_a_generic_method : SourceMonitorFileParserSpec
		{
			private IEnumerable<CodeMetrics> _coverage;

			protected override void Because_of()
			{
				_coverage = Sut.ParseFile(SourceMonitorFile);
			}

			[Specification, Test]
			public void Should_get_Method_name()
			{
				var methodName = from m in _coverage
								where m.Method == "GenericMethod"
								select m.Method;
				methodName.Count().ShouldEqual(1);
			}
		}
	}
}