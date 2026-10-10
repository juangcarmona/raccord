---
id: FR-CRW-INVITATION-TEMPLATES
type: functional-requirement
title: "Configurable invitation templates"
status: draft
derived-from:
  - "UC-CRW-CONFIGURE-INVITATION-TEMPLATES"
  - "BR-CRW-INVITATION-DISCLOSURE"
  - "BR-CRW-INVITATION-TEXT-RESET"
verification:
  - scenario: "With client name hidden by default, an invitation sent without changing the setting omits the client name"
  - scenario: "Resetting all invitation texts restores the default message of every type"
uses-terms:
  - "TERM-INVITATION-TEMPLATE"
provenance:
  source: ".private/research/help-center/articles/23925911973010.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Requirement

The product MUST let administrators configure signature, subject, salutations, per-type messages and default disclosed information for invitations, overridable per send, sent through the company mail account.

## Rationale

Wording and disclosure control.
