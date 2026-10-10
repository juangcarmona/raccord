---
id: TERM-PROJECT-FUNCTION
type: domain-term
title: "Function"
status: draft
defined-in: "BC-CREW"
synonyms:
  - "crew function"
  - "role"
uses-terms:
  - "TERM-FUNCTION-KIND"
  - "TERM-FUNCTION-GROUP"
  - "TERM-FUNCTION-TIMING-MODE"
  - "TERM-FUNCTION-USAGE-PERIOD"
  - "TERM-CREW-PLANNING-PERIOD"
  - "TERM-CREW-RATE"
  - "TERM-SKILL-TAG"
  - "TERM-TIME-SCHEDULE"
  - "TERM-PROJECT"
  - "TERM-VAT-CLASS"
  - "TERM-LEDGER-ACCOUNT"
provenance:
  source: ".private/research/help-center/articles/14232209807506.md, .private/research/help-center/articles/360013223940.md, .private/research/help-center/articles/14319493249170.md"
  confidence: "high"
  recovered-from: "documentation"
---

## Definition

A role or job that must be performed on a project, to be filled by crew members or vehicles. Attributes: kind (crew or transport), external name (on quotes, contracts, invoices), internal name (on call sheets and planning), required quantity, timing mode, function usage period, crew planning period, break length (crew only), price and cost (hourly or per kilometre, fixed, both, or a chosen crew rate), VAT class, ledger account, include-in-price flag, distance (transport only), travel time before and after, skill tags, remarks for customer, planner and crew, plannable flag and the function group it belongs to. It is created from a default function or as a one-off.

## Distinguish From

Shift: internal work outside projects. Crew assignment: the planning of one resource on a function. Function group: a container of functions.

## Usage

Quoting crew and transport, planning, costs, time registration.
