@ECHO OFF
tools\nant\nant
CLS
tools\NBehave\NBehave-Console.exe build\Debug\UnitTests\Crap4n.Specs.dll /sf=build\Debug\UnitTests\*.story"
