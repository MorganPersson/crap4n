using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace Crap4n.FileParser
{
	public abstract class CodeCoverageXmlNodeParser
	{
		protected abstract List<XmlNode> GetAllNodes(XmlNode node);
		protected abstract void RemoveStartAndEndBrace(List<XmlNode> nodes);

		public bool IsValidMethod(XmlNode method)
		{
			return GetValidNodes(method).Count > 0
				|| GetMethodName(method) != ".ctor";
		}
		
		protected string GetMethodSignature(XmlNode method, string attributeName)
		{
			return method.Attributes[attributeName].Value;
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

		protected Percent GetCoveragePercent(XmlNode node, string visitCountAttributeName)
		{
			List<XmlNode> ptNodes = GetValidNodes(node);
			if (ptNodes.Count == 0)
			{
				if (MethodVisited(node))
					return 100.Percent();
				return 0.Percent();
			}

			var visited = (from n in ptNodes
			               where n.Attributes[visitCountAttributeName].Value != "0"
			               select n).ToList();
			double coverage = visited.Count() / Convert.ToDouble(ptNodes.Count);
			return coverage.ToPercent();
		}

		public List<XmlNode> GetValidNodes(XmlNode node)
		{
			var ptNodes = GetAllNodes(node);
			RemoveStartAndEndBrace(ptNodes);
			return ptNodes;
		}
	
		protected bool MethodVisited(XmlNode node)
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
	}
}
