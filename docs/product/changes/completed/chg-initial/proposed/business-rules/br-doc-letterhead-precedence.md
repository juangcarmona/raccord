---
id: "BR-DOC-LETTERHEAD-PRECEDENCE"
type: business-rule
title: "Letterhead and template choice at generation overrides defaults"
status: draft
applies-to:
  - "UC-DOC-GENERATE-DOCUMENT"
  - "UC-DOC-REGENERATE-WITH-OTHER-LAYOUT"
uses-terms:
  - "TERM-LETTERHEAD"
  - "TERM-DOCUMENT-TEMPLATE"
  - "TERM-PROJECT-TYPE"
provenance:
  source: ".private/research/help-center/articles/360014055700.md, .private/research/help-center/articles/360017648260.md, .private/research/help-center/articles/360013503420.md"
  confidence: medium
  recovered-from: documentation
---

## Rule

When a document is generated, the product proposes the selected template's own default letterhead when one exists; otherwise it proposes the default letterhead configured for the project's type. The user may replace either for that document.

## Rationale

Defaults speed up routine work without preventing exceptions.

## Examples

- A festival project type proposes the festival letterhead; the user switches to the neutral letterhead for one quotation.

## Exceptions

The more-specific template default takes precedence over the broader project-type default.
