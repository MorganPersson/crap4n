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
	
	public class PartCoverXmlNodeParser
	{
		public bool IsValidMethod(XmlNode method)
		{
			return GetValidNodes(method).Count > 0
				|| GetMethodName(method) != ".ctor";
		}

		public string GetMethodSignature(XmlNode method)
		{
			return method.Attributes["sig"].Value;
		}

		public string GetNamespace(XmlNode node)
		{
			var name = node.ParentNode.Attributes["name"].Value;

			var lastDot = name.LastIndexOf('.');
			return name.Substring(0, lastDot);
		}

		public string GetMethodName(XmlNode node)
		{
			string name = node.Attributes["name"].Value;
			int dotPos = name.LastIndexOf('.');
			if (dotPos > 0)
				name = name.Substring(dotPos + 1);
			return name;
		}

		public string GetClassName(XmlNode node)
		{
			var name = node.ParentNode.Attributes["name"].Value;
			var lastDot = name.LastIndexOf('.');
			return name.Substring(lastDot + 1);
		}

		public Percent GetCoveragePercent(XmlNode node)
		{
			List<XmlNode> ptNodes = GetValidNodes(node);
			if (ptNodes.Count == 0)
			{
				if (MethodVisited(node))
					return 100.Percent();
				return 0.Percent();
			}

			var visited = from n in ptNodes
				where n.Attributes["visit"].Value != "0"
				select n;
			double coverage = visited.Count() / Convert.ToDouble(ptNodes.Count);
			return coverage.ToPercent();
		}

		public List<XmlNode> GetValidNodes(XmlNode node)
		{
			var ptNodes = GetAllNodes(node);
			RemoveStartAndEndBrace(ptNodes);			
			return ptNodes;
		}
		
		private List<XmlNode> GetAllNodes(XmlNode node)
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
		
		private bool MethodVisited(XmlNode node)
		{
			var ptTempNode = node.SelectNodes("code/pt");
			var ptNodes = new List<XmlNode>();
			for (int i = 0; i < ptTempNode.Count; i++)
			{
				if (ptTempNode[i].Attributes["visit"] != null 
				    && ptTempNode[i].Attributes["visit"].Value != "0")
					return true;
			}
			return false;
		}

		private void RemoveStartAndEndBrace(List<XmlNode> ptNodes)
		{
			if (ptNodes.Count() > 0) 
				ptNodes.Remove(ptNodes.First());
			if (ptNodes.Count() > 0) 
				ptNodes.Remove(ptNodes.Last());
		}
	}
}