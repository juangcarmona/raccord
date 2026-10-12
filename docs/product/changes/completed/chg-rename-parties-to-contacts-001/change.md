---
id: CHG-RENAME-PARTIES-TO-CONTACTS-001
type: product-change
title: 'Rename the Parties bounded context to Contacts'
status: applied
base-revision: '2c000ec'
operations:
  add:
    - BC-CONTACTS
  modify:
    - TERM-ADMINISTRATIVE-CONTACT-PERSON
    - TERM-AUTOMATIC-PROJECT-NOTE
    - TERM-BILLING-ADDRESS
    - TERM-CONTACT
    - TERM-CONTACT-COMMERCIAL-DEFAULTS
    - TERM-CONTACT-NUMBER
    - TERM-CONTACT-PERSON
    - TERM-CONTACT-TYPE
    - TERM-CONTACT-WARNING
    - TERM-CUSTOMER
    - TERM-DEFAULT-CONTACT-PERSON
    - TERM-ISSUER
    - TERM-MAILING-ADDRESS
    - TERM-SUPPLIER-CONTACT
    - TERM-TAX-ID
    - TERM-TRAVEL-ESTIMATE
    - TERM-VENUE
    - TERM-VISITING-ADDRESS
    - CON-SINGLE-ISSUER
    - UC-DEFINE-ISSUER-PROFILE
    - UC-INT-LOOK-UP-COMPANY-DETAILS
    - UC-PRJ-ADD-CONTACT
    - UC-PRJ-ADD-CONTACT-PERSON
    - UC-PRJ-BULK-EDIT-CONTACTS
    - UC-PRJ-IMPORT-CONTACTS
    - UC-PRJ-MANAGE-RECORD-FOLDERS
    - UC-PRJ-SET-CONTACT-COMMERCIAL-DEFAULTS
    - UC-REGISTER-CUSTOMER
  remove:
    - BC-PARTIES
---

## Problem

The bounded context is named `Parties`, although the product language and user-facing experience consistently describe the people and organizations managed there as contacts. In audiovisual production, “parties” is also easily read as celebrations, making the context name misleading. The current Product Definition therefore does not use the domain’s established language consistently.

## Intended Product Outcome

The bounded context is named `Contacts` (`Contactos` in Spanish), with identifier `BC-CONTACTS`. Its responsibility, language, boundaries and external relationships remain unchanged: it covers the issuing organization, customers, private persons, contact persons, venues, suppliers, legal identities, addresses and commercial defaults. Every affected term, use case and constraint refers to `BC-CONTACTS`.

The product keeps `Project` as the umbrella concept. No separate `event` project type is introduced: the existing configurable `Project type` concept already classifies kinds of jobs, and an event can be represented by a project type without requiring a distinct product concept or behavior.

## Rationale

“Contact” is already the established product term in the affected use cases, terms and UI. Renaming the context aligns the bounded-context title with that language without changing its meaning or boundaries. Legal uses of “party” remain valid inside invoice legal-identity concepts and are not part of this context rename.

The existing project-type model describes configurable classifications for projects and already includes job-like examples such as dry hire, sale and festival. Adding `event` would add a label without a distinct semantic responsibility or behavior, so it is explicitly rejected by this change.

## Affected Product Areas

- The Contacts bounded context and its responsibility, language, boundaries and external relationships.
- Contact, customer, issuer, venue, address, tax-identifier and commercial-default domain terms defined in that context.
- Issuer, customer, company-detail lookup, contact-management and record-folder use cases assigned to the context.
- The single-issuer constraint that applies to the context.
- Project classification remains in the Projects bounded context and is unchanged.

## Open Questions

None.

## Product Acceptance

- The accepted model contains `BC-CONTACTS` titled “Contacts” with the same responsibility, language, boundaries and external relationships as the former `BC-PARTIES` context.
- No accepted artifact in the affected set refers to `BC-PARTIES`; all 29 affected artifacts retain their existing meaning while pointing to `BC-CONTACTS`.
- `TERM-PROJECT-TYPE` and its managing use case remain unchanged, and the change records why no separate `event` project type is needed.
- Product-model validation reports no errors.

## Out of Scope

This change does not rename code namespaces, classes, database tables, migrations, APIs, invoice legal-party concepts, issued-invoice representations, UI strings, or implementation tests. It does not add project-type behavior, alter project semantics, apply the change to the accepted model, or record delivery and deployment status.
