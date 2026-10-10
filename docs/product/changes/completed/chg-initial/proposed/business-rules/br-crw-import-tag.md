---
id: BR-CRW-IMPORT-TAG
type: business-rule
title: "Each import tags the records it creates"
status: draft
applies-to:
  - "UC-CRW-IMPORT-CREW-MEMBERS"
  - "UC-CRW-IMPORT-VEHICLES"
uses-terms:
  - "TERM-RESOURCE-IMPORT"
  - "TERM-TAG"
provenance:
  source: ".private/research/help-center/articles/360014366299.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Rule

Every crew or vehicle import tags the records it creates with a tag made of the import date and hour, so that a faulty import can be undone by selecting and deleting the records with that tag.

## Rationale

Partial failures are best corrected by removing and redoing the import.

## Examples

An import at 14:05 on 2 June creates records tagged with that moment.

## Exceptions

None.
