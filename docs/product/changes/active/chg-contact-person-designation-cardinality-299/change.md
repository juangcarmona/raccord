---
id: CHG-CONTACT-PERSON-DESIGNATION-CARDINALITY-299
type: product-change
title: Define one administrative contact person per contact
status: proposed
base-revision: "9b9bcd33e28594142b3a839a700caa0140da3fe0"
operations:
  add: []
  modify:
    - BR-CON-CONTACT-PERSON-DESIGNATIONS
    - FR-CON-002
  remove: []
---

## Problem

The accepted Product Definition describes the default designation cardinality, but does not define the maximum number of administrative contact persons. The approved delivery plan for #299 includes the one-administrative-person-per-contact invariant, so the plan currently states a product rule that the canonical model does not.

## Intended Product Outcome

Each contact has zero or one default contact person and zero or one administrative contact person. A contact person remains associated with the contact under which it was created. The administrative designation does not select a project contact or determine invoice delivery.

## Rationale

The product owner approved this cardinality as part of the #299 Product Change and its delivery plan. Expressing the rule in the canonical model keeps implementation, acceptance tests and future project workflows aligned with the accepted product definition.

## Affected Product Areas

Contacts, contact-person designations and their functional requirement.

## Open Questions

None. This change defines cardinality only. The behavior when assigning an administrative designation to a contact that already has one remains a delivery detail within the approved #299 plan, which allows rejection or atomic replacement.

## Product Acceptance

- The business rule states that a contact has at most one administrative contact person, alongside its existing single-default rule.
- The functional requirement states the same administrative cardinality.
- No project assignment, invoice recipient, communication or additional contact-person behavior is introduced.

## Out of Scope

Changing contact-person creation, editing or deletion scope; selecting contact persons on projects; choosing invoice recipients; sending communications; technical design, implementation, release or deployment.
