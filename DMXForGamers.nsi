;NSIS Modern User Interface
;DMX for Gamers Script
;Written by Paul Voelker

;--------------------------------
;Include Modern UI

  !include "MUI2.nsh"
  
  !define MUI_ICON ".\DMXForGamers\DMXForGamers.ico"
  !define MUI_UNICON ".\DMXForGamersUninstall.ico"

;--------------------------------

  !define APP_NAME "DMX for Gamers"

;--------------------------------
;General

  !getdllversion ".\DMXForGamers\bin\Release\net6.0-windows\DMXForGamers.exe" MyVer_

  ;Name and file
  Name "DMX for Gamers v${MyVer_1}.${MyVer_2}"
  OutFile "DMXForGamersInstall.exe"

  VIProductVersion "${MyVer_1}.${MyVer_2}.${MyVer_3}.${MyVer_4}"
  VIAddVersionKey ProductName "DMX for Gamers Install"
  VIAddVersionKey FileVersion "${MyVer_1}.${MyVer_2}.${MyVer_3}.${MyVer_4}"
  VIAddVersionKey ProductVersion "${MyVer_1}.${MyVer_2}.${MyVer_3}.${MyVer_4}"
  VIAddVersionKey FileDescription "Installation Package for DMX for Gamers"
  VIAddVersionKey LegalCopyright "Copyright © 2022 Paul Voelker"

  ;Default installation folder
  InstallDir "$PROGRAMFILES\${APP_NAME}"
  
  ;Get installation folder from registry if available
  InstallDirRegKey HKCU "Software\${APP_NAME}" ""

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
  
  File .\DMXForGamers\bin\Release\net6.0-windows\*.exe
  File .\DMXForGamers\bin\Release\net6.0-windows\*.dll
  File .\DMXForGamers\bin\Release\net6.0-windows\*.runtimeconfig.json
  File .\DMXForGamers\bin\Release\net6.0-windows\dmxforgamershelp.chm
  
  File /nonfatal /a /r .\DMXForGamers\bin\Release\net6.0-windows\wwwroot\

  SetOutPath "$INSTDIR\Examples"

  File .\ConfigFiles\*.dgd
  File .\ConfigFiles\*.dge

  ;Store installation folder
  WriteRegStr HKCU "Software\DMX for Gamers" "" $INSTDIR
  
  ;'Programs and Features' entry (http://nsis.sourceforge.net/Add_uninstall_information_to_Add/Remove_Programs)
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APP_NAME}" \
                   "DisplayName" "DMX for Gamers"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APP_NAME}" \
                   "Publisher" "Paul Voelker"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APP_NAME}" \
                   "UninstallString" "$\"$INSTDIR\uninstall.exe$\""
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APP_NAME}" \
                   "QuietUninstallString" "$\"$INSTDIR\uninstall.exe$\" /S"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APP_NAME}" \
                   "DisplayIcon" "$\"$INSTDIR\DMXForGamers.exe$\""

  ; Start Menu
  CreateDirectory "$SMPROGRAMS\${APP_NAME}"
  CreateShortCut "$SMPROGRAMS\${APP_NAME}\${APP_NAME}.lnk" "$INSTDIR\DMXForGamers.exe"

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

  ; Start Menu
  Delete "$SMPROGRAMS\${APP_NAME}\${APP_NAME}.lnk"
  RmDir "$SMPROGRAMS\${APP_NAME}"

  Delete "$INSTDIR\Examples\*.dgd"
  Delete "$INSTDIR\Examples\*.dge"

  Delete "$INSTDIR\Uninstall.exe"

  Delete "$INSTDIR\*.exe"
  Delete "$INSTDIR\*.dll"
  Delete "$INSTDIR\*.runtimeconfig.json"
  Delete "$INSTDIR\dmxforgamershelp.chm"

  RmDir /r "$INSTDIR\wwwroot"
  RmDir "$INSTDIR\Examples"
  RmDir "$INSTDIR"

  DeleteRegKey /ifempty HKCU "Software\${APP_NAME}"

  DeleteRegKey HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APP_NAME}"

SectionEnd

;--------------------------------

Function .onInit
 
  ReadRegStr $R0 HKLM \
  "Software\Microsoft\Windows\CurrentVersion\Uninstall\DMX for Gamers" \
  "QuietUninstallString"
  StrCmp $R0 "" done
 
  MessageBox MB_OKCANCEL|MB_ICONEXCLAMATION \
  "'${APP_NAME}' is already installed. $\n$\nClick `OK` to remove the \
  previous version or `Cancel` to cancel this upgrade." \
  IDOK uninst
  Abort
 
;Run the uninstaller
  uninst:
    ClearErrors
    Exec $R0
  done:
 
FunctionEnd

; Sign installer and uninstaller

;!finalize 'sign.bat "%1" "MyProduct Installer" http://example.com'
!finalize 'signinstaller.bat'

;!uninstfinalize 'sign.bat "%1" "MyProduct Installer" http://example.com'
!uninstfinalize 'signuninstaller.bat'