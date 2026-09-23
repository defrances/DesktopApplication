# Test plan

xUnit traits use `Category` = `Unit` | `Smoke` | `Regression`.

CI default (`push` / `pull_request`) runs **all** tests.
Orchestrator / `workflow_dispatch` may pass `filter=Smoke`, `filter=Regression`,
or `filter=all`.

| ID | Category | Guarded files | What it proves |
| --- | --- | --- | --- |
| TC-UNIT-NOTES-EMPTY | Unit | `NoteStore.cs` | Missing file loads as empty string |
| TC-UNIT-NOTES-PATH | Unit | `NoteStore.cs` | Empty path is rejected |
| TC-UNIT-TLS-MARKER | Unit | `InsecureVendorBulletinClient.cs` | Intentional-vuln marker is present while the bypass exists |
| TC-SMOKE-NOTES | Smoke | `NoteStore.cs` | Save then load round-trip |
| TC-SMOKE-SYSINFO | Smoke | `SystemInformation.cs` | Provider returns non-empty host fields |
| TC-REG-TLS-CALLBACK | Regression | `InsecureVendorBulletinClient.cs` | Certificate callback contract (insecure until the PDLC patch; must reject invalid chains after the patch) |

## Filters

```powershell
dotnet test DesktopApplication.sln --filter "Category=Smoke"
dotnet test DesktopApplication.sln --filter "Category=Regression"
dotnet test DesktopApplication.sln --filter "Category=Unit"
dotnet test DesktopApplication.sln
```

There are no UI automation tests. Smoke and regression here are Core xUnit
cases selected for a PDLC run, not a full clinical protocol.
