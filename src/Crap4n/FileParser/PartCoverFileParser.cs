using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace Crap4n.FileParser
{
	public class PartCoverFileParser : IFileParser<CodeCoverage>
	{
		private readonly IXmlFileLoader _xmlFileLoader;
		private readonly PartCoverXmlNodeParser _partCoverXmlNodeParser = new PartCoverXmlNodeParser();
		
		public PartCoverFileParser(IXmlFileLoader xmlFileLoader)
		{
			_xmlFileLoader = xmlFileLoader;
		}

		public bool CanParseFile(string fileName)
		{
			var xml = _xmlFileLoader.LoadXmlDocument(fileName);
			var node = xml.SelectSingleNode("/PartCoverReport");
			return node != null;
		}

		public IEnumerable<CodeCoverage> ParseFile(string fileName)
		{
			var coverage = new List<CodeCoverage>();
			var xml = _xmlFileLoader.LoadXmlDocument(fileName);
			XmlNodeList methods = xml.SelectNodes(@"/PartCoverReport/type/method");
			for (int i = 0; i < methods.Count; i++)
			{
				if (_partCoverXmlNodeParser.IsValidMethod(methods[i]))
				{
					var cc = new CodeCoverage
					{
						Method = _partCoverXmlNodeParser.GetMethodName(methods[i]),
						Class = _partCoverXmlNodeParser.GetClassName(methods[i]),
						NameSpace = _partCoverXmlNodeParser.GetNamespace(methods[i]),
						CoveragePercent = _partCoverXmlNodeParser.GetCoveragePercent(methods[i]),
						MethodSignature = _partCoverXmlNodeParser.GetMethodSignature(methods[i])
					};
					coverage.Add(cc);
				}
			}
			return coverage;
		}
	}
	
	public class PartCoverXmlNodeParser : CodeCoverageXmlNodeParser
	{
		public string GetMethodSignature(XmlNode method)
		{
			return  GetMethodSignature(method, "sig");
		}

		public Percent GetCoveragePercent(XmlNode node)
		{
			return GetCoveragePercent(node, "visit");
		}
		
		protected override List<XmlNode> GetAllNodes(XmlNode node)
		{
			var ptTempNode = node.SelectNodes("code/pt");
			var ptNodes = new List<XmlNode>();
			for (int i = 0; i < ptTempNode.Count; i++)
			{
				if (ptTempNode[i].Attributes["sl"] != null && ptTempNode[i].Attributes["pos"].Value != "0")
					ptNodes.Add(ptTempNode[i]);
			}
			return ptNodes;
		}
		
		protected override void RemoveStartAndEndBrace(List<XmlNode> ptNodes)
		{
			if (ptNodes.Count() > 0)
				ptNodes.Remove(ptNodes.First());
			if (ptNodes.Count() > 0)
				ptNodes.Remove(ptNodes.Last());
		}
	}
}