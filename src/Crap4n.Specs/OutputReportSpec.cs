using System.Collections.Generic;
using System.IO;
using System.Xml;
using Crap4n.Specs.Helper;
using NBehave.Spec.NUnit;
using Context = NUnit.Framework.TestFixtureAttribute;

namespace Crap4n.Specs
{
    [Context]
    public class OutputReportSpec : SpecBase<OutputReport>
    {
        protected override OutputReport Establish_context()
        {
            var crapResult = new List<Crap> {CrapBuilder.CrappyMethod(), CrapBuilder.GoodMethod()};
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

        [Specification]
        public void Should_return_0_Percent_if_0_methods()
        {
            var crapResult = new List<Crap>();
            var stringWriter = new StringWriter();
            var outPut = new PlainTextOutput(stringWriter);
            var outputReport = new OutputReport(crapResult, outPut); 
            const int crapThreshold = 20;
            outputReport.OutputCrapSummary(crapThreshold);
            stringWriter.ToString().ShouldContain("0% methods are CRAP. The CRAPload for this project is 0");
        }
    }
}