---
id: BR-INT-API-COVERAGE-BOUNDARY
type: business-rule
title: "Only offered operations are possible"
status: draft
applies-to:
  - "UC-INT-WRITE-RECORDS-THROUGH-API"
  - "BC-WORKSPACE"
uses-terms:
  - "TERM-API-RESOURCE"
provenance:
  source: ".private/research/help-center/articles/35485155508370.md, .private/research/help-center/articles/18087057489938.md, .private/research/help-center/articles/26707946497042.md"
  confidence: high
  recovered-from: documentation
---

## Rule

The public interface, and everything built on it, can perform only the operations its version offers for each record kind.

## Rationale

The interface is a deliberate contract; capabilities are added explicitly per version.

## Examples

Contacts can be created; quotations and invoices cannot be created; invoices can only be read.

## Exceptions

None.
