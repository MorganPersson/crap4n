using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace Crap4n.FileParser
{
    public class OpenCoverFileParser : IFileParser<CodeCoverage>, IFileParser<CodeMetrics>
    {
        private readonly IXmlFileLoader _xmlFileLoader;

        public OpenCoverFileParser(IXmlFileLoader xmlFileLoader)
        {
            _xmlFileLoader = xmlFileLoader;
        }

        public bool CanParseFile(string fileName)
        {
            var xml = _xmlFileLoader.LoadXmlDocument(fileName);
            var node = xml.SelectSingleNode("/CoverageSession/Modules/Module");
            return node != null;
        }

        IEnumerable<CodeMetrics> IFileParser<CodeMetrics>.ParseFile(string fileName)
        {
            var coverage = new List<CodeMetrics>();
            var xml = _xmlFileLoader.LoadXmlDocument(fileName);
            var methods = xml.SelectNodes(@"/CoverageSession/Modules/Module/Classes/Class/Methods/Method");
            for (int i = 0; i < methods.Count; i++)
            {
                var fullName = GetFullName(methods[i]);
                if (IsValidMethod(methods[i]))
                {
                    var cc = new CodeMetrics
                                 {
                                     Method = GetMethodName(fullName),
                                     Class = GetClassName(fullName),
                                     NameSpace = GetNamespace(fullName),
                                     CyclomaticComplexity = GetCyclomaticComplexity(methods[i]),
                                     SourceFile = GetSourceFile(methods[i]),
                                     // might be possible
                                     SourceFileLineNumber = GetSourceFileLineNumber(methods[i]) // sl for first SeqPoint
                                 };
                    coverage.Add(cc);
                }
            }
            return coverage;
        }

        private static string GetFullName(XmlNode node)
        {
            string fullName = node.SelectSingleNode("Name").InnerText;
            return fullName;
        }

        private int GetSourceFileLineNumber(XmlNode xmlNode)
        {
            //There is a bug in OpenCover, it does not have the FileRef node for constructors or properties
            var fileRef = xmlNode.SelectSingleNode("FileRef");
            if (fileRef == null)
                return -1;
            var nodes = GetSequencePointNodes(xmlNode);
            if (nodes.Any() == false)
                return -1;
            var sl = nodes.Select(_ => int.Parse(_.Attributes["sl"].Value)).Min();
            return sl;
        }

        private string GetSourceFile(XmlNode xmlNode)
        {
            //There is a bug in OpenCover, it does not have the FileRef node for constructors or properties
            var fileRef = xmlNode.SelectSingleNode("FileRef");
            if (fileRef == null)
                return "";
            var fileUid = fileRef.Attributes["uid"].Value;
            var parentNode = xmlNode.ParentNode.ParentNode.ParentNode.ParentNode;
            var fileNode = parentNode
                .SelectSingleNode(string.Format(@"Files/File[@uid = '{0}']", fileUid));

            return fileNode.Attributes["fullPath"].Value;
        }

        private int GetCyclomaticComplexity(XmlNode xmlNode)
        {
            var attr = xmlNode.Attributes["cyclomaticComplexity"];
            return int.Parse(attr.Value);
        }

        public IEnumerable<CodeCoverage> ParseFile(string fileName)
        {
            var coverage = new List<CodeCoverage>();
            var xml = _xmlFileLoader.LoadXmlDocument(fileName);
            var methods = xml.SelectNodes(@"/CoverageSession/Modules/Module/Classes/Class/Methods/Method");
            for (int i = 0; i < methods.Count; i++)
            {
                var fullName = GetFullName(methods[i]);
                if (IsValidMethod(methods[i]))
                {
                    var cc = new CodeCoverage
                                 {
                                     Method = GetMethodName(fullName),
                                     Class = GetClassName(fullName),
                                     NameSpace = GetNamespace(fullName),
                                     CoveragePercent = GetCoveragePercent(methods[i]),
                                     MethodSignature = GetMethodSignature(fullName)
                                 };
                    coverage.Add(cc);
                }
            }
            return coverage;
        }

        private Percent GetCoveragePercent(XmlNode xmlNode)
        {
            // Is there a way to detct brace-only lines and remove them?

            var nodes = GetSequencePointNodes(xmlNode);
            RemoveStartAndEndBrace(nodes);
            if (nodes.Any())
            {
                var visited = nodes.Count(_ => int.Parse(_.Attributes["vc"].Value) > 0);
                return (visited / (1.0d * nodes.Count())).ToPercent();
            }
            return int.Parse(xmlNode.Attributes["sequenceCoverage"].Value).Percent();
        }

        private static List<XmlNode> GetSequencePointNodes(XmlNode xmlNode)
        {
            var nodes = new List<XmlNode>();
            var seqPoints = xmlNode.SelectNodes("SequencePoints/SequencePoint");
            for (int i = 0; i < seqPoints.Count; i++)
                nodes.Add(seqPoints[i]);
            return nodes;
        }

        protected void RemoveStartAndEndBrace(List<XmlNode> ptNodes)
        {
            if (ptNodes.Count() > 0)
                ptNodes.Remove(ptNodes.First());
            if (ptNodes.Count() > 0)
                ptNodes.Remove(ptNodes.Last());
        }

        private string GetMethodSignature(string fullName)
        {
            var idxSpace = fullName.IndexOf(' ');
            var returns = fullName.Substring(0, idxSpace);
            int idxParantes = fullName.IndexOf('(');
            var input = fullName.Substring(idxParantes);
            var methodName = GetMethodName(fullName);
            return returns + " " + methodName + input;
        }

        private string GetNamespace(string fullName)
        {
            var idx = fullName.IndexOf(" ") + 1;
            var end = fullName.IndexOf("::", idx);
            var name = fullName.Substring(idx, end - idx);
            var className = GetClassName(fullName);
            return name.Substring(0, name.Length - className.Length - 1);
        }

        private string GetClassName(string fullName)
        {
            var idx = fullName.IndexOf(" ");
            var end = fullName.IndexOf("::", idx);
            var className = fullName.Substring(idx, end - idx)
                .Split(new[] { '.' })
                .Last();
            return className;
        }

        private string GetMethodName(string fullName)
        {
            var idx = fullName.IndexOf("::") + 2;
            var end = fullName.IndexOf("(", idx);
            var methodName = fullName.Substring(idx, end - idx).Split(new[] { '.' }).Last();
            return methodName;
        }

        private bool IsValidMethod(XmlNode node)
        {
            bool isCtor = GetFullName(node).Contains("::.ctor");
            bool hasFileNode = node.SelectNodes("FileRef").Count > 0;
            return !isCtor || hasFileNode;
        }
    }
}