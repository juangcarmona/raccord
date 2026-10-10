---
id: BR-WHS-STATUS-INDEPENDENCE
type: business-rule
title: "Project status and equipment status are independent"
status: draft
applies-to:
  - "UC-WHS-BOOK-MANUALLY"
  - "UC-WHS-REVIEW-WAREHOUSE-BOARD"
  - "UC-WHS-MARK-DELAYED"
  - "BC-WAREHOUSE"
  - "UC-PRJ-CHANGE-PROJECT-STATUS"
  - "BC-PROJECTS"
uses-terms:
  - "TERM-WAREHOUSE-STATUS"
  - "TERM-PROJECT-STATUS"
provenance:
  source: ".private/research/help-center/articles/15239125718546.md, .private/research/help-center/articles/23280754011154.md, .private/research/help-center/articles/15125682136850.md, .private/research/help-center/articles/13901986722322.md, .private/research/help-center/articles/23361953107346.md"
  confidence: high
  recovered-from: documentation
---

## Rule

Changing a project's status never moves equipment between warehouse statuses, and booking equipment never sets a project status directly; when all equipment of a packing list reaches a warehouse status, the product proposes that status for the packing list only. Each item keeps its own handling status, which may differ from the project status; the project follows only its own lifecycle (BR-INT-PROJECT-MANAGEMENT-LIFECYCLE).

## Rationale

Paperwork and physical reality evolve separately.

## Examples

See SB-WHS-PROJECT-STATUS-DOES-NOT-RETURN-GEAR and SB-PRJ-EARLY-RETURN-PARTIAL.

## Exceptions

None.
