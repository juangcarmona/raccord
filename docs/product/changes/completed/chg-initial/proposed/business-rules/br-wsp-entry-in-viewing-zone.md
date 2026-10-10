---
id: BR-WSP-ENTRY-IN-VIEWING-ZONE
type: business-rule
title: "Times are entered in the viewing time zone"
status: draft
applies-to:
  - "UC-WSP-SWITCH-VIEWING-TIME-ZONE"
  - "UC-WSP-SET-TIME-ZONE"
uses-terms:
  - "TERM-VIEWING-TIME-ZONE"
  - "TERM-PROJECT-TIME-ZONE"
provenance:
  source: ".private/research/help-center/articles/115003579769.md"
  confidence: high
  recovered-from: documentation
---

## Rule

Times typed into a project's schedule are interpreted in the user's current viewing time zone, not in the project's time zone.

## Rationale

The same view drives display and entry.

## Examples

- With the view set to Amsterdam and a New York project, a typed 09:00 means 09:00 Amsterdam time.

## Exceptions

None.
