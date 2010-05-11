using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Crap4n
{
	public class CrapRunner
	{
		private List<Crap> _crapResult =new List<Crap>() ;
		private int _crapThreshold;
		public IEnumerable<Crap> CrapResult { get { return _crapResult; } }
		private ICrapService _crapService;
		
		public CrapRunner(ICrapService crapService)
		{
			_crapService = crapService;
		}
		
		public void Run(string codeCoverageFile, string codeMetricsFile, int crapThreshold)
		{
			ValidateInput(codeCoverageFile, codeMetricsFile, crapThreshold);

			_crapThreshold = crapThreshold;
			_crapResult.Clear();
			_crapResult.AddRange(_crapService.BuildResult(codeCoverageFile, codeMetricsFile, _crapThreshold));
		}

		public void WriteOutput(ITextOutput output)
		{
			var report = new OutputReport(_crapResult, output);
			report.OutputMethodsAboveCrapThreshold(_crapThreshold);
			report.OutputCrapSummary(_crapThreshold);
		}

		public void WriteXmlOutput(string xmlOutputFile, ITextOutput output)
		{
			var report = new OutputReport(_crapResult, output);
			string xml = report.GetReportAsXml(_crapThreshold);
			using (var stream = new StreamWriter(xmlOutputFile, false, Encoding.Unicode))
			{
				stream.Write(xml);
				stream.Flush();
			}
		}

		private void ValidateInput(string codeCoverageFile, string codeMetricsFile, int crapThreshold)
		{
			ValidateFileParam(codeCoverageFile, "codeCoverageFile");
			ValidateFileParam(codeMetricsFile, "codeMetricsFile");
			if (crapThreshold < 0)
				throw new ArgumentOutOfRangeException("crapThreshold", "The crap threshold must be larger than 0");
		}
		
		private void ValidateFileParam(string fileName, string paramName)
		{
			if (fileName == null)
				throw new ArgumentNullException(paramName);
			if (File.Exists(fileName) == false)
				throw new FileNotFoundException(string.Format("Can't find {0} file {1}", paramName, fileName));
		}
	}
}
