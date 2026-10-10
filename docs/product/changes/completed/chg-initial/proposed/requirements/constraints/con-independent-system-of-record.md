---
id: CON-INDEPENDENT-SYSTEM-OF-RECORD
type: constraint
title: "Raccord is an independent system of record"
status: draft
uses-terms: []
---

## Constraint

Raccord owns its data, workflows and terminology and works without any external operations platform. Integrations with accounting software, e-invoice networks, payment providers, calendars, automation platforms and other organizations are product capabilities behind explicit, permission-controlled interfaces; none of them is the system of record for Raccord's data, and Raccord never depends on, wraps or synchronizes with another operations platform.

## Rationale

Self-imposed product direction.

## Consequences

Connectors exchange data through documented interfaces; imports of data from other systems are optional and isolated.
