using System;
using System.Collections.Generic;
using System.Linq;
using NBehave.Spec.NUnit;
using Crap4n.FileParser;
using NUnit.Framework;

namespace Crap4n.Specs
{
	public abstract class NCoverFileParserSpec : SpecBase<NCoverFileParser>
	{
		public const string NCoverFile = @"NCover15Result.xml";

		protected override NCoverFileParser Establish_context()
		{
			return new NCoverFileParser(new XmlFileLoader());
		}

		[TestFixture]
		public class When_file_used_is_NCover_file : NCoverFileParserSpec
		{
			[Specification]
			public void Should_say_file_is_NCover_result_file()
			{
				Sut.CanParseFile(NCoverFile).ShouldBeTrue();
			}
		}

		[TestFixture]
		public class When_file_used_is_not_PartCover_file : NCoverFileParserSpec
		{
			[Specification]
			public void Should_say_file_is_not_NCover_result_file()
			{
				Sut.CanParseFile(PartCoverFileParserSpec.PartCoverFile).ShouldBeFalse();
			}
		}

		[TestFixture]
		public class When_parsing_a_file : NCoverFileParserSpec
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

			[Specification,Test]
			public void Should_get_codeCoverage()
			{
				var coverage = (from m in _coverage
				                where m.Method == "SemiCoverage"
				                select m).FirstOrDefault();
				coverage.ShouldNotBeNull();
				Assert.AreEqual(42.86,coverage.CoveragePercent, 0.01);
			}

			[Specification]
			public void Should_get_empy_signature()
			{
				var methodName = from m in _coverage
					where m.Method == "CompleteCoverage"
					select m.MethodSignature;
				methodName.First().ShouldEqual("");
			}
		}
	}
}
