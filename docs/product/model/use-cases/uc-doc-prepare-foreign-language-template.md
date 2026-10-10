---
id: "UC-DOC-PREPARE-FOREIGN-LANGUAGE-TEMPLATE"
type: use-case
title: "Prepare templates for another language"
status: draft
primary-actor: "ACT-WORKSPACE-ADMINISTRATOR"
supporting-actors: []
bounded-context: "BC-DOCUMENTS"
governed-by:
  - "BR-DOC-TRANSLATED-NAMES"
uses-terms:
  - "TERM-TEMPLATE-LOCALE"
  - "TERM-CUSTOM-FIELD"
provenance:
  source: ".private/research/help-center/articles/360014194100.md, .private/research/help-center/articles/360013503420.md, .private/research/help-center/articles/23413689614226.md"
  confidence: medium
  recovered-from: documentation
---

## Goal

Documents can be sent to customers in another language, including translated equipment and function names.

## Trigger

The company serves customers abroad.

## Preconditions

Custom fields holding translations exist for equipment and default functions and are filled.

## Main Flow

1. The administrator sets the language and country of the built-in template of the document types used for those customers.
2. The product prints the translated names from the translation custom fields instead of the names in the catalogue.
3. The administrator does the same for other translatable fields such as external remarks.
4. The administrator saves.
5. The administrator creates e-mail text templates in the language.

## Alternative Flows

Translations for equipment are imported in bulk; those of default functions are entered manually.

## Failure Conditions

Records lacking a translation print empty names.

## Postconditions

Documents in the target language can be generated.
