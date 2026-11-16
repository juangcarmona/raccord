# Tasks

## 1. Recover the contact directory prerequisite

- [ ] 1.1 Recover and requalify approved #298 contact-directory domain, persistence, API, authorization, audit, and editor behavior; verify directory/API/editor tests cover installation isolation and authorized access.
- [ ] 1.2 Add or reconcile directory schema and migration; verify migration/model tests and existing customer/project/invoice data behavior.

## 2. Implement contact-person behavior

- [ ] 2.1 Add contact-person domain and persistence with immutable parent ownership and unique designation constraints; verify domain and persistence tests cover validation, uniqueness, and atomic reassignment.
- [ ] 2.2 Add authorized list/create/read/edit API operations and audit evidence; verify Web tests cover invalid input, missing/wrong parent, authorization, installation isolation, and transaction conflicts.
- [ ] 2.3 Add the contact editor people workflow; verify frontend tests cover listing, create/edit, and independent designation reassignment.

## 3. Verify end-to-end invariants

- [ ] 3.1 Add a Contacts-tagged selected browser journey; verify it creates and edits people, changes default and administrative designations, and proves sole-designation invariants.
- [ ] 3.2 Verify project associations and issued-invoice snapshots remain unchanged in regression coverage; run the required solution, frontend, ProductShape, E2E-selection, browser, and strict implementation-plan checks and record completed receipts.
