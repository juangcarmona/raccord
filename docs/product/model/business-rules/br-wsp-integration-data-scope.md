---
id: BR-WSP-INTEGRATION-DATA-SCOPE
type: business-rule
title: "Each integration shares only its documented data"
status: draft
applies-to:
  - "BC-WORKSPACE"
uses-terms:
  - "TERM-WORKSPACE"
provenance:
  source: ".private/research/help-center/articles/27616522173458.md"
  confidence: medium
  recovered-from: documentation
---

## Rule

Enabling an integration shares company data with the third party only within that integration's documented scope: tax-authority reporting sends the data of finalized invoices required by the authority; the company registry lookup sends only the search text typed by the user; calendar feeds publish only the streams the user chose, within the user's permissions; the public interface exposes what the custom integration requests within the token owner's permissions.

## Rationale

The company must be able to tell what leaves the product when it enables an integration.

## Examples

- Enabling tax-authority reporting never sends crew data.

## Exceptions

None.
