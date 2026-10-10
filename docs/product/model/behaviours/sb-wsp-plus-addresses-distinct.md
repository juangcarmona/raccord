---
id: SB-WSP-PLUS-ADDRESSES-DISTINCT
type: structured-behaviour
title: "Plus-addressed variants count as different addresses"
status: draft
illustrates:
  - "BR-WSP-ONE-PROFILE-PER-EMAIL"
given:
  - "Warehouse scanner users share one mailbox"
when: "Three crew members each verify a plus-addressed variant of the shared mailbox"
then:
  - "Each variant is accepted as a unique address"
  - "Three separate profiles exist"
  - "All verification emails arrive in the shared mailbox if the mail provider supports plus addressing"
uses-terms:
  - "TERM-PERSONAL-PROFILE"
provenance:
  source: ".private/research/help-center/articles/36290993235730.md"
  confidence: medium
  recovered-from: documentation
---

## Intent

Documents the accepted workaround for shared mailboxes.

## Boundaries

Does not assert that every mail server delivers plus-addressed mail.
