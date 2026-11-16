<!-- pdac-scope: cited -->

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

<!-- pdac:cite id="FR-CON-001" digest="sha256:a7e73bb8c5c496226d8074b5fda85a5f974703596c57ee0f7b10c49eba3e040e" -->
<!-- pdac:cite id="FR-CON-002" digest="sha256:050518c8af11e5d7f1e90fa0e7e4f71e949c4923ac5e8d8335697cd4c3fa235b" -->
<!-- pdac:cite id="BR-CON-CONTACT-PERSON-DESIGNATIONS" digest="sha256:b80ca1deda21cf11609d89248e6aa165275057b80e02519feb6ea2cbb967c033" -->
<!-- pdac:cite id="BR-PRJ-CONTACT-PERSON-NOT-MOVABLE" digest="sha256:de5fef396fa13aa8f696c139c356dc719be7dde97995a7db49bf9c8d15111c8b" -->
<!-- pdac:cite id="BR-PRJ-INVOICE-RECIPIENT" digest="sha256:5bd94b44ca62c6c328b9dcfee904b108a0543af7e5a048e4637c7daca3816e52" -->
<!-- pdac:cite id="SB-CON-CHANGE-DEFAULT-CONTACT-PERSON" digest="sha256:5cd239965aa4275900b2daef3a6f6d520dbe10391c8f83e0aa8ae3c517d2d8ea" -->
<!-- pdac:cite id="SB-CON-REASSIGN-ADMINISTRATIVE-CONTACT-PERSON" digest="sha256:0513919df0cd30d0d16fc75d98b69f1c76d8824d19e38a292620a9c0c661abb4" -->
<!-- pdac:cite id="SB-CON-CONTACT-PERSON-PRESERVES-PARTY-SNAPSHOT" digest="sha256:93235b2bbd6b5add5a7f27ceed023329ab7ca3af5b2c4a2e4f3178d997095124" -->
