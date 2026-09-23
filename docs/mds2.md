# MDS2-lite (Manufacturer Disclosure Statement)

Short security disclosure for this PoC. Not the full MDS2 2020 questionnaire
and not a 510(k) submission.

| ID | Topic | Status | Statement |
| --- | --- | --- | --- |
| MDS2-ID | Identification | met | Windows WPF client, assembly `DesktopApplication`, version from csproj. |
| MDS2-NET | Network connections | met | One optional outbound HTTPS GET (`InsecureVendorBulletinClient`). No inbound ports. |
| MDS2-TLS | Cryptography / TLS | **not met** | `AcceptAnyServerCertificate` always returns `true`. The process uses host Schannel for the handshake, then **discards** certificate validation results. Marker: `INTENTIONAL_SKILL_TEST_VULNERABILITY`. |
| MDS2-DATA | Data at rest | met | Local notes file only; no PHI; no remote storage. |
| MDS2-AUTH | Authentication | n/a | Single-user desktop process; Windows session identity only. |
| MDS2-PATCH | Patch model | met | New self-contained `win-x64` exe + SPDX SBOM. Host OS KBs are recommended separately and are not packaged with the app. |
| MDS2-AUDIT | Audit | partial | No application audit log. Build evidence is CI artifacts (exe, SBOM, test output). |
| MDS2-SBOM | Software bill of materials | met | SPDX 2.2 generated in CI (`microsoft.sbom.dotnettool`). |

## How to read status

- `met` — a control exists in code or process on `main`.
- `not met` — a documented gap; PDLC must treat this as missing countermeasure.
- `partial` / `n/a` — limited applicability for this PoC client.
