---
id: BR-EQP-DEFAULT-CONTENT-CATALOG-ONLY
type: business-rule
title: "Default content is maintained in the catalog"
status: draft
applies-to:
  - "UC-EQP-DEFINE-PHYSICAL-KIT"
  - "UC-EQP-DEFINE-VIRTUAL-KIT"
uses-terms:
  - "TERM-DEFAULT-CONTENT"
provenance:
  source: ".private/research/help-center/articles/20641508946450.md, .private/research/help-center/articles/360013703140.md, .private/research/help-center/articles/20045646271250.md"
  confidence: high
  recovered-from: documentation
---

## Rule

Default content of a kit is defined only by editing the kit in the catalog or through a content import; scanning never changes default content. Editing content on a project never changes default content.

## Rationale

The catalog definition must not drift with warehouse activity.

## Examples

- Scanning a cable while booking out a case does not add a cable to the case's default content.

## Exceptions

None.
