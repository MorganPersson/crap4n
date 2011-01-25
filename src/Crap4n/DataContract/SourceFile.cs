using System;
using System.Xml.Serialization;

namespace Crap4n.DataContract
{
    [Serializable]
    public class SourceFile
    {
        [XmlText]
        public string FileName { get; set; }

        [XmlAttribute(AttributeName = "lineNumber")]
        public int LineNumber { get; set; }
    }
}