Unicode true
!include "MUI2.nsh"
!include "x64.nsh"
!include "FileFunc.nsh"

#
# Application information
#

!define APP_NAME "RDCLauncher"
!define APP_NAME_LC "rdclauncher"
!define APP_DISPLAY_NAME "RDC Launcher"
!define APP_VERSION_MAJOR 0
!define APP_VERSION_MINOR 5
!define APP_VERSION "${APP_VERSION_MAJOR}.${APP_VERSION_MINOR}.0"
!define APP_ICON "..\msrdcui\Resources\msrdcui0.ico"
!define APP_EXE_FILE_NAME "rdclauncher.exe"
!define UNINSTALLER_EXE_FILE_NAME "uninstall.exe"

#
# General
#

# Installer file name.
OutFile "setup-${APP_NAME_LC}-${APP_VERSION}.exe"

# Install path.
InstallDir "$APPDATA\${APP_NAME}"

# Requested execution privileges for the installation.
RequestExecutionLevel user

# Enable DPI-aware
ManifestDPIAware true

#
# Installer's version infomation
#

VIProductVersion "${APP_VERSION}.0"
VIFileVersion "${APP_VERSION}.0"
VIAddVersionKey /LANG=0 "ProductName" "${APP_DISPLAY_NAME}"
VIAddVersionKey /LANG=0 "ProductVersion" "${APP_VERSION}"
VIAddVersionKey /LANG=0 "FileVersion" "${APP_VERSION}"
VIAddVersionKey /LANG=0 "FileDescription" "The RDC Launcher allows to use the Remote Desktop client (msrdc) as like the Remote Desktop Connection (mstsc)."
VIAddVersionKey /LANG=0 "LegalCopyright" "Copyright (c) 2021-present Takeshi Katano. All rights reserved."

#
# Modern UI
#

!define MUI_ABORTWARNING

# Application name.
Name "${APP_DISPLAY_NAME}"

# Installer icon.
!define MUI_ICON "${APP_ICON}"

#
# Uninstall information
#

!define PUBLISHER "Takeshi Katano"
!define URL_INFO_ABOUT "https://github.com/tksh164/rdclauncher"
!define URL_UPDATE_INFO "https://github.com/tksh164/rdclauncher/releases/latest"
!define HELP_LINK "https://github.com/tksh164/rdclauncher"

#
# Installer pages
#

!define MUI_LICENSEPAGE_CHECKBOX
!insertmacro MUI_PAGE_LICENSE "..\..\LICENSE"

!insertmacro MUI_PAGE_INSTFILES

Page custom MsrdcDownloadPageCreator

!define MUI_FINISHPAGE_NOAUTOCLOSE
!insertmacro MUI_PAGE_FINISH

#
# Uninstaller pages
#

!insertmacro MUI_UNPAGE_CONFIRM

!insertmacro MUI_UNPAGE_INSTFILES

!define MUI_UNFINISHPAGE_NOAUTOCLOSE
!insertmacro MUI_UNPAGE_FINISH

#
# MUI Language
#

!insertmacro MUI_LANGUAGE "English"

#
# The Remote Desktop client download page
#

Var Dialog
Var MsrdcLink
Function "MsrdcDownloadPageCreator"

    # Verify the installation of the Remote Desktop client.
    ${If} ${RunningX64}
        SetRegView 64
    ${EndIf}
    ClearErrors
    ReadRegStr $0 HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\{017C228A-33BE-45BC-9651-DF83C2EE53F8}" "InstallLocation"
    ${IfNot} ${Errors}
        Abort  # Skip this page if the Remote Desktop client already installed.
    ${EndIf}

    !insertmacro MUI_HEADER_TEXT "Remote Desktop client for Windows Desktop" "Download the Remote Desktop client for Windows Desktop."

    nsDialogs::Create 1018
    Pop $Dialog
    ${If} $Dialog == error
        Abort
    ${EndIf}

    ${NSD_CreateLabel} 0 0 100% 12u "This application requires the Remote Desktop client for Windows Desktop installation."
    Pop $0
    ${NSD_CreateLabel} 0 12u 100% 12u "Do you want to download the Remote Desktop client now? You can download it later also."
    Pop $0

    ${NSD_CreateLink} 0 36u 100% 12u "Click to open the download page of the Remote Desktop client in your browser."
    Pop $MsrdcLink
    CreateFont $1 "Tahoma" 8 500 /UNDERLINE
    SendMessage $MsrdcLink ${WM_SETFONT} $1 1
    ${NSD_OnClick} $MsrdcLink "onDownloadMsrdcLinkClick"

    ${If} ${RunningX64}
        ${NSD_CreateLabel} 0 48u 100% 12u "Then download the $\"Windows 64-bit$\" file from the page and install it."
        Pop $0
    ${Else}
        ${NSD_CreateLabel} 0 48u 100% 12u "Then download the $\"Windows 32-bit$\" file from the page and install it."
        Pop $0
    ${EndIf}

    nsDialogs::Show

