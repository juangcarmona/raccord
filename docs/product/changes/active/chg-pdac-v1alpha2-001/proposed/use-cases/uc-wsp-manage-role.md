---
id: UC-WSP-MANAGE-ROLE
type: use-case
title: "Create or edit a user role"
status: draft
primary-actor: "ACT-WORKSPACE-ADMINISTRATOR"
supporting-actors: []
bounded-context: "BC-WORKSPACE"
governed-by:
  - "BR-WSP-FULL-ROLE-STILL-RESTRICTABLE"
  - "BR-WSP-HIDDEN-MODULES"
  - "BR-WSP-ROLE-EDITOR-SELF-ELEVATION"
  - "BR-WSP-PROJECT-ACCESS-IMPLICATIONS"
  - "BR-WSP-WAREHOUSE-ALL-PROJECTS"
  - "BR-WSP-DELETE-NEEDS-EDIT"
  - "BR-WSP-INVOICE-CREATE-IMPLIES-ACCESS"
  - "BR-WSP-EXTRA-ITEMS-OVERRIDE"
  - "BR-WSP-CREW-RATES-WITH-PROJECT-ACCESS"
  - "BR-WSP-LAST-SEEN-REQUIRES-LOGIN-RIGHTS"
  - "BR-WSP-DASHBOARD-RESPECTS-PERMISSIONS"
  - "BR-WSP-TAGS-WITHIN-ACCESSIBLE-MODULES"
  - "BR-WSP-FILES-ON-READONLY-RECORDS"
  - "BR-WSP-FILE-DOWNLOAD-LEVELS"
  - "BR-WSP-TEMPLATE-AND-COLUMN-SET-SCOPE"
  - "BR-WSP-CONFIDENTIAL-FIELDS"
  - "BR-WSP-LOCATION-CONTACT-WINDOW"
  - "BR-WSP-OWN-SENT-EMAILS"
  - "BR-WSP-TASK-RIGHTS-IN-SIDEBAR"
  - "BR-WSP-PLANNED-EQUIPMENT-APP"
  - "BR-FIN-FINANCIAL-VISIBILITY"
  - "BR-CRW-TIME-REGISTRATION-RIGHTS"
  - "BR-CRW-AVAILABILITY-RIGHTS"
  - "BR-CRW-DOCUMENT-ACCESS-PERMISSION"
  - "BR-PRJ-NOTE-WITHOUT-EDIT-RIGHTS"
uses-terms:
  - "TERM-USER-ROLE"
  - "TERM-ROLE-PERMISSION"
  - "TERM-ACCESS-LEVEL"
  - "TERM-OWN-RECORD"
provenance:
  source: ".private/research/help-center/articles/360012611160.md, .private/research/help-center/articles/22185539659666.md"
  confidence: high
  recovered-from: documentation
---

## Goal

A role exists whose permissions match what a group of users needs.

## Trigger

A new group of users (for example warehouse staff) needs different access, or an existing role must change.

## Preconditions

- The administrator's role allows creating and editing roles.

## Main Flow

1. The administrator adds a role and names it.
2. For each module the administrator grants or withholds access and sets each permission's level and scope.
3. The administrator saves; the product applies the role to all its users immediately.

## Alternative Flows

- The administrator starts from one of the four predefined roles (Admin, Office, Warehouse, Crew) and adapts it.

## Failure Conditions

- The user's role does not allow editing roles: the role list is not offered and nothing changes.

## Postconditions

- Users holding the role see only the granted modules and can perform only the granted actions.
