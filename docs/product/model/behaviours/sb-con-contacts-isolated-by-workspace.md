---
id: SB-CON-CONTACTS-ISOLATED-BY-WORKSPACE
type: structured-behaviour
title: Contact organization remains inside its installation
status: active
illustrates:
  - BR-CON-CONTACT-TAG-SCOPE
  - BR-CON-FOLDER-HIERARCHY
given:
  - Two organizations use separate installations and databases
  - Both installations contain the same contact tag and folder names
  - The actor is authenticated to the first installation
when: The actor filters or organizes contacts in the first installation
then:
  - Only contacts, tags and folders in the first installation are read or changed
  - The second installation is neither queried nor changed
---

## Intent

Contact organization remains inside its installation.

## Boundaries

ADR-0012 supplies database-per-deployment isolation; no tenant discriminator or client-selected workspace ID is added.
