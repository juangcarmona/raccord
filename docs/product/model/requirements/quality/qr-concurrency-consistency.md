---
id: QR-CONCURRENCY-CONSISTENCY
type: quality-requirement
title: "Concurrent use never corrupts billing"
status: draft
quality-attribute: "consistency"
applies-to:
  - "UC-ISSUE-INVOICE"
  - "UC-CREATE-INVOICE-DRAFT"
verification:
  - scenario-ref: "SB-CONCURRENT-ISSUING-GAPLESS"
  - scenario-ref: "SB-LINE-IN-TWO-DRAFTS"
uses-terms: []
---

## Requirement

Concurrent requests MUST never duplicate numbers, skip numbers or bill a line twice; losing requests MUST be told to retry.

## Measurement

Parallel requests in automated tests; zero violations.
