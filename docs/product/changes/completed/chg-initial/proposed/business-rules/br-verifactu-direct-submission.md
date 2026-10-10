---
id: BR-VERIFACTU-DIRECT-SUBMISSION
type: business-rule
title: "Billing records are submitted directly with the company certificate"
status: draft
applies-to:
  - "UC-REPORT-BILLING-RECORDS"
uses-terms:
  - "TERM-COMPANY-CERTIFICATE"
  - "TERM-COMPLIANCE-RECORD"
---

## Rule

Each installation submits its billing records directly to the tax authority, authenticated with the company's own electronic certificate; no intermediary is involved. Without a valid certificate, records stay pending and the administrator is warned before the certificate expires and when submissions fail.

## Rationale

Decided by the product owner: one company per installation, no third-party dependency.

## Examples

A certificate expiring in 20 days is flagged; after expiry, records queue until a new certificate is installed.

## Exceptions

None. Each installing company acts as the producer of the Raccord installation it runs for its own use, signs the responsible declaration and uses its own electronic certificate. Raccord provides the declaration template and release information; modified forks or customisations require their own declaration. This is Raccord's product policy, subject to legal verification before any compliance claim.
