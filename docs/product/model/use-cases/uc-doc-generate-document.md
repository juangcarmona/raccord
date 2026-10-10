---
id: "UC-DOC-GENERATE-DOCUMENT"
type: use-case
title: "Generate a document from a record"
status: draft
primary-actor: "ACT-OPERATIONS-COORDINATOR"
supporting-actors:
  - "ACT-WAREHOUSE-TECHNICIAN"
bounded-context: "BC-DOCUMENTS"
governed-by:
  - "BR-DOC-TYPE-SCOPES-TEMPLATE"
  - "BR-DOC-LETTERHEAD-PRECEDENCE"
  - "BR-DOC-LETTERHEAD-TYPE-AVAILABILITY"
  - "BR-DOC-LETTERHEAD-PAGE-MAPPING"
  - "BR-DOC-COMBINATION-CONTENT-DISPLAY"
  - "BR-FIN-DEFAULTS-BY-PROJECT-TYPE"
  - "BR-PRJ-DOCUMENT-REGENERATION"
  - "BR-PRJ-REMEMBER-LAST-TEMPLATE"
uses-terms:
  - "TERM-GENERATED-DOCUMENT"
  - "TERM-DOCUMENT-TEMPLATE"
  - "TERM-LETTERHEAD"
  - "TERM-PERSONAL-TEXT"
  - "TERM-ATTACHED-FILE"
provenance:
  source: ".private/research/help-center/articles/360013503420.md, .private/research/help-center/articles/360014055700.md, .private/research/help-center/articles/360017645460.md, .private/research/help-center/articles/360017648140.md, .private/research/help-center/articles/360017647860.md, .private/research/help-center/articles/360014260839.md, .private/research/help-center/articles/360017700539.md, .private/research/help-center/articles/360013838560.md, .private/research/help-center/articles/360013631800.md, .private/research/help-center/articles/360013677019.md"
  confidence: high
  recovered-from: documentation
---

## Goal

A document of the chosen type is produced for a record with the right template, letterhead and content.

## Trigger

A quotation, slip, label, reminder or other document is needed for a record.

## Preconditions

The document type has its built-in template.

## Main Flow

1. The user starts creating a document from the record.
2. The product proposes the default template and letterhead (from the project type or the template).
3. The user optionally changes template and letterhead, sets the file name and e-mail subject, overrides combination content display and VAT display and adds a personal text. For projects, the template last used for that project type is proposed (BR-PRJ-REMEMBER-LAST-TEMPLATE).
4. The product generates the document, filling in the record's data.
5. The user downloads it (PDF or word-processor format), saves it to the record's files, or e-mails it, optionally requesting a digital signature.

## Alternative Flows

- The user regenerates with other choices (UC-DOC-REGENERATE-WITH-OTHER-LAYOUT).
- During handling in the mobile web app a packing slip is produced and can be signed on the spot.

## Failure Conditions

- No letterhead is enabled for the document type: none can be chosen.
- Too much content for a small page overflows onto further pages or labels.
- Record changes made after generation are not reflected until the document is regenerated (BR-PRJ-DOCUMENT-REGENERATION).

## Postconditions

A generated document exists reflecting the record at generation time.
