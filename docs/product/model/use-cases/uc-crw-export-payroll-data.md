---
id: UC-CRW-EXPORT-PAYROLL-DATA
type: use-case
title: "Export hours for payroll"
status: draft
primary-actor: "ACT-CREW-PLANNER"
supporting-actors: []
bounded-context: "BC-CREW"
governed-by:
  - "BR-CRW-EXPORT-PENDING-WARNING"
  - "BR-CRW-NO-PAYROLL-PROCESSING"
  - "BR-CRW-PAYROLL-CODE-IN-EXPORT"
  - "BR-CRW-ACTIVITY-DURATION-CONSISTENCY"
  - "BR-CRW-HOUR-TYPE-TOTALS"
uses-terms:
  - "TERM-PAYROLL-EXPORT"
provenance:
  source: ".private/research/help-center/articles/23814779870610.md, .private/research/help-center/articles/27336532785298.md, .private/research/help-center/articles/24569529468562.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Goal

Payroll receives worked hours.

## Trigger

Payroll period closes.

## Preconditions

Registrations exist.

## Main Flow

1. The user selects registrations.
2. The user chooses registration columns (visible, all or manual) and whether and which activity columns to include.
3. The product warns if any are pending and produces a spreadsheet file.

## Alternative Flows

None.

## Failure Conditions

- The user lacks permission to view or export the selected hours: the export is refused and no spreadsheet is produced.
- No registrations match the selection: no spreadsheet is produced and the user is told the selection is empty.
- Some selected registrations are still pending: a warning is shown, but the export remains possible (BR-CRW-EXPORT-PENDING-WARNING).
- The spreadsheet cannot be produced or delivered: the export fails visibly and can be retried.

## Postconditions

A file exists for the payroll system.
