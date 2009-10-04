using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace Crap4n.FileParser
{
	
	public class NCover3FileParser : IFileParser<CodeCoverage>, IFileParser<CodeMetrics>
	{
		private IXmlFileLoader _xmlFileLoader;
		private readonly NCover3XmlNodeParser _ncover3XmlNodeParser = new NCover3XmlNodeParser();
		
		public NCover3FileParser(IXmlFileLoader xmlFileLoader)
		{
			_xmlFileLoader = xmlFileLoader;
		}
		
		bool IsNCover3File(string fileName)
		{
			var xml = _xmlFileLoader.LoadXmlDocument(fileName);
			var node = xml.SelectSingleNode("/coverage");
			if (node == null) return false;
			var attrib = node.Attributes["driverVersion"];
			if (attrib != null && attrib.Value.StartsWith("3")) return true;
			return false;
		}

		bool IFileParser<CodeCoverage>.CanParseFile(string fileName)
		{
			return IsNCover3File(fileName);
		}
		
		IEnumerable<CodeCoverage> IFileParser<CodeCoverage>.ParseFile(string fileName)
		{
			var coverage = new List<CodeCoverage>();
			var xml = _xmlFileLoader.LoadXmlDocument(fileName);
			XmlNodeList methods = xml.SelectNodes(@"/coverage/module/class/method");
			for (int i = 0; i < methods.Count; i++)
			{
				if (_ncover3XmlNodeParser.IsValidMethod(methods[i]))
				{
					var cc = new CodeCoverage
					{
						Method = _ncover3XmlNodeParser.GetMethodName(methods[i]),
						Class = _ncover3XmlNodeParser.GetClassName(methods[i]),
						NameSpace = _ncover3XmlNodeParser.GetNamespace(methods[i]),
						CoveragePercent = _ncover3XmlNodeParser.GetCoveragePercent(methods[i]),
						MethodSignature = _ncover3XmlNodeParser.GetMethodSignature(methods[i])
					};
					coverage.Add(cc);
				}
			}
			return coverage;
		}
		
		IEnumerable<CodeMetrics> IFileParser<CodeMetrics>.ParseFile(string fileName)
		{
			var metrics = new List<CodeMetrics>();
			var xml = _xmlFileLoader.LoadXmlDocument(fileName);
			XmlNodeList methods = xml.SelectNodes(@"/coverage/module/class/method");
			for (int i = 0; i < methods.Count; i++)
			{
				if (_ncover3XmlNodeParser.IsValidMethod(methods[i]))
				{
					var cc = new CodeMetrics
					{
						Method = _ncover3XmlNodeParser.GetMethodName(methods[i]),
						Class = _ncover3XmlNodeParser.GetClassName(methods[i]),
						NameSpace = _ncover3XmlNodeParser.GetNamespace(methods[i]),
						CyclomaticComplexity=_ncover3XmlNodeParser.GetCyclomaticComplexity(methods[i]),
						SourceFileLineNumber = _ncover3XmlNodeParser.GetLineNumber(methods[i]),
						SourceFile = _ncover3XmlNodeParser.GetSourceFile(methods[i])
					};
					metrics.Add(cc);
				}
			}
			return metrics;
		}

		bool IFileParser<CodeMetrics>.CanParseFile(string fileName)
		{
			return IsNCover3File(fileName);
		}
	}
	
	
	public class NCover3XmlNodeParser : CodeCoverageXmlNodeParser
	{
		public int GetCyclomaticComplexity(XmlNode node)
		{
			return Int32.Parse(node.Attributes["cc"].Value);
		}
		
		public string GetSourceFile(XmlNode node)
		{
			var nodes = GetValidNodes(node);
			if (nodes.Count()==0)
				return string.Empty;
			string doc = nodes.First().Attributes["doc"].Value;
			var docNode = node.OwnerDocument.SelectSingleNode(string.Format(@"coverage/documents/doc[@id='{0}']",doc));
			return Path.GetFileName(docNode.Attributes["url"].Value);
		}
		
		public int GetLineNumber(XmlNode node)
		{
			var r = from n in GetAllNodes(node)
				where Int32.Parse(n.Attributes["l"].Value)>0
				select n.Attributes["l"].Value;
			if (r.Any())
				return Int32.Parse(r.Min()) - 2;
			return -1;
		}
		
		public string GetMethodSignature(XmlNode method)
		{
			return  GetMethodSignature(method, "signature");
		}

		public Percent GetCoveragePercent(XmlNode node)
		{
			return GetCoveragePercent(node, "vc");
		}

		protected override List<XmlNode> GetAllNodes(XmlNode node)
		{
			var ptTempNode = node.SelectNodes("seqpnt");
			var ptNodes = new List<XmlNode>();
			for (int i = 0; i < ptTempNode.Count; i++)
			{
				if (ptTempNode[i].Attributes["l"] != null && ptTempNode[i].Attributes["l"].Value != "0"
				    && ptTempNode[i].Attributes["ex"].Value == "false")
					ptNodes.Add(ptTempNode[i]);
			}
			return ptNodes;
		}
		
		protected override void RemoveStartAndEndBrace(List<XmlNode> ptNodes)
		{
			if (ptNodes.Count() > 0)
				ptNodes.Remove(ptNodes.Last());
		}
	}	
}