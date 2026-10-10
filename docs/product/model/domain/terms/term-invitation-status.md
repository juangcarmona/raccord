---
id: TERM-INVITATION-STATUS
type: domain-term
title: "Crew invitation status"
status: draft
defined-in: "BC-CREW"
synonyms: []
uses-terms:
  - "TERM-INVITATION"
  - "TERM-STAFFING-STATUS"
provenance:
  source: ".private/research/help-center/articles/17080436056978.md, .private/research/help-center/articles/360014444159.md, .private/research/help-center/articles/360014625560.md, .private/research/help-center/articles/360013323900.md"
  confidence: "medium"
  recovered-from: "documentation"
---

## Definition

Lifecycle of a crew invitation.

States:
- Unsent: prepared (for example marked on the planning timeline) and counted as waiting to be sent.
- Open: sent and awaiting a response.
- Accepted: answered yes, by the crew member or by the planner on their behalf.
- Declined: answered no.
- Expired: closed without effect, notably when a final-planning function reaches its quantity.
- Withdrawn: cancelled by the planner.

Transitions:

| From | To | Trigger |
| --- | --- | --- |
| Unsent | Open | planner sends invitations |
| Open | Accepted | yes response |
| Open | Declined | no response |
| Open | Expired | function becomes fully planned (final-planning type) |
| Open | Withdrawn | planner withdraws |

Accepted and declined together are the answered invitations.

## Distinguish From

Account invitation status: logins.

## Usage

Planning views filter on open, answered and expired invitations.
