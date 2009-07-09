using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace Crap4n.FileParser
{
    public class PartCoverFileParser : IFileParser<CodeCoverage>
    {
        private readonly IXmlFileLoader _xmlFileLoader;

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
            	if (IsValidMethod(methods[i]))
                {
                    var cc = new CodeCoverage
                                {
                                    Method = GetMethodName(methods[i]),
                                    Class = GetClassName(methods[i]),
                                    NameSpace = GetNamespace(methods[i]),
                                    CoveragePercent = GetCoveragePercent(methods[i]),
                                    MethodSignature = GetMethodSignature(methods[i])
                                };
                    coverage.Add(cc);
                }
            }
            return coverage;
        }
        
        private bool IsValidMethod(XmlNode method)
        {
        	return GetValidNodes(method).Count > 0
        		|| GetMethodName(method) != ".ctor";
        }

        private string GetMethodSignature(XmlNode method)
        {
            return method.Attributes["sig"].Value;
        }

        private string GetNamespace(XmlNode node)
        {
            var name = node.ParentNode.Attributes["name"].Value;

            var lastDot = name.LastIndexOf('.');
            return name.Substring(0, lastDot);
        }

        private string GetMethodName(XmlNode node)
        {
        	string name = node.Attributes["name"].Value;
        	int dotPos = name.LastIndexOf('.');
        	if (dotPos > 0)
        		name = name.Substring(dotPos + 1);
        	return name;
        }

        private string GetClassName(XmlNode node)
        {
            var name = node.ParentNode.Attributes["name"].Value;
            var lastDot = name.LastIndexOf('.');
            return name.Substring(lastDot + 1);
        }

        private Percent GetCoveragePercent(XmlNode node)
        {
            List<XmlNode> ptNodes = GetValidNodes(node);
            if (ptNodes.Count == 0)
                return 100.Percent();

            var visited = from n in ptNodes
                          where n.Attributes["visit"].Value != "0"
                          select n;
            double coverage = visited.Count() / Convert.ToDouble(ptNodes.Count);
            return coverage.ToPercent();
        }

        private List<XmlNode> GetValidNodes(XmlNode node)
        {
            var ptTempNode = node.SelectNodes("code/pt");
            var ptNodes = new List<XmlNode>();
            for (int i = 0; i < ptTempNode.Count; i++)
            {
                if (ptTempNode[i].Attributes["sl"] != null && ptTempNode[i].Attributes["pos"].Value != "0")
                    ptNodes.Add(ptTempNode[i]);
            }
            if (ptNodes.Count() > 0)
                ptNodes.Remove(ptNodes.First());
            if (ptNodes.Count() > 0)
                ptNodes.Remove(ptNodes.Last());
            return ptNodes;
        }
    }
}