---
id: BR-INT-PROJECT-MANAGEMENT-LIFECYCLE
type: business-rule
title: "Project-management status lifecycle with automatic progress"
status: draft
applies-to:
  - "BC-PROJECTS"
  - "UC-INT-LEAVE-EQUIPMENT-BEHIND"
  - "UC-PRJ-PROGRESS-STATUS-AUTOMATICALLY"
uses-terms:
  - "TERM-PROJECT-STATUS"
provenance:
  source: ".private/research/help-center/articles/37629880908050.md, .private/research/help-center/articles/26707946497042.md"
  confidence: medium
  recovered-from: documentation
---

## Rule

The project status is a pure project-management status ordered Cancelled, Inquiry, Draft, Pending, Confirmed, In progress, Execution done, Complete; In progress is set automatically at the first warehouse booking, Execution done automatically when all equipment of every packing moment is back, and Complete only by a user. Warehouse statuses, including custom ones, are not selectable as project status.

## Rationale

Warehouse progress lives on packing moments.

## Examples

Booking the first cable sets the project to In progress; the last return sets Execution done.

## Exceptions

None: this lifecycle applies to all projects.
