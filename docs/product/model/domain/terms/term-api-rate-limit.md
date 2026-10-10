---
id: TERM-API-RATE-LIMIT
type: domain-term
title: "API rate limit"
status: draft
defined-in: "BC-WORKSPACE"
synonyms: []
uses-terms: []
provenance:
  source: ".private/research/help-center/articles/26707946497042.md, .private/research/help-center/articles/360015784860.md"
  confidence: medium
  recovered-from: documentation
---

## Definition

The maximum request rate the public interface accepts from the installation; requests beyond it are refused with a forbidden-type response even where an endpoint's documentation does not list that outcome.

## Distinguish From

Not a permission refusal, although both are signalled as refusals.

## Usage

Integrations must slow down and retry when refused.
