using System;
using System.Xml.Serialization;

namespace Crap4n.DataContract
{
    [Serializable]
    public class CrapMethod
    {
        [XmlAttribute(AttributeName = "name")]
        public string MethodName { get; set; }

        [XmlAttribute(AttributeName = "signature")]
        public string MethodSignature { get; set; }

        [XmlAttribute(AttributeName = "class")]
        public string Class { get; set; }

        [XmlAttribute(AttributeName = "namespace")]
        public string Namespace { get; set; }

        [XmlElement(ElementName = "crap", Order = 1)]
        public double CrapValue { get; set; }

        [XmlElement(ElementName = "crapLoad", Order = 2)]
        public double CrapLoad { get; set; }

        [XmlElement(ElementName = "codeCoverage", Order = 4)]
        public double CodeCoverage { get; set; }

        [XmlElement(ElementName = "cyclomaticComplexity", Order = 3)]
        public int CyclomaticComplexity { get; set; }

        [XmlElement(ElementName = "sourceFile", Order = 5)]
        public SourceFile SourceFile { get; set; }
    }
}