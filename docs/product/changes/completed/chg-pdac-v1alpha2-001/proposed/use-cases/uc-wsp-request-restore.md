---
id: UC-WSP-REQUEST-RESTORE
type: use-case
title: "Restore the installation from a backup"
status: draft
primary-actor: "ACT-WORKSPACE-ADMINISTRATOR"
supporting-actors: []
bounded-context: "BC-WORKSPACE"
governed-by:
  - "BR-WSP-RESTORE-DISCARDS-LATER"
  - "BR-WSP-BACKUP-SCHEDULE"
uses-terms:
  - "TERM-WORKSPACE-BACKUP"
provenance:
  source: ".private/research/help-center/articles/360015902019.md"
  confidence: medium
  recovered-from: documentation
---
## Goal
The whole installation returns to a correct earlier state.
## Trigger
Large-scale data damage (for example a failed import).
## Preconditions
- A scheduled backup from before the damage exists.
- The administrator has configuration access.
## Main Flow
1. The administrator opens the list of backups and chooses the one from before the damage.
2. The administrator checks the backup's date and confirms the restore explicitly.
3. The product restores the installation to that moment.
## Alternative Flows
- Only a few items are damaged: the administrator re-enters them by hand instead.
## Failure Conditions
- No approval: nothing is restored.
## Postconditions
- The installation equals the chosen backup; later changes are lost.
