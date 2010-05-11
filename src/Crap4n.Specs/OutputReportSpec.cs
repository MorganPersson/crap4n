using System.Collections.Generic;
using System.IO;
using System.Xml;
using Crap4n.Console;
using NBehave.Spec.NUnit;
using Context = NUnit.Framework.TestFixtureAttribute;

namespace Crap4n.Specs
{
    [Context]
    public class OutputReportSpec : SpecBase<OutputReport>
    {
        protected override OutputReport Establish_context()
        {
            var crapResult = new List<Crap> { Helper.CrapBuilder.CrappyMethod(), Helper.CrapBuilder.GoodMethod() };
            var outPut = new PlainTextOutput(new StringWriter());
            return new OutputReport(crapResult, outPut);
        }

        [Specification]
        public void Should_serialize_result_as_xml()
        {
            const int crapThreshold = 20;
            string xml = Sut.GetReportAsXml(crapThreshold);
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);
            xmlDoc.ChildNodes.Count.ShouldNotBeNull();
        }
    }
}