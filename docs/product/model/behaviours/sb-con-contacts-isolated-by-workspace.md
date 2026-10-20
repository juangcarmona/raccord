---
id: SB-CON-CONTACTS-ISOLATED-BY-WORKSPACE
type: structured-behaviour
title: "Contact organization filters do not cross workspaces"
status: active
illustrates:
    - "BR-CON-CONTACT-TAG-SCOPE"
    - "BR-CON-FOLDER-HIERARCHY"
given:
    - "A user has access to Workspace A but not Workspace B"
    - "Both workspaces contain contacts with the same tag and folder names"
when: "The user filters contacts in Workspace A"
then:
    - "Only contacts from Workspace A can appear"
uses-terms:
    - "TERM-CONTACT"
    - "TERM-CONTACT-TAG"
    - "TERM-CONTACT-FOLDER"
---

## Intent

Filtering and organization preserve workspace tenant isolation.

## Boundaries

Tag and folder names may be the same in separate workspaces without exposing or combining their contacts.
