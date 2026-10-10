---
id: "BR-DOC-TRANSLATED-NAMES"
type: business-rule
title: "Foreign-language documents print translated names from custom fields"
status: draft
applies-to:
  - "UC-DOC-PREPARE-FOREIGN-LANGUAGE-TEMPLATE"
uses-terms:
  - "TERM-CUSTOM-FIELD"
  - "TERM-TEMPLATE-LOCALE"
provenance:
  source: ".private/research/help-center/articles/360014194100.md"
  confidence: medium
  recovered-from: documentation
---

## Rule

To print equipment, crew and transport function names in another language, the document prints the name held in a custom field for the translation instead of the standard name; records without a translation print an empty value.

## Rationale

The product stores one name per record; translations are company data.

## Examples

See SB-DOC-TRANSLATED-EQUIPMENT-NAMES.

## Exceptions

None.
