---
id: TERM-API-TOKEN-VALIDITY
type: domain-term
title: "API token validity"
status: draft
defined-in: "BC-WORKSPACE"
synonyms: []
uses-terms:
  - "TERM-API-TOKEN"
provenance:
  source: ".private/research/help-center/articles/360013767839.md, .private/research/help-center/articles/30759527977490.md"
  confidence: high
  recovered-from: documentation
---

## Definition

The lifecycle of an API token.

| State | Meaning |
| --- | --- |
| Valid | Accepted by the public interface. |
| Revoked | Replaced by regeneration; any call presenting it is refused. |
| Expired | Its validity period has ended; it must be regenerated. |
| Suspended | Public-interface access of the workspace is switched off; calls are refused until access is connected again. |

Transitions: (creation) -> Valid; Valid -> Revoked (regeneration, which creates a new Valid token); Valid -> Expired (end of validity period); Valid -> Suspended (interface access switched off) and back (switched on); Expired -> no transition except replacement by a new token.

## Distinguish From

Not the role permissions of the owner: a valid token can still be refused an operation the owner's role does not allow.

## Usage

Decides whether calls authenticate at all.
