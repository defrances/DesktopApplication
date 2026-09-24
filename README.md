# DesktopApplication

Windows desktop application (.NET 9, WPF). This repository is the **application under test**. Vendor-update intelligence and the PDLC follow-through live in [FindUpdates](https://github.com/defrances/FindUpdates) and [Orchestrator](https://github.com/defrances/Orchestrator).

GitHub Actions builds a self-contained `win-x64` package and always publishes an [SBOM](https://www.cisa.gov/sbom) with the artifacts. CI on `main` does **not** start Orchestrator.

## What the app does

- System information panel (user, computer, OS, runtime)
- Local notes in `%AppData%\DesktopApplication\notes.txt`
- Vendor bulletin HTTPS check (`InsecureVendorBulletinClient` / `CheckBulletinCommand`)
- Notes export (`NoteStore.ExportCopy`)

## Security notes (code vs docs)

TLS validation on `main` requires `SslPolicyErrors.None` (marker `TLS_CERTIFICATE_VALIDATED`). Regression `TC-REG-TLS-CALLBACK` expects an invalid chain to be rejected.

`docs/vulnerability-report.md` and `docs/mds2.md` still describe VR-TLS-001 as open / MDS2-TLS not met. That corpus is the **PDLC input** and is intentionally not rewritten when the code changes. Orchestrator scores countermeasures from `docs/` plus the tree on `main`; AI may mark a finding present even when the markdown still says open.

`NoteStore.ExportCopy` is an intentional path-traversal test (`INTENTIONAL_SKILL_TEST_VULNERABILITY`): the destination name is not constrained, so `..` can write outside the notes folder. It is **not** listed in `docs/vulnerability-report.md`.

The published exe is `--self-contained true`. A host Windows / .NET KB does not patch the runtime inside the zip.

## Role in the three-repo chain

```text
FindUpdates detect (daily / manual)
  → station_report (which OS KB applies to which workstation)
  → Orchestrator Vendor impact and PDLC
      → vendor-impact email (KB vs this app on main)
      → PDLC score from docs/ + main
      → Smoke/Regression on this tree
      → app zip + Windows KB bundle (manifest, not .msu)
```

This repository does not poll MSRC and does not send the results email.

## Product documentation (PDLC corpus)

Orchestrator copies these files into `inputs/` on each follow-through run.

| Doc | Role |
| --- | --- |
| [docs/architecture.md](docs/architecture.md) | Process boundaries, modules, host vs bundled runtime |
| [docs/mds2.md](docs/mds2.md) | Short security disclosure (MDS2-lite) |
| [docs/test-plan.md](docs/test-plan.md) | Unit / smoke / regression matrix |
| [docs/vulnerability-report.md](docs/vulnerability-report.md) | Product findings for the PDLC skill (may lag `main`) |

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

Categories are xUnit traits `Category=Unit|Smoke|Regression`. Default CI runs all. Orchestrator re-runs Smoke + Regression as a release gate.

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
| `DesktopApplication-win-x64` | `DesktopApplication.exe` and `DesktopApplication.sbom.spdx.json` |
| `sbom` | SPDX SBOM `DesktopApplication.sbom.spdx.json` |

The SBOM is generated with [Microsoft sbom-tool](https://github.com/microsoft/sbom-tool) (SPDX 2.2). The job fails if the file is missing.

Manual **Release package** (`.github/workflows/release.yml`) uploads `release-package`: exe, SBOM, `RELEASE_NOTES.md`, `TEST_RESULTS.md`. It does not include Windows OS KBs.

Orchestrator follow-through: [FindUpdates detect](https://github.com/defrances/FindUpdates/actions/workflows/detect.yml) then [Vendor impact and PDLC](https://github.com/defrances/Orchestrator/actions). GitHub Issues are not created for vendor clusters.
