---
id: BR-PRJ-CLIENT-COMMERCIAL-OVERRIDE
type: business-rule
title: "Client commercial defaults override project values"
status: draft
applies-to:
  - "UC-PRJ-SET-CONTACT-COMMERCIAL-DEFAULTS"
  - "UC-PRJ-EDIT-PROJECT-DETAILS"
uses-terms:
  - "TERM-CONTACT-COMMERCIAL-DEFAULTS"
  - "TERM-PROJECT-TYPE"
provenance:
  source: ".private/research/help-center/articles/360014254000.md"
  confidence: high
  recovered-from: documentation
---

## Rule

When a contact with its own invoice moment, payment term or VAT scheme is the client of a project, each of these values overrides the project's own (including the project type default).

## Rationale

Agreements with a customer prevail over generic defaults.

## Examples

See SB-PRJ-CLIENT-PAYMENT-TERM-OVERRIDES.

## Exceptions

Values not set on the contact fall back to the project.
