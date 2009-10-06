using System.Xml;
using Crap4n.FileParser;
using NBehave.Spec.NUnit;
using NUnit.Framework;


namespace Crap4n.Specs
{
	public abstract class PartCoverXmlNodeParserSpec : SpecBase
	{
		PartCoverXmlNodeParser _partCoverXmlNodeParser;
		
		protected override void Establish_context()
		{
			_partCoverXmlNodeParser = new PartCoverXmlNodeParser();
		}

		private XmlNode BuildNode(string methodName, int timesVisited)
		{
			XmlDocument xml = new XmlDocument();
			XmlNode node = xml.CreateElement("method");
			AddAttribute("name", methodName, xml, node);
			AddAttribute("sig", "int  (int)", xml, node);
			AddAttribute("sig", "int  (int)", xml, node);
			AddAttribute("flags", "134", xml, node);
			AddAttribute("iflags", "0", xml, node);
			var code = xml.CreateElement("code");
			node.AppendChild(code);
			AddPtNode(timesVisited, xml, code, 1);			
			AddPtNode(timesVisited, xml, code, 2);			
			return node;
		}

		void AddPtNode(int timesVisited, XmlDocument xml, XmlElement code, int lineNumber)
		{
			var pt = xml.CreateElement("pt");
			code.AppendChild(pt);
			AddAttribute("visit", timesVisited.ToString(), xml, pt);
			AddAttribute("pos", "5", xml, pt);
			AddAttribute("len", "6", xml, pt);
			AddAttribute("sl", lineNumber.ToString(), xml, pt);
		}
		
		void AddAttribute(string attribName, string value, XmlDocument xml, XmlNode node)
		{
			node.Attributes.Append(xml.CreateAttribute(attribName));
			node.Attributes[attribName].Value = value;
		}
		
		[TestFixture]
		public class When_calculating_code_coverage_for_property_or_empty_method : PartCoverXmlNodeParserSpec
		{
			[Specification]
			public void Should_get_100_percent_codeCoverage_for_visited_property()
			{
				int visited = 1;
				XmlNode node = BuildNode("Foo", visited);
				Percent coverage = _partCoverXmlNodeParser.GetCoveragePercent(node);
				coverage.Value.ShouldEqual(100);
			}

			[Specification]
			public void Should_get_0_percent_codeCoverage_for_non_visited_property()
			{
				int visited = 0;
				XmlNode node = BuildNode("Foo", visited);
				Percent coverage = _partCoverXmlNodeParser.GetCoveragePercent(node);
				coverage.Value.ShouldEqual(0);
			}
		}
	}
}
