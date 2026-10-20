---
id: CHG-CONTACT-COMMERCIAL-DEFAULTS-300
type: product-change
title: Accept contact commercial defaults copied into new projects
status: proposed
base-revision: "769edf87e11dec97b23de000741c2f745545256b"
operations:
  add:
    - SB-CONTACT-DEFAULTS-SNAPSHOT-ON-CREATE
  modify:
    - UC-PRJ-SET-CONTACT-COMMERCIAL-DEFAULTS
    - TERM-CONTACT-COMMERCIAL-DEFAULTS
    - BR-PRJ-CLIENT-COMMERCIAL-OVERRIDE
    - FR-PRJ-033
  remove: []
---

## Problem

The model has draft contact commercial defaults, but does not define accepted fields or when they affect a project. Backlog issue #300 requires users to maintain these defaults, copy them into a new project, and leave existing or running projects unchanged when a contact default later changes.

## Intended Product Outcome

An authorized user can maintain commercial defaults on a contact. When the contact is selected as the client of a new project, the product copies the applicable saved values into that project's initial commercial settings. The project retains those values if the contact's defaults later change; users can still edit the project's own settings through the project workflow.

The proposed field set follows the existing draft `TERM-CONTACT-COMMERCIAL-DEFAULTS`: invoice moment, payment term, VAT scheme, optional insurance percentage, and rental and sale discount groups.

## Rationale

Copying the values at project creation preserves the agreement in force when work starts and avoids silent changes to existing projects.

## Affected Product Areas

Contacts commercial defaults, project creation, project commercial settings, and the Contact and Project terms.

## Open Questions

- Confirm that the supported fields are invoice moment, payment term, VAT scheme, optional insurance percentage, and rental and sale discount groups. Recommendation: accept the full draft term field set; “financial settings” in #300's out-of-scope section means global configuration, quotes, and unrelated settings, not these per-contact defaults.
- Confirm that defaults are copied once during project creation and never synchronized later. Recommendation: copy once; allow normal project-level edits.
- Confirm which roles can edit defaults and create projects. Recommendation: billing administrators and operations coordinators may maintain defaults; project creation follows its existing permission.

## Product Acceptance

- The Product Owner confirms the supported default fields.
- The Product Owner confirms snapshot-on-project-creation semantics, including that later contact edits do not alter existing or running projects.
- The Product Owner confirms the authorized roles.

## Out of Scope

Applying defaults to quotes, global finance configuration, retroactive changes, project-level editing behavior beyond initial defaults, and implementation, technical design, release or deployment.
