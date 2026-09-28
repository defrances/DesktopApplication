# Security risk management (PoC)

Short product file for **UVCS configuration 1**. Not a full ISO 14971 file and not a 510(k) submission.

Vendor OS and library patches are assessed under **product design control**. The design spec / configuration spec for this product is where patch management is written. This file is the security risk-management record those specs point at.

[FindUpdates](https://github.com/defrances/FindUpdates) and [Orchestrator](https://github.com/defrances/Orchestrator) collect station KB facts, SBOM/source evidence, and the impact email. They are analysis tools. They are **not** the product tool-validation procedure.

## What this file covers

- Host KBs and third-party library advisories that might couple to this product configuration
- Whether SBOM scan and source scan show that dependency in the code on `main`
- Whether the coupling changes product functions, hazards, or tests

## What stays outside this file

- FindUpdates lab promotion gate (`PASS` / `FAIL` / `BLOCKED` / `INCONCLUSIVE`)
- Packaging Microsoft `.msu` / `.cab` installers
- Authorization to install, approve, or deploy in the field
