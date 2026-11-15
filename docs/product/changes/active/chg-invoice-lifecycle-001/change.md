---
id: CHG-INVOICE-LIFECYCLE-001
type: product-change
title: Make invoice draft removal a traceable lifecycle state
status: proposed
base-revision: "e6d00da5fbff519eb5695ba08f3f974a3a2b819c"
operations:
  add:
    - LC-INVOICE-STATUS
    - SB-INVOICE-DRAFT-REMOVED
  modify:
    - TERM-INVOICE-STATUS
    - UC-REVISE-INVOICE-DRAFT
    - UC-FIN-EDIT-INVOICE-DRAFT
    - BR-FIN-DRAFT-PREVIEW-MARK
    - FR-FIN-PREVIEW-DRAFTS
    - SB-FIRST-INVOICE-NUMBER
    - SB-FIN-PREVIEW-NOT-EXPORTED
  remove: []
unaffected:
  - id: "FR-INVOICE-002"
    cause: "UC-REVISE-INVOICE-DRAFT"
    relationship: "derived-from"
    digest: "sha256:a90401f360e2dc01449955177f428391202437b80c5028a37b7b0e7ffe937742"
    cause-digest: "sha256:1d6fb89ab4b265596c68d13a34350cfd31d025b25c9d6b34f54843f1ca1e4a8a"
    reason: "Its line-release requirement remains true; the removed draft is additionally retained."
  - id: "UC-FIN-RUN-ACCOUNTING-EXPORT"
    cause: "BR-FIN-DRAFT-PREVIEW-MARK"
    relationship: "governed-by"
    digest: "sha256:e631ae761f0dc4f62753faf93fb341434a422c64ad8ea939f13b11cde1744328"
    cause-digest: "sha256:52840a3757c5cb1831bc1c2f3fa35b438e2f484b02b506dd1df6861a3ab7f010"
    reason: "The export still includes finalized invoices only; removed drafts remain excluded."
  - id: "UC-ISSUE-INVOICE"
    cause: "BR-FIN-DRAFT-PREVIEW-MARK"
    relationship: "governed-by"
    digest: "sha256:342ce2d34a022b8a621fd91efff10751c2dd841e6f42b28c5530dbc5ead7eb63"
    cause-digest: "sha256:52840a3757c5cb1831bc1c2f3fa35b438e2f484b02b506dd1df6861a3ab7f010"
    reason: "The issue flow still regenerates the preview document without its mark."
  - id: "JRN-BILL-FINISHED-PROJECT"
    cause: "UC-REVISE-INVOICE-DRAFT"
    relationship: "steps[].use-case"
    digest: "sha256:b9530a2b0a9aac0f9cb3a5a9608494d08be1fc1190f395887fc7bb7fb30df28e"
    cause-digest: "sha256:1d6fb89ab4b265596c68d13a34350cfd31d025b25c9d6b34f54843f1ca1e4a8a"
    reason: "The billing journey still releases work when a draft is discarded."
  - id: "FR-INVOICE-005"
    cause: "SB-FIRST-INVOICE-NUMBER"
    relationship: "verification[].scenario-ref"
    digest: "sha256:bef03e64c65036146ad6554121f0336f0c95d5c7e8aef15a0536b845b6781e76"
    cause-digest: "sha256:99641b094f7dd5a614326773f716975a6e5176e491dfda9cf94f19ff5ef984ea"
    reason: "The example still verifies the same first number and sequence."
---

## Problem

TERM-INVOICE-STATUS describes discarding a draft as a transition to Removed, but lists only Draft and Issued as states. UC-REVISE-INVOICE-DRAFT does not say whether a discarded draft remains available. The examples do not cover the discard transition.

## Intended Product Outcome

An invoice draft has three explicit states: Draft, Issued and Removed. Issued and Removed are terminal. Discarding a draft releases its billable lines, records the action and retains the draft as a queryable Removed record. Preview rules apply to open drafts. Structured behaviours make both issue and discard transitions observable.

## Rationale

The billing administrator needs to distinguish a discarded draft from a draft that never existed. An explicit lifecycle connects the existing use cases and rules to observable examples without changing invoice numbering or the separate payment status. It also narrows existing "not finalized" language to open drafts, so a removed draft cannot be edited.

## Affected Product Areas

Invoice drafts, invoice status, issuing, discarding, billing-line availability and audit history.

## Open Questions

None.

## Product Acceptance

The accepted definition names Draft as initial, Issued and Removed as terminal, and covers both transitions with structured behaviours. Discarding releases the lines and leaves the removed draft queryable with its audit trail. Issued invoices remain immutable; neither terminal state can be revised, discarded or issued again.

## Out of Scope

Retention duration, query interface design, data migration, implementation and deployment.
