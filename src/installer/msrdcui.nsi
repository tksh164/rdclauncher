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
!define APP_VERSION_MINOR 6
!define APP_VERSION_PATCH 0
!define APP_VERSION "${APP_VERSION_MAJOR}.${APP_VERSION_MINOR}.${APP_VERSION_PATCH}"
!define APP_ICON "..\msrdcui\Resources\msrdcui0.ico"
!define APP_EXE_FILE_NAME "rdclauncher.exe"
!define UNINSTALLER_EXE_FILE_NAME "uninstall.exe"

#
# Installer's version infomation
#

VIProductVersion "${APP_VERSION}.0"
VIFileVersion "${APP_VERSION}.0"
VIAddVersionKey /LANG=0 "ProductName" "${APP_DISPLAY_NAME}"
VIAddVersionKey /LANG=0 "ProductVersion" "${APP_VERSION}"
VIAddVersionKey /LANG=0 "FileVersion" "${APP_VERSION}"
VIAddVersionKey /LANG=0 "FileDescription" "RDC Launcher allows to use the Remote Desktop client (msrdc) as like the Remote Desktop Connection (mstsc)."
VIAddVersionKey /LANG=0 "LegalCopyright" "Copyright (c) 2021-present Takeshi Katano. All rights reserved."

#
# Uninstall information
#

!define APP_GUID "{D08CADB0-E33F-43DD-AF24-3877D7590576}"
!define PUBLISHER "Takeshi Katano"
!define URL_INFO_ABOUT "https://github.com/tksh164/rdclauncher"
!define URL_UPDATE_INFO "https://github.com/tksh164/rdclauncher/releases/latest"
!define HELP_LINK "https://github.com/tksh164/rdclauncher"

#
# General
#

# Installer file name.
OutFile "setup-${APP_NAME_LC}-${APP_VERSION}.exe"

# Install path.
InstallDir "$APPDATA\${APP_NAME}"

# Requested execution privileges for the installation.
RequestExecutionLevel user

# Compression settings
SetCompress auto
SetCompressor zlib

# Saving the date and time of files.
SetDateSave on

# Overwrite any existing files.
SetOverwrite on

# Set the installer font.
SetFont /LANG=${LANG_ENGLISH} "Segoe UI" 10

# Show details
ShowInstDetails show
ShowUninstDetails show

# Enable DPI-aware
ManifestDPIAware true

#
# Modern UI
#

# Application name.
Name "${APP_DISPLAY_NAME}"

# Installer/Uninstaller icon.
!define MUI_ICON "${APP_ICON}"
!define MUI_UNICON "${APP_ICON}"

# Show a warning message when the user cancel installation.
!define MUI_ABORTWARNING

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
# Sections
#

Section "install"

    # Verify the previous installation existence.
    ${If} ${RunningX64}
        SetRegView 64
    ${EndIf}
    ClearErrors
    ReadRegStr $0 HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APP_GUID}" "QuietUninstallString"
    ${IfNot} ${Errors}
    ${AndIf} $0 != ""
        DetailPrint "The previous installation found. Uninstall the previous installation before this installation."
        ExecWait "$0 _?=$INSTDIR"
    ${EndIf}

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
    WriteRegStr HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APP_GUID}" "DisplayName" "${APP_DISPLAY_NAME}"
    WriteRegStr HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APP_GUID}" "DisplayVersion" "${APP_VERSION}"
    WriteRegStr HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APP_GUID}" "DisplayIcon" "$\"$INSTDIR\${APP_EXE_FILE_NAME}$\""
    WriteRegDWORD HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APP_GUID}" "EstimatedSize" ${INSTALLATION_SIZE_KB}
    WriteRegStr HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APP_GUID}" "InstallLocation" "$\"$INSTDIR$\""
    WriteRegStr HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APP_GUID}" "UninstallString" "$\"$INSTDIR\${UNINSTALLER_EXE_FILE_NAME}$\""
    WriteRegStr HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APP_GUID}" "QuietUninstallString" "$\"$INSTDIR\${UNINSTALLER_EXE_FILE_NAME}$\" /S"
    WriteRegStr HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APP_GUID}" "Publisher" "${PUBLISHER}"
    WriteRegStr HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APP_GUID}" "URLInfoAbout" "${URL_INFO_ABOUT}"
    WriteRegStr HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APP_GUID}" "URLUpdateInfo" "${URL_UPDATE_INFO}"
    WriteRegStr HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APP_GUID}" "HelpLink" "${HELP_LINK}"
    WriteRegDWORD HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APP_GUID}" "NoModify" 1
    WriteRegDWORD HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APP_GUID}" "NoRepair" 1
    WriteRegDWORD HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APP_GUID}" "VersionMajor" ${APP_VERSION_MAJOR}
    WriteRegDWORD HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APP_GUID}" "VersionMinor" ${APP_VERSION_MINOR}

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
    DeleteRegKey HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APP_GUID}"

    # Delete the app settings file.
    RMDir /r /REBOOTOK "$LOCALAPPDATA\rdclauncher"

SectionEnd

#
# The Remote Desktop client download page
#

Var Dialog
Var MsrdcLink
Function "MsrdcDownloadPageCreator"

    # Verify the installation of the Remote Desktop client.
    # NOTE: The uninstall registry key is not use for the Remote Desktop client existence verification because the GUID of the registry key is deferent for each machine.
    ${If} ${FileExists} "$LOCALAPPDATA\Apps\Remote Desktop\msrdc.exe"  # For user install x64/x86
        Abort  # Skip this page if the Remote Desktop client already installed.
    ${EndIf}
    ${If} ${RunningX64}
        ${If} ${FileExists} "$PROGRAMFILES64\Remote Desktop\msrdc.exe"  # For machine install x64
            Abort  # Skip this page if the Remote Desktop client already installed.
        ${EndIf}
    ${Else}
        ${If} ${FileExists} "$PROGRAMFILES\Remote Desktop\msrdc.exe"  # For machine install x86
            Abort  # Skip this page if the Remote Desktop client already installed.
        ${EndIf}
    ${EndIf}

    !insertmacro MUI_HEADER_TEXT "Require the Microsoft Remote Desktop client" "Download and install the Microsoft Remote Desktop client."

    nsDialogs::Create 1018
    Pop $Dialog
    ${If} $Dialog == error
        Abort
    ${EndIf}

    ${NSD_CreateLabel} 0 0 100% 24u "RDC Launcher requires the Microsoft Remote Desktop client installation.$\nDo you want to download the Microsoft Remote Desktop client now? You can download it later also."
    Pop $0

    ${NSD_CreateLink} 0 36u 100% 12u "Open the download page of the Microsoft Remote Desktop client in your browser."
    Pop $MsrdcLink
    CreateFont $1 "$(^Font)" $(^FontSize) 500 /UNDERLINE
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
