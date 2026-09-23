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

## Product documentation (PDLC corpus)

| Doc | Role |
| --- | --- |
| [docs/architecture.md](docs/architecture.md) | Process boundaries, modules, host vs bundled runtime |
| [docs/mds2.md](docs/mds2.md) | Short security disclosure (MDS2-lite) |
| [docs/test-plan.md](docs/test-plan.md) | Unit / smoke / regression matrix |
| [docs/vulnerability-report.md](docs/vulnerability-report.md) | Product findings for the PDLC skill |

## Tests

Categories are xUnit traits `Category=Unit|Smoke|Regression`. Default CI runs all.

```powershell
dotnet test DesktopApplication.sln
dotnet test DesktopApplication.sln --filter "Category=Smoke"
dotnet test DesktopApplication.sln --filter "Category=Regression"
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

Manual **Release package** workflow (`.github/workflows/release.yml`) uploads `release-package`: a zip with the exe, SBOM, `RELEASE_NOTES.md`, and `TEST_RESULTS.md`. It does not include Windows OS KBs.

CI on `main` does **not** start Orchestrator. [FindUpdates](https://github.com/defrances/FindUpdates/actions/workflows/detect.yml) runs daily (and manually), then notifies [Orchestrator](https://github.com/defrances/Orchestrator/actions). That `findupdates-complete` event starts one Orchestrator run: vendor email plus product PDLC (docs + vulnerability report → tests → release zip + host KB bundle). Orchestrator does not open GitHub Issues.
