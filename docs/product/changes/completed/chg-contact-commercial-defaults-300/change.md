---
id: CHG-CONTACT-COMMERCIAL-DEFAULTS-300
type: product-change
title: Accept contact commercial defaults copied into new projects
status: applied
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

None. The accepted field set is invoice moment, payment term, VAT scheme, optional insurance percentage, and rental and sale discount groups. These per-contact defaults are distinct from global financial configuration and quotes.

## Product Acceptance

- Billing administrators and operations coordinators may maintain contact commercial defaults; project creation follows its existing permission.
- A newly created project copies the selected contact's current defaults once into its initial commercial settings. Later edits to contact defaults do not alter existing or running projects.
- Project users can continue editing the project's own commercial settings through the existing project workflow.

## Out of Scope

Applying defaults to quotes, global finance configuration, retroactive changes, project-level editing behavior beyond initial defaults, and implementation, technical design, release or deployment.
