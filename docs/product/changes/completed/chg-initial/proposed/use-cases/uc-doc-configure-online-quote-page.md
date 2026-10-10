---
id: "UC-DOC-CONFIGURE-ONLINE-QUOTE-PAGE"
type: use-case
title: "Configure the online quote page defaults"
status: draft
primary-actor: "ACT-WORKSPACE-ADMINISTRATOR"
supporting-actors: []
bounded-context: "BC-DOCUMENTS"
governed-by:
  - "BR-DOC-ONLINE-QUOTE-FORCED-PASSWORD"
  - "BR-FIN-ONLINE-QUOTE-ACCESS"
uses-terms:
  - "TERM-ONLINE-QUOTE"
  - "TERM-TERMS-AND-CONDITIONS"
  - "TERM-TEXT-TEMPLATE"
provenance:
  source: ".private/research/help-center/articles/29667214920082.md"
  confidence: high
  recovered-from: documentation
---

## Goal

Online quotes present the company consistently.

## Trigger

The company starts publishing quotes online.

## Preconditions

None.

## Main Flow

1. The administrator writes the default introduction text.
2. The administrator sets the password-page background image, whether a password is always required, and the terms and conditions shown.
3. The administrator saves.
4. The administrator creates an e-mail text template for sending the quote link and password.

## Alternative Flows

None.

## Failure Conditions

- Saving with missing or invalid required settings, such as an unusable background image, is refused with the reason and nothing is saved.
- An actor without permission to change company settings is refused and nothing changes.
- While password protection is forced, a quote visitor without the correct password is not shown the content (BR-DOC-ONLINE-QUOTE-FORCED-PASSWORD, BR-FIN-ONLINE-QUOTE-ACCESS).
- If another user changed the same settings in the meantime, the save is refused and the administrator must reload and retry.

## Postconditions

Newly published online quotes use these defaults.
