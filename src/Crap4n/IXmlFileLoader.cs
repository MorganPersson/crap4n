using System.Xml;

namespace Crap4n
{
    public interface IXmlFileLoader
    {
        XmlDocument LoadXmlDocument(string fileName);
    }
}
