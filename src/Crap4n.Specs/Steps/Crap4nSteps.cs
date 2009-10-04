using System;
using System.IO;
using System.Xml;
using NBehave.Spec.NUnit;
using NBehave.Narrator.Framework;

namespace Crap4n.Specs.Scenarios
{
	[ActionSteps]
	public class Crap4nSteps
	{
		private string _path = string.Empty;
		private string _codeMetricsFile = string.Empty;
		private string _codeCoverageFile = string.Empty;
		private string _crapResultOutputFileName;
		private XmlNamespaceManager _xmlNameSpaceManager;
		private XmlDocument _crapResult;

		//Does not work correctly
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
			if (string.IsNullOrEmpty(_crapResultOutputFileName)==false)
				if (File.Exists(_crapResultOutputFileName))
					File.Delete(_crapResultOutputFileName);
			_codeMetricsFile = string.Empty;
			_codeCoverageFile = string.Empty;
		}
		
		[Given(@"a code metrics file (?<fileName>\w+.\w+) from SourceMonitor 2.5")]
		[Given(@"a code metrics file (?<fileName>\w+.\w+) from NCover 3.2.4")]
		public void Given_a_code_metrics_file(string fileName)
		{
			_codeMetricsFile = Path.Combine(_path, fileName);
		}
		
		[Given(@"a code coverage file (?<fileName>\w+.\w+) from PartCover")]
		[Given(@"a code coverage file (?<fileName>\w+.\w+) from NCover 1.5.8")]
		[Given(@"a code coverage file (?<fileName>\w+.\w+) from NCover 3.2.4")]
		public void Given_a_code_coverage_file(string fileName)
		{
			_codeCoverageFile = Path.Combine(_path, fileName);
		}
		
		[When(@"the CRAP metric is calculated and stored in (?<fileName>\w+.\w+)")]
		public void When_the_CRAP_metric_result_is_stored_in_file(string fileName)
		{
			_crapResultOutputFileName = fileName;
			var args = new string[]
			{
				string.Format(@"/codeCoverage={0}", _codeCoverageFile) ,
				string.Format(@"/codeMetrics={0}", _codeMetricsFile) ,
				string.Format(@"/xml={0}", fileName)
			};
			Crap4n.Console.Program.Main(args);
		}
		
		[Then(@"I should get a (?<fileName>\w+.\w+) result file")]
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
