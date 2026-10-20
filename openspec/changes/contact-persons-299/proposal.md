<!-- pdac-scope: cited -->

## Why

Contacts can represent companies and private people, but the product has no accepted implementation workflow for maintaining the individual people affiliated with a contact. Operations coordinators need to keep those records and their default and administrative designations current without changing project assignments or invoice recipients.

## What Changes

- Add contact-person records that are created and edited under one existing contact and cannot be moved to another contact.
- Let authorized users assign default and administrative designations, allowing at most one person per contact for each designation.
- When a new default is selected, clear the previous default atomically and leave the previous designation intact if the update fails.
- Keep contact-person maintenance inside Contacts; do not use the designations for project selection or invoice/message delivery.
- Exclude deletion, custom fields and file attachments from this change.

## Capabilities

### New Capabilities

- `contact-persons`: Maintain people affiliated with a contact and their supported designations.

## Product definition impact

This delivery implements approved Product Change `CHG-CONTACT-PERSONS-299`. The use case and functional requirement define the behavior; the two designation rules govern one-default reassignment, administrative cardinality and the immutable parent association. The structured behavior is the browser journey's acceptance anchor.

<!-- pdac:cite id="UC-CON-MANAGE-CONTACT-PERSONS" digest="sha256:8253cf3c4c3b50fc1f53ca143ee99989c9c7652e0fa822e7d13ad5306bb43385" -->
<!-- pdac:cite id="FR-CON-002" digest="sha256:a04f4522eb929f86d105ac81b4f68aea091803e9d1a4b18e9210d2a50e4421d7" -->
<!-- pdac:cite id="BR-CON-CONTACT-PERSON-DESIGNATIONS" digest="sha256:47e4d43d3335cc85f1faafee6ec1eca70373441fe54704126c5c84417748ed8f" -->
<!-- pdac:cite id="BR-PRJ-CONTACT-PERSON-NOT-MOVABLE" digest="sha256:5b2f5e3280eb10ed7873b1e8d55d3c9f3eb57a339aef663b822a17da062c86af" -->
<!-- pdac:cite id="SB-CON-CHANGE-DEFAULT-CONTACT-PERSON" digest="sha256:fc39711eb0d43ad05b94f3c9fbbf6b2b2178ae806e8a05da3af3d8d1b0459e97" -->
<!-- pdac:cite id="BC-CONTACTS" digest="sha256:bdadd84cb901027c8630d88c714ba8c52bcecdf383345d35a2631c7fa8b69c85" -->

## Impact

- Extends the Contacts module domain, contracts, application use cases and persistence adapter with records owned by a parent contact.
- Adds authenticated contact-person API operations and generated OpenAPI/TypeScript contracts.
- Extends the contact edit experience in English and Spanish.
- Adds domain, persistence, API, frontend and real-browser coverage, including the per-PR journey manifest and evidence capture.
- Updates architecture documentation and the process ledger after reviewing the feature loop.

## Test impact

- Domain and persistence tests cover contact ownership, detail validation, designation cardinality and atomic default reassignment.
- API tests cover authorized create/read/edit operations, invalid parent changes, missing contacts, validation errors, and unchanged persisted state after rejected writes.
- Frontend tests cover loading, empty, success and error states plus create/edit and designation changes.
- A selected Playwright journey creates a second person, changes default and administrative designations, verifies the sole-default invariant and confirms existing project/invoice data is unchanged.

## Out of scope

Deleting or merging contacts or contact persons, custom fields, files, project contact-person selection, invoice recipient selection, email or message delivery, imports, and unrelated user or crew records.
