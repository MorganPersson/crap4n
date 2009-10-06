using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace Crap4n.Console.DataContract
{
    // <?xml-stylesheet type="text/xsl" href="crap4n-summary.xsl"?>
    [XmlRoot(ElementName = "crapResult")]
    public class CrapResult
    {
        public const string Namespace = "Crap4n";

        private CrapResult()
        {
        }

        [XmlElement(ElementName = "summary", Order = 1)]
        public Summary Summary { get; set; }

        //[XmlElement(ElementName = "methods", Order = 2)]
        [XmlArray(ElementName = "methods", Order = 2)]
        [XmlArrayItem(ElementName = "method")]
        public List<CrapMethod> Methods { get; set; }

        public static CrapResult Build(IEnumerable<Crap> crap, Func<int, IEnumerable<Crap>> aboveThreshold, int crapThreshold)
        {
            var crapResult = new CrapResult();
            GetSummary(crapResult, aboveThreshold, crapThreshold, crap);
            List<CrapMethod> methods = GetMethods(crap);
            crapResult.Methods = methods;
            return crapResult;
        }

        private static void GetSummary(CrapResult crapResult, Func<int, IEnumerable<Crap>> aboveThreshold, int crapThreshold, IEnumerable<Crap> crap)
        {
            var crappyMethods = aboveThreshold(crapThreshold);

            crapResult.Summary = new Summary
                                     {
                                         CrapLoad = Math.Round(crappyMethods.Sum(c => c.CrapLoad()), 1),
                                         TotalMethods = crap.Count(),
                                         CrappyMethods = crappyMethods.Count(),
                                         CrapThreshold = crapThreshold
                                     };
        }

        private static List<CrapMethod> GetMethods(IEnumerable<Crap> crap)
        {
            var methods = new List<CrapMethod>();
            foreach (var method in crap)
            {
                var crapMethod = new CrapMethod
                                     {
                                         Namespace = method.NameSpace,
                                         Class = method.Class,
                                         MethodName = method.Method,
                                         MethodSignature = method.MethodSignature,
                                         CodeCoverage = Math.Round(method.CodeCoverage, 1),
                                         CyclomaticComplexity = method.CyclomaticComplexity,
                                         CrapLoad = Math.Round(method.CrapLoad(), 1),
                                         CrapValue = Math.Round(method.Value, 1),
                                         SourceFile = new SourceFile { FileName = method.SourceFile, LineNumber = method.SourceFileLineNumber }
                                     };
                methods.Add(crapMethod);
            }
            return methods.OrderByDescending(c=>c.CrapValue).ThenBy(c=>c.CodeCoverage) .ToList();
        }
    }
}