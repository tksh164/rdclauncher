# RDC Launcher

[![Open in Visual Studio Code](https://open.vscode.dev/badges/open-in-vscode.svg)](https://open.vscode.dev/tksh164/rdclauncher)

The RDC Launcher allows to use the Remote Desktop client (msrdc) as like the Remote Desktop Connection (mstsc). You can leverage the benefit of the Remote Desktop client. e.g. Advanced display settings, window title naming.

<img src="./media/screenshot01.png" width="60%" alt="Screenshot of the app.">

<img src="./media/screenshot02.png" width="70%" alt="Screenshot of the advanced display settings in the Remote Desktop client.">

## 📋 Prerequisites

- Windows 10
- .NET Framework 4.7.2 or 4.8
  - Windows 10 has the .NET Framework 4.7.2 by default.
- Remote Desktop client (MSRDC)
  - You can download the Remote Desktop client installer from the [Microsoft's web page](https://docs.microsoft.com/en-us/windows-server/remote/remote-desktop-services/clients/windowsdesktop). Download it from the **Windows 64-bit** link if you use 64-bit Windows, also download it from the **Windows 32-bit** link if you use 32-bit Windows.

## 📥 Install

1. Download [the zip file](https://github.com/tksh164/rdclauncher/releases/latest).
    - After the download the zip file, you can unblock the zip file from the file's property or using [Unblock-File](https://docs.microsoft.com/en-us/powershell/module/microsoft.powershell.utility/unblock-file) cmdlet.
2. Extract to files from the zip file. You can extract files from the `Extract All...` context menu in the File Explorer or using [Expand-Archive](https://docs.microsoft.com/en-us/powershell/module/microsoft.powershell.archive/expand-archive) cmdlet.
3. Locate to the extracted files to anywhere you like.

If you don't need this app anymore, you can uninstall it by delete the located folder.

## 💡 Tips

- You can change the default settings of this app by edit the `rdclauncher.exe.config` that placed in the same place as the application's executable file (`rdclauncher.exe`).

  - `PortNumber`: The default port number.

    ```
    <setting name="PortNumber" serializeAs="String">
        <value>3389</value>
    </setting>
    ```

  - `DefaultFitSessionToWindowEnabled`: If set True, the `Fit session to window` checkbox is checked by default.

    ```
    <setting name="DefaultFitSessionToWindowEnabled" serializeAs="String">
        <value>False</value>
    </setting>
    ```

  - `DefaultUpdateResolutionOnResizeEnabled`: If set True, the `Update resolution on resize` checkbox is checked by default.

    ```
    <setting name="DefaultUpdateResolutionOnResizeEnabled" serializeAs="String">
        <value>True</value>
    </setting>
    ```

  - `DefaultFullScreenEnabled`: If set True, the `Full screen` checkbox is checked by default.

    ```
    <setting name="DefaultFullScreenEnabled" serializeAs="String">
        <value>False</value>
    </setting>
    ```

- The history file located at `%LocalAppData%\rdclauncher\rdclauncher.exe_Url_<random-string>\<version>\user.config`. Delete this file if you want to delete the history.

## 🔨 Build from source

You can build the project using [Visual Studio](https://visualstudio.microsoft.com/).

## ⚖ License

Copyright (c) 2021-present Takeshi Katano. All rights reserved. This software is released under the [MIT License](https://github.com/tksh164/rdclauncher/blob/master/LICENSE).

Disclaimer: The codes stored herein are my own personal codes and do not related my employer's any way.
