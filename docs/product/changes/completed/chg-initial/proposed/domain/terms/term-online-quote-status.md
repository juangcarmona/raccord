---
id: TERM-ONLINE-QUOTE-STATUS
type: domain-term
title: "Online quote status"
status: draft
defined-in: "BC-COMMERCIAL"
synonyms: []
uses-terms:
  - "TERM-ONLINE-QUOTE"
provenance:
  source: ".private/research/help-center/articles/29481848461458.md"
  confidence: medium
  recovered-from: documentation
---

## Definition

Lifecycle of the publication of a quote.

| State | Meaning |
| --- | --- |
| Not published | The quote exists only as a document. |
| Published | The page is online and shows the latest synchronised version. |
| Published, newer version pending | A new version exists that the customer cannot see yet. |
| Offline | The page was taken offline. |

Transitions: not published -> published (publish); published -> published (overwrite or new version, immediately visible); published -> newer version pending (new version without synchronising); newer version pending -> published (synchronise); published or pending -> offline (take offline); offline -> published (publish latest version again).

## Distinguish From

Quote status (validity and signature).

## Usage

Shown with the quote in the project.
