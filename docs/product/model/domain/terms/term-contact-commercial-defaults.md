---
id: TERM-CONTACT-COMMERCIAL-DEFAULTS
type: domain-term
title: "Contact commercial defaults"
status: active
defined-in: "BC-CONTACTS"
synonyms: []
uses-terms:
  - "TERM-CONTACT"
  - "TERM-CUSTOMER"
  - "TERM-PAYMENT-TERM"
  - "TERM-VAT-SCHEME"
  - "TERM-DISCOUNT-GROUP"
  - "TERM-INVOICE-MOMENT"
provenance:
  source: ".private/research/help-center/articles/360014254000.md, .private/research/help-center/articles/360013628759.md"
  confidence: high
  recovered-from: documentation
---

## Definition

Commercial settings stored on a contact and copied to a new project when that contact is selected as its client: invoice moment, payment term, VAT scheme, optional contact-specific insurance percentage, and rental and sale discount groups. Each setting is optional.

## Distinguish From

Project type defaults initialize a project according to its kind. Contact defaults, when present, take precedence during initial project creation.

## Usage

Honouring agreed terms when a new project is created. Later contact changes do not alter existing projects.
