---
id: SB-CON-CONTACT-PERSON-PRESERVES-PARTY-SNAPSHOT
type: structured-behaviour
title: Maintain contact people without rewriting project or invoice history
status: active
illustrates:
  - UC-CON-MANAGE-CONTACT-PERSONS
given:
  - A contact has a project association and an issued invoice with a frozen party snapshot
when: An authorized coordinator creates or edits a contact person or reassigns a designation
then:
  - The contact person remains owned by the same parent contact
  - Existing project associations are unchanged
  - The issued invoice party snapshot is unchanged
---

## Intent

Maintain contact people without rewriting project or invoice history.

## Boundaries

Project contact-person selection and invoice delivery are separate workflows.
