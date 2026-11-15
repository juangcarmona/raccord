---
id: LC-INVOICE-STATUS
type: domain-lifecycle
title: "Invoice draft lifecycle"
status: draft
subject: "TERM-INVOICE-STATUS"
states:
  - id: DRAFT
    title: "Draft"
    initial: true
    terminal: false
  - id: ISSUED
    title: "Issued"
    initial: false
    terminal: true
  - id: REMOVED
    title: "Removed"
    initial: false
    terminal: true
transitions:
  - id: ISSUE
    title: "Issue invoice"
    from:
      - DRAFT
    to: ISSUED
    trigger: "The billing administrator issues a valid draft"
    initiated-by:
      - "ACT-BILLING-ADMINISTRATOR"
    governed-by:
      - "BR-ISSUE-READINESS"
      - "BR-GAPLESS-NUMBERING"
      - "BR-ISSUED-INVOICE-IMMUTABLE"
      - "BR-AUDITED-ACTIONS"
    realized-by:
      - "UC-ISSUE-INVOICE"
  - id: DISCARD
    title: "Discard draft"
    from:
      - DRAFT
    to: REMOVED
    trigger: "The billing administrator discards an open draft"
    initiated-by:
      - "ACT-BILLING-ADMINISTRATOR"
    governed-by:
      - "BR-SINGLE-BILLING"
      - "BR-ISSUED-INVOICE-IMMUTABLE"
      - "BR-AUDITED-ACTIONS"
    realized-by:
      - "UC-REVISE-INVOICE-DRAFT"
uses-terms:
  - "TERM-INVOICE-DRAFT"
  - "TERM-ISSUED-INVOICE"
---

## Purpose

Make issue and discard outcomes explicit for each invoice draft.

## Invariants

Only a Draft can be issued or discarded. Issued and Removed are terminal. A discarded draft remains queryable, while its billable lines become unbilled again. Payment status is separate.

## State Semantics

Draft is open, unnumbered and editable. Issued identifies a numbered, immutable invoice. Removed identifies a discarded draft retained as a queryable record with its audit trail; it cannot be edited or issued.

## Transition Semantics

ISSUE checks readiness, assigns the next invoice number and creates the immutable invoice and its audit and compliance records together. DISCARD releases all billable lines, records the discard and changes the draft to Removed.

## Boundaries

Corrections to issued invoices use credit notes. This lifecycle does not govern payment status or set a retention duration.
