using System.Xml;

namespace Crap4n
{
    public class XmlFileLoader : IXmlFileLoader
    {
        #region IXmlFileLoader Members

        public XmlDocument LoadXmlDocument(string fileName)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.Load(fileName);
            return xmlDoc;
        }

        #endregion
    }
}