Story: Crap4n
	As a developer
	I want to measure the CRAP metric
	So that I know which methods that need attention
		
Scenario: PartCover and SourceMonitor 2.5
	Given a code metrics file SourceMonitorResult.xml from SourceMonitor 2.5
		And a code coverage file PartCoverResult.xml from PartCover
	When the CRAP metric is calculated and stored in crapResult.xml
	Then I should get a crapResult.xml result file 
		And the method CompleteCoverage should have a CRAP value of 4
		And the method SemiCoverage should have a CRAP value of 7
		And the method NoCoverage should have a CRAP value of 20

Scenario: NCover 1.5.8 and SourceMonitor 2.5
	Given a code metrics file SourceMonitorResult.xml from SourceMonitor 2.5
		And a code coverage file NCover15Result.xml from NCover 1.5.8
	When the CRAP metric is calculated and stored in crapResult.xml
	Then I should get a crapResult.xml result file 
		And the method CompleteCoverage should have a CRAP value of 4
		And the method SemiCoverage should have a CRAP value of 7
		And the method NoCoverage should have a CRAP value of 20

Scenario: NCover 3.2 and SourceMonitor 2.5
	Given a code metrics file SourceMonitorResult.xml from SourceMonitor 2.5
		And a code coverage file NCover32Result.xml from NCover 3.2.4
	When the CRAP metric is calculated and stored in crapResult.xml
	Then I should get a crapResult.xml result file 
		And the method CompleteCoverage should have a CRAP value of 4
		And the method SemiCoverage should have a CRAP value of 7
		And the method NoCoverage should have a CRAP value of 20

Scenario: NCover 3.2 only
	Given a code metrics file NCover32Result.xml from NCover 3.2.4
		And a code coverage file NCover32Result.xml from NCover 3.2.4
	When the CRAP metric is calculated and stored in crapResult.xml
	Then I should get a crapResult.xml result file 
		And the method CompleteCoverage should have a CRAP value of 4
		And the method SemiCoverage should have a CRAP value of 7
		And the method NoCoverage should have a CRAP value of 20	