FunctionEnd

Function "onDownloadMsrdcLinkClick"
    Pop $0
    ExecShell "open" "https://docs.microsoft.com/en-us/windows-server/remote/remote-desktop-services/clients/windowsdesktop"
FunctionEnd

#
# Sections
#

Section "install"

    # Deploy the application files into the install path.
    SetOutPath "$INSTDIR"
    File "..\msrdcui\bin\Release\${APP_EXE_FILE_NAME}"
    File "..\msrdcui\bin\Release\${APP_EXE_FILE_NAME}.config"
    File "..\msrdcui\bin\Release\rdclauncher.pdb"
    File "..\..\LICENSE"
    File "..\..\ThirdPartyNotices.txt"

    # Create an uninstaller in the install path.
    WriteUninstaller "$INSTDIR\${UNINSTALLER_EXE_FILE_NAME}"

    # Create a start menu item.
    CreateShortCut "$SMPROGRAMS\${APP_DISPLAY_NAME}.lnk" "$\"$INSTDIR\${APP_EXE_FILE_NAME}$\""

    # Get the installation size.
    ${GetSize} "$INSTDIR" "/S=0K" $0 $1 $2
    !define INSTALLATION_SIZE_KB $0

    # Write the uninstall information into the registry.
    WriteRegStr HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APP_NAME}" "DisplayName" "${APP_DISPLAY_NAME}"
    WriteRegStr HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APP_NAME}" "DisplayVersion" "${APP_VERSION}"
    WriteRegStr HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APP_NAME}" "DisplayIcon" "$\"$INSTDIR\${APP_EXE_FILE_NAME}$\""
    WriteRegDWORD HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APP_NAME}" "EstimatedSize" ${INSTALLATION_SIZE_KB}
    WriteRegStr HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APP_NAME}" "InstallLocation" "$\"$INSTDIR$\""
    WriteRegStr HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APP_NAME}" "UninstallString" "$\"$INSTDIR\${UNINSTALLER_EXE_FILE_NAME}$\""
    WriteRegStr HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APP_NAME}" "QuietUninstallString" "$\"$INSTDIR\${UNINSTALLER_EXE_FILE_NAME}$\" /S"
    WriteRegStr HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APP_NAME}" "Publisher" "${PUBLISHER}"
    WriteRegStr HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APP_NAME}" "URLInfoAbout" "${URL_INFO_ABOUT}"
    WriteRegStr HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APP_NAME}" "URLUpdateInfo" "${URL_UPDATE_INFO}"
    WriteRegStr HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APP_NAME}" "HelpLink" "${HELP_LINK}"
    WriteRegDWORD HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APP_NAME}" "NoModify" 1
    WriteRegDWORD HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APP_NAME}" "NoRepair" 1
    WriteRegDWORD HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APP_NAME}" "VersionMajor" ${APP_VERSION_MAJOR}
    WriteRegDWORD HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APP_NAME}" "VersionMinor" ${APP_VERSION_MINOR}

SectionEnd

Section "uninstall"

    # Delete the installed applicaton files.
    Delete "$INSTDIR\${APP_EXE_FILE_NAME}"
    Delete "$INSTDIR\${APP_EXE_FILE_NAME}.config"
    Delete "$INSTDIR\rdclauncher.pdb"
    Delete "$INSTDIR\LICENSE"
    Delete "$INSTDIR\ThirdPartyNotices.txt"

    # Delete the uninstaller.
    Delete "$INSTDIR\${UNINSTALLER_EXE_FILE_NAME}"

    # Try to delete the install directory.
    RMDir "$INSTDIR"

    # Delete the start menu item.
    Delete "$SMPROGRAMS\${APP_DISPLAY_NAME}.lnk"

    # Delete the uninstall information from the registry.
    DeleteRegKey HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APP_NAME}"

    # Delete the app settings file.
    RMDir /r /REBOOTOK "$LOCALAPPDATA\rdclauncher"

SectionEnd
