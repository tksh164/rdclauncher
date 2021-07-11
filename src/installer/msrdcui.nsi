Unicode true
!include "MUI2.nsh"
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
!define UNINSTALLER_NAME "uninstall.exe"

#
# General
#

# Installer file name.
OutFile "setup-${APP_NAME_LC}-${APP_VERSION}.exe"

# Install path.
InstallDir "$APPDATA\${APP_NAME}"

# Requested execution privileges for the installation.
RequestExecutionLevel user

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
!insertmacro MUI_PAGE_LICENSE "${NSISDIR}\Docs\Modern UI\License.txt"

!insertmacro MUI_PAGE_INSTFILES

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

    # Deploy the application files into the install path.
    SetOutPath "$INSTDIR"
    File "..\msrdcui\bin\Release\rdclauncher.exe"
    File "..\msrdcui\bin\Release\rdclauncher.exe.config"
    File "..\msrdcui\bin\Release\rdclauncher.pdb"

    # Create an uninstaller in the install path.
    WriteUninstaller "$INSTDIR\${UNINSTALLER_NAME}"

    # Create a start menu item.
    CreateShortCut "$SMPROGRAMS\${APP_DISPLAY_NAME}.lnk" "$\"$INSTDIR\rdclauncher.exe$\""

    # Get the installation size.
    ${GetSize} "$INSTDIR" "/S=0K" $0 $1 $2
    !define INSTALLATION_SIZE_KB $0

    # Write the uninstall information into the registry.
    WriteRegStr HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APP_NAME}" "DisplayName" "${APP_DISPLAY_NAME}"
    WriteRegStr HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APP_NAME}" "DisplayVersion" "${APP_VERSION}"
    WriteRegDWORD HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APP_NAME}" "EstimatedSize" ${INSTALLATION_SIZE_KB}
    WriteRegStr HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APP_NAME}" "InstallLocation" "$\"$INSTDIR$\""
    WriteRegStr HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APP_NAME}" "UninstallString" "$\"$INSTDIR\${UNINSTALLER_NAME}$\""
    WriteRegStr HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APP_NAME}" "QuietUninstallString" "$\"$INSTDIR\${UNINSTALLER_NAME}$\" /S"
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
    Delete "$INSTDIR\rdclauncher.exe"
    Delete "$INSTDIR\rdclauncher.exe.config"
    Delete "$INSTDIR\rdclauncher.pdb"

    # Delete the uninstaller.
    Delete "$INSTDIR\${UNINSTALLER_NAME}"

    # Try to delete the install directory.
    RMDir "$INSTDIR"

    # Delete the start menu item.
    Delete "$SMPROGRAMS\${APP_DISPLAY_NAME}.lnk"

    # Delete the uninstall information from the registry.
    DeleteRegKey HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APP_NAME}"

    # Delete the app settings file.
    RMDir /r /REBOOTOK "$LOCALAPPDATA\rdclauncher"
SectionEnd
