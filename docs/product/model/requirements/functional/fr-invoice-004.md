---
id: FR-INVOICE-004
type: functional-requirement
title: "Report every reason a draft cannot be issued"
status: draft
derived-from:
  - "BR-ISSUE-READINESS"
  - "UC-ISSUE-INVOICE"
verification:
  - scenario-ref: "SB-MISSING-RECIPIENT-TAX-ID"
uses-terms:
  - "TERM-VALIDATION-ISSUE"
---

## Requirement

The product MUST show on each open draft every unmet issue-readiness condition for today's date, and MUST refuse to issue a draft with any unmet condition without consuming a number.

## Rationale

The administrator learns what is missing before trying to issue.
