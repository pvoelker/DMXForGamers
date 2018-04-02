;NSIS Modern User Interface
;DMX for Gamers Script
;Written by Paul Voelker

;--------------------------------
;Include Modern UI

  !include "MUI2.nsh"
  
  !define MUI_ICON ".\DMXForGamers\DMXForGamers.ico"
  !define MUI_UNICON ".\DMXForGamersUninstall.ico"

;--------------------------------
;General

  ;Name and file
  Name "DMX for Gamers"
  OutFile "DMXForGamersInstall.exe"

  ;Default installation folder
  InstallDir "$PROGRAMFILES\DMX for Gamers"
  
  ;Get installation folder from registry if available
  InstallDirRegKey HKCU "Software\DMX for Gamers" ""

  ;Request application privileges for Windows Vista
  RequestExecutionLevel admin

;--------------------------------
;Interface Settings

  !define MUI_ABORTWARNING

;--------------------------------
;Pages

  ;!insertmacro MUI_PAGE_LICENSE "${NSISDIR}\Docs\Modern UI\License.txt"
  ;!insertmacro MUI_PAGE_COMPONENTS
  !insertmacro MUI_PAGE_DIRECTORY
  !insertmacro MUI_PAGE_INSTFILES
  
  !insertmacro MUI_UNPAGE_CONFIRM
  !insertmacro MUI_UNPAGE_INSTFILES
  
;--------------------------------
;Languages
 
  !insertmacro MUI_LANGUAGE "English"

;--------------------------------
;Installer Sections

Section

  SetOutPath "$INSTDIR"
  
  FILE .\DMXForGamers\bin\Release\*.exe
  FILE .\DMXForGamers\bin\Release\*.dll
  FILE .\DMXForGamers\bin\Release\DMXForGamers.exe.config
  
  ;Store installation folder
  WriteRegStr HKCU "Software\DMX for Gamers" "" $INSTDIR
  
  ;'Programs and Features' entry (http://nsis.sourceforge.net/Add_uninstall_information_to_Add/Remove_Programs)
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\DMX for Gamers" \
                   "DisplayName" "DMX for Gamers"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\DMX for Gamers" \
                   "Publisher" "Paul Voelker"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\DMX for Gamers" \
                   "UninstallString" "$\"$INSTDIR\uninstall.exe$\""
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\DMX for Gamers" \
                   "QuietUninstallString" "$\"$INSTDIR\uninstall.exe$\" /S"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\DMX for Gamers" \
                   "DisplayIcon" "$\"$INSTDIR\DMXForGamers.exe$\""

  ;Create uninstaller
  WriteUninstaller "$INSTDIR\Uninstall.exe"

SectionEnd

;--------------------------------
;Descriptions

  ;Language strings
  ;LangString DESC_SecDummy ${LANG_ENGLISH} "A test section."

  ;Assign language strings to sections
  ;!insertmacro MUI_FUNCTION_DESCRIPTION_BEGIN
  ;!insertmacro MUI_DESCRIPTION_TEXT ${SecDummy} $(DESC_SecDummy)
  ;!insertmacro MUI_FUNCTION_DESCRIPTION_END

;--------------------------------
;Uninstaller Section

Section "Uninstall"

  Delete "$INSTDIR\Uninstall.exe"

  Delete "$INSTDIR\*.exe"
  Delete "$INSTDIR\*.dll"
  Delete "$INSTDIR\DMXForGamers.exe.config"

  RMDir "$INSTDIR"

  DeleteRegKey /ifempty HKCU "Software\DMX for Gamers"

  DeleteRegKey HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\DMX for Gamers"

SectionEnd