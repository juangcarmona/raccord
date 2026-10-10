---
id: TERM-PUBLIC-API
type: domain-term
title: "Public interface"
status: draft
defined-in: "BC-WORKSPACE"
synonyms:
  - "Open API"
uses-terms:
  - "TERM-API-RESOURCE"
  - "TERM-API-TOKEN"
  - "TERM-API-VERSION"
provenance:
  source: ".private/research/help-center/articles/360013767839.md, .private/research/help-center/articles/26707946497042.md, .private/research/help-center/articles/35485155508370.md, .private/research/help-center/articles/18087057489938.md"
  confidence: high
  recovered-from: documentation
---

## Definition

The documented programmatic interface through which external applications read and change workspace data. It exposes a defined set of record kinds (TERM-API-RESOURCE) with, per kind, the operations allowed (list, read one, create, update, delete), related child collections, and query features: paging, filtering, field selection, expansion of related records. It is versioned, described by a machine-readable specification, authenticated with an API token and limited in request rate.

## Distinguish From

Not the user interface: several capabilities of the product (for example creating quotations or invoices) are not offered by the public interface. Not a push mechanism: external applications ask for data, the product does not send change events.

## Usage

Used by custom integrations written or commissioned by the company.
