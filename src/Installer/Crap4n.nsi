!ifndef VERSION
	!define VERSION "0.1"
!endif
!define FILES "..\..\Build\dist"
!define XSL_FILES "..\xsl"
!define EXAMPLEFILES "..\..\Build"
; The name of the installer
Name "Crap4n"


; The files to write
Outfile "..\..\Build\Crap4n_${VERSION}.exe"

; The default installation directory
InstallDir $PROGRAMFILES\Crap4n\${VERSION}

; Registry key to check for directory (so if you install again, it will 
; overwrite the old one automatically)
InstallDirRegKey HKLM "Software\Crap4n\${VERSION}" "Install_Dir"

; Request application privileges for Windows Vista
RequestExecutionLevel admin

;--------------------------------

; Pages

Page components
Page directory
Page instfiles

UninstPage uninstConfirm
UninstPage instfiles

;--------------------------------

; The stuff to install
Section "Framework Files (required)" ;No components page, name is not important
	
	SectionIn RO
	
	; Set output path to the installation directory.
	SetOutPath $INSTDIR
  
  ; Put file there
	File "${FILES}\Crap4n-Console.exe"	
	File "${FILES}\Crap4n.dll"
	File "${FILES}\Autofac.dll"
	File "${XSL_FILES}\*.xsl"

	; Write the installation path into the registry
	WriteRegStr HKLM SOFTWARE\Crap4n\${VERSION} "Install_Dir" "$INSTDIR"
  
	; Write the uninstall keys for Windows
	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Crap4n ${VERSION}" "DisplayName" "Crap4n ${VERSION}"
	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Crap4n ${VERSION}" "UninstallString" '"$INSTDIR\uninstall.exe"'
	WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Crap4n ${VERSION}" "NoModify" 1
	WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Crap4n ${VERSION}" "NoRepair" 1
	WriteUninstaller "uninstall.exe"

SectionEnd

;Section "MSbuild task"
;	File "${FILES}\Crap4n.MSBuild.dll"
;SectionEnd

;Section "NAnt task"
;	File "${FILES}\Crap4n.NAnt.dll"
;SectionEnd

; Uninstaller
Section "Uninstall"
  
  ; Remove registry keys
  DeleteRegKey HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Crap4n ${VERSION}"
  DeleteRegKey HKLM SOFTWARE\Crap4n\${VERSION}
  ; Remove files and uninstaller
  Delete $INSTDIR\*.dll
  Delete $INSTDIR\*.zip
  Delete $INSTDIR\Crap4n-Console.exe
  Delete $INSTDIR\uninstall.exe

  ; Remove directories used
  RMDir "$INSTDIR"

SectionEnd
