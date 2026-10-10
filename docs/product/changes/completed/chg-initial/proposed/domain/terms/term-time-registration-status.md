---
id: TERM-TIME-REGISTRATION-STATUS
type: domain-term
title: "Time registration status"
status: draft
defined-in: "BC-CREW"
synonyms: []
uses-terms:
  - "TERM-TIME-REGISTRATION"
  - "TERM-HOUR-TYPE"
provenance:
  source: ".private/research/help-center/articles/27336532785298.md, .private/research/help-center/articles/360015498439.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Definition

Lifecycle of a time registration whose hour type requires approval.

States: pending (submitted, editable, not counted in worked totals) and approved (validated, read-only, counted).

Transitions: created -> pending; pending -> approved (individually, in bulk or while reviewing); approved -> pending (reverted to correct a mistake). Registrations of hour types that do not require approval are recorded directly without this approval step.

## Distinguish From

Reconciliation status: compares registered with planned time.

## Usage

Gatekeeping payroll exports.
