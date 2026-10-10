---
id: UC-WHS-PRINT-PACKING-SLIP
type: use-case
title: "Print a packing slip"
status: draft
primary-actor: "ACT-WAREHOUSE-TECHNICIAN"
supporting-actors:
  - "ACT-OPERATIONS-COORDINATOR"
bounded-context: "BC-WAREHOUSE"
governed-by: []
uses-terms:
  - "TERM-PACKING-SLIP"
  - "TERM-PACKING-LIST"
provenance:
  source: ".private/research/help-center/articles/38542583895186.md, .private/research/help-center/articles/14074151954834.md, .private/research/help-center/articles/15125679219858.md"
  confidence: high
  recovered-from: documentation
---

## Goal

The crew has a paper or signed document of what to pack.

## Trigger

Packing starts.

## Preconditions

A packing-slip template exists (a default is provided).

## Main Flow

1. The user chooses one list or the whole project (grouped by times), a template, letterhead and optional text.
2. The product generates the slip; on a device it can be previewed or signed.

## Alternative Flows

None.

## Failure Conditions

- Generating without a packing-slip template is refused with the reason and nothing is produced.
- A list with no lines produces an empty slip.
- If rendering fails, no slip is produced and generation can be retried.
- A user without access to the list is refused and nothing changes.

## Postconditions

A slip document exists.
