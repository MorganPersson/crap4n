Feature: Crap4n
	As a developer
	I want to measure the CRAP metric
	So that I know which methods that need attention

Scenario: Supported Tools
	Given a code metrics file [metricsFile] from [metricsTool]
		And a code coverage file [codeCoverageFile] from [codeCoverageTool]
	When the CRAP metric is calculated and stored in [outputFileName]
	Then I should get a [outputFileName] result file 
		And the method CompleteCoverage should have a CRAP value of 4
		And the method SemiCoverage should have a CRAP value of 7
		And the method NoCoverage should have a CRAP value of 20
		
Examples:
|metricsFile            |metricsTool      |codeCoverageFile   |codeCoverageTool|outputFileName |
|SourceMonitorResult.xml|SourceMonitor 2.5|PartCoverResult.xml|PartCover       |crapResult1.xml|
|SourceMonitorResult.xml|SourceMonitor 2.5|NCover15Result.xml |NCover 1.5.8    |crapResult2.xml|
|SourceMonitorResult.xml|SourceMonitor 2.5|NCover32Result.xml |NCover 3.2.4    |crapResult3.xml|
|NCover32Result.xml     |NCover 3.2.4     |NCover32Result.xml |NCover 3.2.4    |crapResult4.xml|
|ReflectorResult.xml    |Reflector        |PartCoverResult.xml|PartCover       |crapResult5.xml|