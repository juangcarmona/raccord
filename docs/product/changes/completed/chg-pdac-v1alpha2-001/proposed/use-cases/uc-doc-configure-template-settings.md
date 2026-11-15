---
id: "UC-DOC-CONFIGURE-TEMPLATE-SETTINGS"
type: use-case
title: "Configure the settings of a document type's template"
status: draft
primary-actor: "ACT-WORKSPACE-ADMINISTRATOR"
supporting-actors: []
bounded-context: "BC-DOCUMENTS"
governed-by:
  - "BR-DOC-TYPE-SCOPES-TEMPLATE"
  - "BR-DOC-COUNTRY-FORMATTING"
uses-terms:
  - "TERM-DOCUMENT-TEMPLATE"
  - "TERM-TEMPLATE-LOCALE"
  - "TERM-LETTERHEAD"
provenance:
  source: ".private/research/help-center/articles/360013503420.md, .private/research/help-center/articles/360014232559.md, .private/research/help-center/articles/17707866444178.md, .private/research/help-center/articles/360013979460.md"
  confidence: high
  recovered-from: documentation
---

## Goal

The built-in template of a document type has the right language, country, e-mail subject, file name, header, footer, signing options and default letterhead.

## Trigger

The company sets up its documents or changes how a document type looks or reads.

## Preconditions

The document type has its built-in template.

## Main Flow

1. The administrator opens the settings of the document type's template.
2. The administrator sets the language and country.
3. The administrator sets the e-mail subject, file name, header text and footer text.
4. The administrator sets the default letterhead and the digital signing options.
5. The administrator saves.

## Alternative Flows

Each document type has its own template; the type decides where the template is offered and what it contains.

## Failure Conditions

- Saving with a missing or invalid required setting is refused with the reason and nothing is saved.
- An actor without permission to change company settings is refused and nothing changes.

## Postconditions

Documents of that type generated afterwards follow the new settings.
