param (
    [Parameter(Mandatory)]
    [string] $TxtFilePath,

    [Parameter(Mandatory)]
    [string] $RtfFilePath
)

$txtFileContent = (Get-Content -LiteralPath $TxtFilePath -Encoding UTF8 -Raw).
    Replace('\', '\\').
    Replace('{', '\{').
    Replace('}', '\}').
    Replace([System.Environment]::NewLine, ' \par ')
$rtfFileContnet = '{\rtf1\ansi{\fonttbl\f0\fswiss Calibri;}\f0\fs17\pard ' + $txtFileContent + ' }'
Set-Content -LiteralPath $RtfFilePath -Encoding Ascii -NoNewline -Value $rtfFileContnet
