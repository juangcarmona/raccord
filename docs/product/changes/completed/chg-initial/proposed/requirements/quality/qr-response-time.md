---
id: QR-RESPONSE-TIME
type: quality-requirement
title: "Responsive billing screens"
status: draft
quality-attribute: "performance"
applies-to:
  - "BC-INVOICING"
  - "BC-PROJECTS"
verification:
  - scenario: "With 10,000 projects and 50,000 issued invoices on a 2-vCPU host, list and detail views respond within 300 ms at the 95th percentile"
uses-terms: []
---

## Requirement

Billing and project views MUST respond within 300 ms at the 95th percentile at the stated data volume.

## Measurement

Load test with generated data before the first release. Not yet measured.
