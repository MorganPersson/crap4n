using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace Crap4n.FileParser
{
	/// <summary>
	/// Supports NCover 1.5.8
	/// </summary>
	public class NCoverFileParser : IFileParser<CodeCoverage>
	{
		private readonly IXmlFileLoader _xmlFileLoader;
		private readonly NCoverXmlNodeParser _ncoverXmlNodeParser = new NCoverXmlNodeParser();

		public NCoverFileParser(IXmlFileLoader xmlFileLoader)
		{
			_xmlFileLoader=xmlFileLoader;
		}
		
		public bool CanParseFile(string fileName)
		{
			var xml = _xmlFileLoader.LoadXmlDocument(fileName);
			var node = xml.SelectSingleNode("/coverage");
			if (node==null)
				return false;
			var attrib = node.Attributes["driverVersion"];
			if (attrib!=null && attrib.Value.StartsWith("1"))
				return true;
			return false;
		}
		
		public IEnumerable<CodeCoverage> ParseFile(string fileName)
		{
			var coverage = new List<CodeCoverage>();
			var xml = _xmlFileLoader.LoadXmlDocument(fileName);
			XmlNodeList methods = xml.SelectNodes(@"/coverage/module/method");
			for (int i = 0; i < methods.Count; i++)
			{
				if (_ncoverXmlNodeParser.IsValidMethod(methods[i]))
				{
					var cc = new CodeCoverage
					{
						Method = _ncoverXmlNodeParser.GetMethodName(methods[i]),
						Class = _ncoverXmlNodeParser.GetClassName(methods[i]),
						NameSpace = _ncoverXmlNodeParser.GetNamespace(methods[i]),
						CoveragePercent = _ncoverXmlNodeParser.GetCoveragePercent(methods[i]),
						MethodSignature = _ncoverXmlNodeParser.GetMethodSignature(methods[i])
					};
					coverage.Add(cc);
				}
			}
			return coverage;
		}
	}
	
	public class NCoverXmlNodeParser
	{
		public bool IsValidMethod(XmlNode method)
		{
			return method.Attributes["excluded"].Value=="false"
				|| GetMethodName(method) != ".ctor";
		}

		//not in the xml file
		public string GetMethodSignature(XmlNode method)
		{
			return string.Empty;
		}

		public string GetNamespace(XmlNode node)
		{
			var name = node.Attributes["class"].Value;

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
			var name = node.Attributes["class"].Value;
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
				where n.Attributes["visitcount"].Value != "0"
				&& n.Attributes["excluded"].Value == "false"
				select n;
			double coverage = visited.Count() / Convert.ToDouble(ptNodes.Count);
			return coverage.ToPercent();
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
		
		public List<XmlNode> GetValidNodes(XmlNode node)
		{
			var nodes = node.SelectNodes("seqpnt");
			List<XmlNode> nodeList = NodesAsList(nodes);
			var nodesSorted = SortNodes(nodeList);
			var nrOfNodes = nodesSorted.Count();
			return nodesSorted.Take(nrOfNodes-1).ToList();
		}

		List<XmlNode> NodesAsList(XmlNodeList nodes)
		{
			var nodeList = new List<XmlNode>();
			for (int i = 0; i < nodes.Count; i++) {
				nodeList.Add(nodes[i]);
			}
			return nodeList;
		}

		IEnumerable<XmlNode> SortNodes(IEnumerable<XmlNode> nodes)
		{
			var nodesSorted = from n in nodes
				orderby Int32.Parse(n.Attributes["line"].Value)
				select n;
			return nodesSorted;
		}
	}
}
