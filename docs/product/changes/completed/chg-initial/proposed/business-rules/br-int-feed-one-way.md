---
id: BR-INT-FEED-ONE-WAY
type: business-rule
title: "Subscription feeds are read-only"
status: draft
applies-to:
  - "UC-INT-SUBSCRIBE-TO-CALENDAR-FEED"
uses-terms:
  - "TERM-CALENDAR-SUBSCRIPTION-FEED"
provenance:
  source: ".private/research/help-center/articles/360014047779.md"
  confidence: high
  recovered-from: documentation
---

## Rule

Items flow from the workspace to subscription feeds only; only two-way synchronisation brings device items back.

## Rationale

The feed format is a publication, not a sync protocol.

## Examples

An event added in a subscribed web calendar stays there.

## Exceptions

None.
