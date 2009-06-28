using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace Crap4n.FileParser
{
    public class SourceMonitorFileParser : IFileParser<CodeMetrics>
    {
        private readonly IXmlFileLoader _xmlFileLoader;

        public SourceMonitorFileParser(IXmlFileLoader xmlFileLoader)
        {
            _xmlFileLoader = xmlFileLoader;
        }

        public bool CanParseFile(string fileName)
        {
            var xml = _xmlFileLoader.LoadXmlDocument(fileName);
            var node = xml.SelectSingleNode("/sourcemonitor_metrics");
            return node != null;
        }

        public IEnumerable<CodeMetrics> ParseFile(string fileName)
        {
            var xml = _xmlFileLoader.LoadXmlDocument(fileName);
            XmlNode checkPoint = GetLatestCheckPoint(xml);
            XmlNodeList methodMetrics = GetMethodMetricNodes(checkPoint);
            return ParseMethodMetrics(methodMetrics);
        }

        private IEnumerable<CodeMetrics> ParseMethodMetrics(XmlNodeList methodMetrics)
        {
            var codeMetrics = new List<CodeMetrics>();
            for (int i = 0; i < methodMetrics.Count; i++)
            {
                CodeMetrics codeMetricsForMethod = CreateCodeMetricsForMethodFromXml(methodMetrics[i]);
                codeMetrics.Add(codeMetricsForMethod);
            }
            return codeMetrics;
        }

        private CodeMetrics CreateCodeMetricsForMethodFromXml(XmlNode methodMetrics)
        {
            var classAndName = methodMetrics.Attributes["name"].Value.Split('.');
            return new CodeMetrics
                       {
                           Class = classAndName[0],
                           Method = classAndName[1].Replace("()", ""),
                           CyclomaticComplexity = int.Parse(methodMetrics.SelectSingleNode("complexity").InnerText)
                       };
        }

        private XmlNodeList GetMethodMetricNodes(XmlNode checkPoint)
        {
            return checkPoint.SelectNodes("files/file/method_metrics/method");
        }

        private XmlNode GetLatestCheckPoint(XmlNode xml)
        {
            XmlNodeList nodes = xml.SelectNodes("sourcemonitor_metrics/project/checkpoints/checkpoint");
            var nodeArray = new List<XmlNode>();
            for (int i = 0; i < nodes.Count; i++)
                nodeArray.Add(nodes[i]);
            var latest = from n in nodeArray
                         orderby n.Attributes["checkpoint_date"]
                         select n;
            return latest.First();
        }
    }
}