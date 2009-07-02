using System;
using System.Xml.Serialization;

namespace Crap4n.Console.DataContract
{
    [Serializable]
    public class Summary
    {
        [XmlAttribute(AttributeName = "totalMethods")]
        public int TotalMethods { get; set; }
        [XmlAttribute(AttributeName = "crappyMethods")]
        public int CrappyMethods { get; set; }
        [XmlAttribute(AttributeName = "crapLoad")]
        public double CrapLoad { get; set; }
        [XmlAttribute(AttributeName = "crapThreshold")]
        public int CrapThreshold { get; set; }
    }
}