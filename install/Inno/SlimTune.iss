[Setup]
; NOTE: The value of AppId uniquely identifies this application.
; Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{72513408-FD69-404F-9FDB-2DB6A7DCDE84}
AppName=SlimTune Profiler
AppVerName=SlimTune Profiler v0.1.3
AppPublisher=SlimDX Group
AppPublisherURL=http://www.slimtune.com/
AppSupportURL=http://www.slimtune.com/
AppUpdatesURL=http://www.slimtune.com/
DefaultDirName={pf}\SlimTune Profiler
DefaultGroupName=SlimTune Profiler
LicenseFile=..\ExtraFiles\License.rtf
OutputDir=D:\Promit\Documents\SlimTune\trunk\install\Inno
OutputBaseFilename=SlimTune-0.1.3
Compression=lzma
SolidCompression=yes
VersionInfoVersion=0.1.3.0

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Files]
Source: "..\ExtraFiles\vcredist_x86.exe"; DestDir: "{tmp}"; Flags: ignoreversion; AfterInstall: InstallVCRedist

Source: "..\publish\SlimTuneUI.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\publish\WeifenLuo.WinFormsUI.Docking.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\publish\MediaLicense.txt"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\publish\CodeLicense.txt"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\publish\Backends\SlimTuneCLR.dll"; DestDir: "{app}\Backends"; Flags: regserver ignoreversion
Source: "..\publish\Backends\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs

Source: "..\ExtraFiles\SSCERuntime-ENU-x86.msi"; DestDir: "{tmp}"; Flags: ignoreversion
; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[Icons]
Name: "{group}\SlimTune Profiler"; Filename: "{app}\SlimTuneUI.exe"
Name: "{group}\{cm:UninstallProgram,SlimTune Profiler}"; Filename: "{uninstallexe}"

[Tasks]
Name: Firewall; Description: "Open TCP ports 3000 - 3001 in Windows Firewall"; GroupDescription: "Firewall:"; MinVersion: 0,5.01.2600sp2;

[Run]
Filename: "msiexec.exe"; Parameters: "/passive /i ""{tmp}\SSCERuntime-ENU-x86.msi"""; StatusMsg: "Installing SQL Server Compact Edition"
Filename: "{sys}\netsh.exe"; Parameters: "firewall add portopening TCP 3000 ""SlimTune Profiler (3000)"" ENABLE SUBNET"; Tasks: Firewall;
Filename: "{sys}\netsh.exe"; Parameters: "firewall add portopening TCP 3001 ""SlimTune Profiler (3001)"" ENABLE SUBNET"; Tasks: Firewall;

Filename: "{app}\SlimTuneUI.exe"; Description: "{cm:LaunchProgram,SlimTune Profiler}"; Flags: nowait postinstall skipifsilent

[UninstallRun]
Filename: "{sys}\netsh.exe"; Parameters: "firewall delete portopening TCP 3000"; Flags: runhidden; MinVersion:0,5.01.2600sp2; Tasks: Firewall;
Filename: "{sys}\netsh.exe"; Parameters: "firewall delete portopening TCP 3001"; Flags: runhidden; MinVersion:0,5.01.2600sp2; Tasks: Firewall;

[Code]
function InitializeSetup(): Boolean;
var
  NetFrameWorkInstalled : Boolean;
begin
  NetFrameWorkInstalled := RegKeyExists(HKLM,'SOFTWARE\Microsoft\.NETFramework\policy\v2.0');
  if NetFrameWorkInstalled then
  begin
    Result := true;
  end else
  begin
    MsgBox('This setup requires the .NET Framework. Please download and install the .NET Framework and run this setup again.',
      mbConfirmation, MB_OK);
    Result:=false;
  end;
end;

procedure InstallVCRedist();
var
  TempDir:      String;
  RedistPath:   String;
  ReturnCode:   Integer;
begin
  TempDir := ExpandConstant('{tmp}');
  RedistPath := TempDir + '\vcredist_x86.exe';
  
  Exec(RedistPath, '/qb!', TempDir, SW_SHOW, ewWaitUntilTerminated, ReturnCode)
end;
