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
			string[] classAndName = methodMetrics.Attributes["name"].Value.Split('.');
			var codeMetrics = new CodeMetrics { Class = classAndName[0], SourceFile = GetSourceFile(methodMetrics), SourceFileLineNumber = GetSourceFileLineNumber(methodMetrics) };
			if (classAndName.Length == 3)
			{
				if (IsProperty(classAndName))
					codeMetrics.Method = GetPropertyName(classAndName); //Explicit interface with property is bugged in SourceMonitor
				else
					codeMetrics.Method = classAndName[2];
			}
			else
			{
				codeMetrics.Method = classAndName[1];
			}

			codeMetrics.Method = codeMetrics.Method.Replace("()", "");
			if (codeMetrics.Method == codeMetrics.Class)
				codeMetrics.Method = ".ctor";
			codeMetrics.Method = RemoveGenericStuff(codeMetrics.Method);
			codeMetrics.CyclomaticComplexity = int.Parse(methodMetrics.SelectSingleNode("complexity").InnerText);
			return codeMetrics;
		}
		
		private string RemoveGenericStuff(string methodName)
		{
			int pos = methodName.IndexOf('<');
			if (pos == -1)
				return methodName;
			return methodName.Substring(0, pos);
		}

		private int GetSourceFileLineNumber(XmlNode methodMetrics)
		{
			return int.Parse(methodMetrics.Attributes["line"].Value);
		}

		private string GetSourceFile(XmlNode methodMetrics)
		{
			return methodMetrics.ParentNode.ParentNode.Attributes["file_name"].Value;
		}

		private string GetPropertyName(string[] classAndName)
		{
			return classAndName.Last().Replace("()", "") + "_" + classAndName[1];
		}

		private bool IsProperty(string[] classAndName)
		{
			return classAndName.Last().Equals("get()") || classAndName.Last().Equals("set()");
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