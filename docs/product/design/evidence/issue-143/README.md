# Issue 143 evidence: warehouse packing and dispatch

The Playwright journey creates a project and equipment item, plans the equipment, creates its packing list, and drives the same authoritative line through packed, dispatched and received states with named operators.

Product references:

- [UC-WHS-ASSIGN-SCAN-CODES](../../../model/use-cases/uc-whs-assign-scan-codes.md)
- [UC-WHS-PRINT-CODE-LABELS](../../../model/use-cases/uc-whs-print-code-labels.md)
- [UC-WHS-BOOK-BY-SCANNING](../../../model/use-cases/uc-whs-book-by-scanning.md)
- [UC-WHS-BOOK-MANUALLY](../../../model/use-cases/uc-whs-book-manually.md)

The current slice proves the shared packing lifecycle through the authenticated API; scan-device integration and printable packing-slip presentation remain separate boundaries.

The hosted CI artifact contains the screenshot and trace from the successful run.
