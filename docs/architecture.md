# DesktopApplication architecture

Product process boundaries for PDLC analysis. This is the application under
analysis on branch `main`.

## Process

DesktopApplication is a Windows WPF client (`OutputType=WinExe`,
`net9.0-windows`, `UseWPF`). CI publishes a **self-contained** `win-x64`
single-file exe. The published process embeds `Microsoft.NETCore.App` and
`Microsoft.WindowsDesktop.App`. It does **not** embed the host TLS stack
(Schannel), Win32k, DWM, or NTFS.

```
DesktopApplication.exe
├── UI (WPF)          src/DesktopApplication
└── Core (net9.0)     src/DesktopApplication.Core
      ├── NoteStore
      ├── SystemInformationProvider
      └── InsecureVendorBulletinClient
```

## Modules

| Module | Path | Responsibility |
| --- | --- | --- |
| Shell / window | `src/DesktopApplication/MainWindow.xaml` | Two-panel UI, DPI via `app.manifest` (`PerMonitorV2`) |
| View model | `src/DesktopApplication/MainViewModel.cs` | Notes save, `CheckBulletinCommand` |
| Notes | `src/DesktopApplication.Core/NoteStore.cs` | `%AppData%\DesktopApplication\notes.txt` |
| System info | `src/DesktopApplication.Core/SystemInformation.cs` | User, machine, OS, runtime strings |
| Vendor bulletin | `src/DesktopApplication.Core/InsecureVendorBulletinClient.cs` | HTTPS GET to MSRC; **currently trusts every server certificate** |
| Publish | `.github/workflows/ci.yml` | `dotnet publish --self-contained true -r win-x64` + SPDX SBOM |

## Trust boundary

- **Inside the exe:** application logic, bundled .NET runtime, WPF libraries from publish.
- **Host OS (not in the exe):** Schannel (TLS handshake), Win32k (windows/DPI), DWM (composition), NTFS (notes file), Windows Shell (launch).

An OS .NET / .NET Framework KB does not patch the bundled runtime unless publish becomes framework-dependent.

## Network

The only outbound call is `InsecureVendorBulletinClient.FetchAsync` to
`https://www.microsoft.com/en-us/msrc` when the user clicks **Check vendor bulletin**.
There is no listener, no patient data, and no cloud backend.

## Local data

`NoteStore` writes a single UTF-8 text file under the current user's AppData.
No encryption at rest. No PHI by design.
