---
id: TERM-API-TOKEN
type: domain-term
title: "API token"
status: draft
defined-in: "BC-WORKSPACE"
synonyms: []
uses-terms:
  - "TERM-API-TOKEN-VALIDITY"
provenance:
  source: ".private/research/help-center/articles/360013767839.md, .private/research/help-center/articles/30759527977490.md, .private/research/help-center/articles/35485155508370.md, .private/research/help-center/articles/18087057489938.md"
  confidence: high
  recovered-from: documentation
---

## Definition

The secret credential an external application presents to the public interface. A token is created by a user with configuration access, is personal to that user, and grants exactly the access of that user's role at the time of each call. Attributes: owner, creation date, expiry date (ten years after creation) and validity state (TERM-API-TOKEN-VALIDITY). It must be protected like a password.

## Distinguish From

Not a user password.

## Usage

Shown on request in the configuration; regenerated to revoke it; used by custom integrations.
