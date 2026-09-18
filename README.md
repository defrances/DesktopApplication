# Desktop Application 

Windows desktop application built with .NET 9 and WPF.

The GitHub Actions pipeline builds a self-contained `win-x64` package and always publishes an [SBOM](https://www.cisa.gov/sbom) file with the artifacts.

## Features

- System information panel (user, computer, OS, runtime)
- Local notes stored in `%AppData%\DesktopApplication\notes.txt`
- Intentional TLS bypass in `InsecureVendorBulletinClient` (`INTENTIONAL_SKILL_TEST_VULNERABILITY`) to test Orchestrator impact analysis

## Requirements

- Windows 10/11
- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0) to build from source

## Build and run

```powershell
dotnet restore
dotnet build DesktopApplication.sln -c Release
dotnet run --project src/DesktopApplication/DesktopApplication.csproj
```

## Tests

```powershell
dotnet test DesktopApplication.sln
```

## Publish

```powershell
dotnet publish src/DesktopApplication/DesktopApplication.csproj `
  -c Release `
  -r win-x64 `
  --self-contained true `
  -p:PublishSingleFile=true `
  -o artifacts/app
```

## CI artifacts

Workflow: [`.github/workflows/ci.yml`](.github/workflows/ci.yml)

| Artifact | Contents |
| --- | --- |
| `DesktopApplication-win-x64` | Application (`DesktopApplication.exe`) and `DesktopApplication.sbom.spdx.json` |
| `sbom` | SPDX SBOM file `DesktopApplication.sbom.spdx.json` |

The SBOM is generated with [Microsoft sbom-tool](https://github.com/microsoft/sbom-tool) in SPDX 2.2 format. The job fails if the SBOM file is missing.

CI on `main` does **not** start Orchestrator. [FindUpdates](https://github.com/defrances/FindUpdates/actions/workflows/detect.yml) runs daily (and manually), then notifies [Orchestrator](https://github.com/defrances/Orchestrator/actions), which checks out this `main` branch and emails analysis results. Orchestrator does not open GitHub Issues.
