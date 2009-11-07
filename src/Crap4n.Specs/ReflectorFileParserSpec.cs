using System;
using System.Collections.Generic;
using System.Linq;
using NBehave.Spec.NUnit;
using Crap4n.FileParser;
using NUnit.Framework;

namespace Crap4n.Specs
{
    public abstract class ReflectorFileParserSpec : SpecBase<ReflectorFileParser>
	{
		public const string ReflectorFile = @"ReflectorResult.xml";

		protected override ReflectorFileParser Establish_context()
		{
			return new ReflectorFileParser(new XmlFileLoader());
		}

		public class When_file_used_is_Reflector_result_file : ReflectorFileParserSpec
		{
			[Specification]
			public void Should_say_file_is_Reflector_result_file()
			{
				Sut.CanParseFile(ReflectorFile).ShouldBeTrue();
			}
		}

		public class When_file_used_is_not_Reflector_file : ReflectorFileParserSpec
		{
			[Specification]
			public void Should_say_file_is_SourceMonitor_result_file()
			{
				Sut.CanParseFile(PartCoverFileParserSpec.PartCoverFile).ShouldBeFalse();
			}
		}

		public class When_parsing_a_file : ReflectorFileParserSpec
		{
			private IEnumerable<CodeMetrics> _cc;

			protected override void Because_of()
			{
				_cc = Sut.ParseFile(ReflectorFile);
			}

			[Specification]
			public void Should_get_namespace()
			{
				var ns = from m in _cc
					where m.Method == "CompleteCoverage"
					select m.NameSpace;
				ns.Count().ShouldEqual(1);
				ns.First().ShouldEqual("Example");
			}

			[Specification]
			public void Should_get_class_name()
			{
				var name = from m in _cc
					where m.Method == "CompleteCoverage"
					select m.Class;
				name.Count().ShouldEqual(1);
				name.First().ShouldEqual("Example");
			}

			[Specification]
			public void Should_get_Method_name()
			{
				var methodName = from m in _cc
					where m.Method == "CompleteCoverage"
					select m.Method;
				methodName.Count().ShouldEqual(1);
			}

			[Specification]
            [Ignore("Reflector does not Output source file (it works directly on assemblies")]
			public void Should_get_SourceFile()
			{
				var name = from m in _cc
					where m.Method == "CompleteCoverage"
					select m.SourceFile;
				name.Count().ShouldEqual(1);
				name.First().ShouldEqual("Example.cs");
			}

			[Specification]
            [Ignore("Reflector does not Output source file (it works directly on assemblies")]
            public void Should_get_SourceFile_line()
			{
				var name = from m in _cc
					where m.Method == "CompleteCoverage"
					select m.SourceFileLineNumber;
				name.Count().ShouldEqual(1);
				name.First().ShouldEqual(34);
			}
		}

		public class When_parsing_a_file_with_class_that_has_explicit_interface_implementation : ReflectorFileParserSpec
		{
			private IEnumerable<CodeMetrics> _cc;

			protected override void Because_of()
			{
				_cc = Sut.ParseFile(ReflectorFile);
			}

			[Specification]
			public void Should_get_class_name()
			{
				var ns = from m in _cc
					where m.Method == "CompleteCoverage"
					select m.Class;
				ns.Count().ShouldEqual(1);
				ns.First().ShouldEqual("Example");
			}

			[Specification]
			public void Should_get_Method_name()
			{
				var methodName = from m in _cc
					where m.Method == "CompleteCoverage"
					select m.Method;
				methodName.Count().ShouldEqual(1);
			}
		}
		
		public class When_parsing_a_file_with_class_that_has_a_property : ReflectorFileParserSpec
		{
			private IEnumerable<CodeMetrics> _cc;

			protected override void Because_of()
			{
				_cc = Sut.ParseFile(ReflectorFile);
			}

			[Specification]
			public void Should_get_class_name()
			{
				var ns = from m in _cc
					where m.Method == "get_BarProp"
					select m.Class;
				ns.Count().ShouldEqual(1);
				ns.First().ShouldEqual("Example");
			}

			[Specification]
			public void Should_get_get_Property_name()
			{
				var name = from m in _cc
					where m.Method == "get_BarProp"
					select m.Method;
				name.Count().ShouldEqual(1);
			}

			[Specification]
			public void Should_get_set_Property_name()
			{
				var name = from m in _cc
					where m.Method == "set_BarProp"
					select m.Method;
				name.Count().ShouldEqual(1);
			}
		}
		
		public class When_parsing_a_file_with_a_generic_method : ReflectorFileParserSpec
		{
			private IEnumerable<CodeMetrics> _cc;

			protected override void Because_of()
			{
				_cc = Sut.ParseFile(ReflectorFile);
			}

			[Specification]
			public void Should_get_Method_name()
			{
				var methodName = from m in _cc
								where m.Method == "GenericMethod"
								select m.Method;
				methodName.Count().ShouldEqual(1);
			}
		}
	}
}
