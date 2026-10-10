---
id: UC-WSP-REVIEW-ROLE-ASSIGNMENTS
type: use-case
title: "Review who holds which role"
status: draft
primary-actor: "ACT-WORKSPACE-ADMINISTRATOR"
supporting-actors: []
bounded-context: "BC-WORKSPACE"
governed-by: []
uses-terms:
  - "TERM-USER-ROLE"
  - "TERM-CREW-MEMBER"
provenance:
  source: ".private/research/help-center/articles/360012611160.md, .private/research/help-center/articles/360015898239.md"
  confidence: high
  recovered-from: documentation
---

## Goal

The administrator knows which users have which role.

## Trigger

Periodic access review or a question about someone's access.

## Preconditions

- The administrator may view crew members.

## Main Flow

1. The administrator shows the role column in the crew member overview.
2. The administrator filters crew members by a role, or opens the role list and follows the count of users per role.
3. The product lists the matching users.

## Alternative Flows

- The administrator reviews the number and names of users holding the Admin role.

## Failure Conditions

- The administrator lacks the right to view crew members: the roles and counts are refused and nothing is shown.
- No crew member holds the filtered role: the list is shown empty.
- The product cannot be reached: the overview fails to load visibly and can be retried.

## Postconditions

- The administrator has an overview of role assignments.
