using System.IO;
using System.Xml;
using Crap4n.Console;
using NBehave.Narrator.Framework;
using NBehave.Spec.NUnit;

namespace Crap4n.Specs.Steps
{
    [ActionSteps]
    public class Crap4nSteps
    {
        private string _codeCoverageFile = string.Empty;
        private string _codeMetricsFile = string.Empty;
        private XmlDocument _crapResult;
        private string _crapResultOutputFileName;
        private string _path = string.Empty;
        private XmlNamespaceManager _xmlNameSpaceManager;

        [BeforeScenario]
        public void Before_each_scenario()
        {
            _path = Path.GetDirectoryName(GetType().Assembly.Location);
            System.Console.WriteLine(_path);
        }

        [AfterScenario]
        public void After_each_scenario()
        {
            _crapResult = null;
            if (string.IsNullOrEmpty(_crapResultOutputFileName) == false)
                if (File.Exists(_crapResultOutputFileName))
                    File.Delete(_crapResultOutputFileName);
            _codeMetricsFile = string.Empty;
            _codeCoverageFile = string.Empty;
        }

        [Given(@"a code metrics file $fileName from Reflector")]
        [Given(@"a code metrics file $fileName from SourceMonitor 2.5")]
        [Given(@"a code metrics file $fileName from NCover 3.2.4")]
        public void Given_a_code_metrics_file(string fileName)
        {
            _codeMetricsFile = Path.Combine(_path, fileName);
        }

        [Given(@"a code coverage file $fileName from PartCover")]
        [Given(@"a code coverage file $fileName from NCover 1.5.8")]
        [Given(@"a code coverage file $fileName from NCover 3.2.4")]
        public void Given_a_code_coverage_file(string fileName)
        {
            _codeCoverageFile = Path.Combine(_path, fileName);
        }

        [When(@"the CRAP metric is calculated and stored in $outputFileName")]
        public void When_the_CRAP_metric_result_is_stored_in_file(string outputFileName)
        {
            _crapResultOutputFileName = outputFileName;
            var args = new[]
                           {
                               string.Format(@"/codeCoverage={0}", _codeCoverageFile),
                               string.Format(@"/codeMetrics={0}", _codeMetricsFile),
                               string.Format(@"/xml={0}", outputFileName)
                           };
            Program.Main(args);
        }

        [Then(@"I should get a $fileName result file")]
        public void Then_I_should_get_a_result_file(string fileName)
        {
            File.Exists(fileName).ShouldBeTrue();
            _crapResult = new XmlDocument();
            _crapResult.Load(fileName);
            _xmlNameSpaceManager = new XmlNamespaceManager(_crapResult.NameTable);
            _xmlNameSpaceManager.AddNamespace("c", "Crap4n");
        }

        [Then(@"the method (?<methodName>\w+) should have a CRAP value of (?<crapValue>\d+\,?\.?\d*)")]
        public void Then_methof_should_have_crap_value(string methodName, string crapValue)
        {
            string xpath = string.Format("c:crapResult/c:methods/c:method[@name='{0}']/c:crap", methodName);
            var calculatedCrapValue = _crapResult.SelectSingleNode(xpath, _xmlNameSpaceManager);
            calculatedCrapValue.ShouldNotBeNull();
            calculatedCrapValue.InnerText.ShouldEqual(crapValue);
        }
    }
}