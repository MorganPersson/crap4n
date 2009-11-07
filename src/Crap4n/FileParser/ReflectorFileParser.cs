using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Crap4n.FileParser
{
    public class ReflectorFileParser : IFileParser<CodeMetrics>
    {
        private readonly IXmlFileLoader _xmlFileLoader;

        public ReflectorFileParser(IXmlFileLoader xmlFileLoader)
        {
            _xmlFileLoader = xmlFileLoader;
        }

        #region IFileParser<CodeMetrics> Members

        public bool CanParseFile(string fileName)
        {
            var xml = _xmlFileLoader.LoadXmlDocument(fileName);
            var node = xml.SelectSingleNode("/Report");
            return node != null;
        }

        public IEnumerable<CodeMetrics> ParseFile(string fileName)
        {
            var data = new List<CodeMetrics>();
            IEnumerable<XElement> results = LoadMethodsDataFromFile(fileName);
            foreach (var method in results)
            {
                var newEntry = new CodeMetrics
                                   {
                                       NameSpace = GetNameSpace(method),
                                       Class = GetTypeNameWithoutNameSpace(method),
                                       Method = GetMethodName(method),
                                       CyclomaticComplexity = GetCyclomaticComplexityValue(method)
                                   };
                data.Add(newEntry);
            }
            return data;
        }

        private string GetNameSpace(XElement method)
        {
            var fullMethodName = (string) method.Attribute("Name");
            string[] parts = fullMethodName.Split(new[] {'.'});
            return parts[0];
        }

        private string GetTypeNameWithoutNameSpace(XElement method)
        {
            var fullMethodName = (string) method.Attribute("Name");
            if (fullMethodName.StartsWith("<>"))
            {
                return GetAnontmousTypeName(fullMethodName);
            }
            string[] parts = fullMethodName.Split(new[] {'.'});
            return parts[1];
        }

        private string GetAnontmousTypeName(string fullMethodName)
        {
            string[] parts = fullMethodName.Split(new[] {'.'});
            return parts[0];
        }

        private string GetMethodName(XElement method)
        {
            var fullMethodName = (string) method.Attribute("Name");
            if (fullMethodName.StartsWith("<>"))
            {
                fullMethodName = "Anonymous." + fullMethodName;
            }
            string[] parts = fullMethodName.Split(new[] {'.'});
            if (IsCtor(parts))
                return ".ctor";
            if (IsCctor(parts))
                return ".cctor";
            string methodName = GetMethodPart(parts);
            return CleanMethodName(methodName);
        }

        private bool IsCtor(string[] parts)
        {
            if (parts.Length == 4 && parts[3].StartsWith("ctor"))
            {
                return true;
            }
            return false;
        }

        private bool IsCctor(string[] parts)
        {
            if (parts.Length == 4 && parts[3].StartsWith("cctor"))
            {
                return true;
            }
            return false;
        }

        private string CleanMethodName(string methodName)
        {
            methodName = CleanTemplate(methodName);
            return CleanArguments(methodName);
        }

        private string CleanTemplate(string methodName)
        {
            return methodName.Split(new[] {'<'})[0];
        }

        private string CleanArguments(string methodName)
        {
            return methodName.Split(new[] {'('})[0];
        }

        private static string GetMethodPart(string[] parts)
        {
            return parts[2];
        }

        private int GetCyclomaticComplexityValue(XElement method)
        {
            return (int) method.Attribute("CyclomaticComplexity");
        }


        private IEnumerable<XElement> LoadMethodsDataFromFile(string reportFileName)
        {
            XDocument loaded = XDocument.Load(reportFileName);
            var result = (from metric in loaded.Descendants()
                          where metric.Name.LocalName == "Method"
                          select metric);
            return result;
        }

        #endregion
    }
}