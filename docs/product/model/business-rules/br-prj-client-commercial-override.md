---
id: BR-PRJ-CLIENT-COMMERCIAL-OVERRIDE
type: business-rule
title: "Contact commercial defaults initialize new projects"
status: active
applies-to:
  - "UC-PRJ-SET-CONTACT-COMMERCIAL-DEFAULTS"
  - "UC-PRJ-EDIT-PROJECT-DETAILS"
uses-terms:
  - "TERM-CONTACT-COMMERCIAL-DEFAULTS"
  - "TERM-PROJECT-TYPE"
provenance:
  source: ".private/research/help-center/articles/360014254000.md"
  confidence: high
  recovered-from: documentation
---

## Rule

When a contact with saved commercial defaults is selected as the client during new project creation, each set contact value initializes the corresponding project value and takes precedence over the project type default. A value not set on the contact follows the existing project and project-type defaults. After creation, the project keeps its own values; later contact default changes do not propagate automatically.

## Rationale

Agreed customer terms apply when a project begins without silently changing existing or running projects later.

## Examples

If a contact's payment term is 30 days when a project is created, that project keeps 30 days after the contact is later changed to 45 days. A subsequently created project starts with 45 days.

## Exceptions

None.
