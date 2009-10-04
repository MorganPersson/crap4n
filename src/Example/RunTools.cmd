@REM ----------------
@ECHO OFF
CLS
@ECHO ---- SourceMonitor
..\..\tools\SourceMonitor\SourceMonitor.exe /C sourcemonitor.config

@ECHO ---- PartCover
regsvr32 -s ..\..\tools\PartCover\PartCover.CorDriver.dll
..\..\tools\PartCover\PartCover.exe --settings partcover.config
regsvr32 -s -u ..\..\tools\PartCover\PartCover.CorDriver.dll

@ECHO ---- NCover 1.5
c:\Program\NCover\NCover.Console.exe ..\..\tools\nunit\nunit-console.exe /noshadow "bin\debug\Example.dll" //x ExampleNCover3Result.xml //eas "nunit;^NBehave\..+" //em "^Example\.Specs\..+"

@ECHO ---- NCover 3.2 (3.x ??)
c:\Program\NCover\NCover.Console.exe ..\..\tools\nunit\nunit-console.exe /noshadow "bin\debug\Example.dll" //x ExampleNCover3Result.xml //eas "nunit;^NBehave\..+" //em "^Example\.Specs\..+"
