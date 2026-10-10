---
id: TERM-ACCOUNT-INVITATION-STATUS
type: domain-term
title: "Crew account invitation status"
status: draft
defined-in: "BC-CREW"
synonyms: []
uses-terms:
  - "TERM-CREW-MEMBER"
provenance:
  source: ".private/research/help-center/articles/360013323900.md, .private/research/help-center/articles/360014366299.md"
  confidence: "medium"
  recovered-from: "documentation"
---

## Definition

Lifecycle of the invitation a crew member receives to create their login.

States:
- Not sent: the record exists with login disabled, so no invitation email has gone out.
- Pending: the invitation email was sent and the person has not completed their profile.
- Accepted: the person set up their account and can log in.
- Expired: 30 days passed without acceptance.

Transitions:

| From | To | Trigger |
| --- | --- | --- |
| (creation with login allowed) | Pending | crew member added or imported |
| Not sent | Pending | login enabled and invitation sent |
| Pending | Accepted | person completes the profile |
| Pending | Expired | 30 days elapse |
| Pending or Expired | Pending | planner resends the invitation |
| Accepted | login blocked | login permission switched off |

## Distinguish From

Crew invitation status: concerns work offers, not logins.

## Usage

Onboarding crew; preventing premature invitation emails while the installation is being set up.
