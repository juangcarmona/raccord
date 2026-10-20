<!-- pdac-scope: cited -->

## 1. Contacts domain and persistence

- [ ] 1.1 Add failing domain tests for valid person creation and editing, required names, supported contact details, one default and one administrative person per contact, failed designation changes preserving the previous state, and immutable parent ownership; implement the contact-person model after the tests fail for the expected reason.
- [ ] 1.2 Add failing persistence tests for parent-scoped list/create/edit, one-default and one-administrative constraints, concurrent default reassignment, and atomic rollback; add the migration and persistence adapter behavior required to pass them.
- [ ] 1.3 Add regression tests proving contact-person writes do not change project contact associations or issued invoice party snapshots.

<!-- pdac:cite id="FR-CON-002" digest="sha256:d44280cb004062fbc3bd34228233dcfa03b2b6e51ee64b4f717652351f721feb" -->
<!-- pdac:cite id="BR-CON-CONTACT-PERSON-DESIGNATIONS" digest="sha256:a0f4d168bcd4dc9375a7f21fd3341b41eb0d100c57fe6a1a25c54ea21986e27e" -->
<!-- pdac:cite id="BR-PRJ-CONTACT-PERSON-NOT-MOVABLE" digest="sha256:5b2f5e3280eb10ed7873b1e8d55d3c9f3eb57a339aef663b822a17da062c86af" -->

## 2. API contracts and application behavior

- [ ] 2.1 Add failing application and Web API tests for list, create, read and edit operations, existing Contacts authorization, missing parent/person errors, invalid details, rejected parent changes and designation conflicts; implement use cases and endpoints through the Contacts module boundary.
- [ ] 2.2 Regenerate OpenAPI and TypeScript contracts and verify API compatibility, module architecture rules and contract drift.

## 3. Contacts web experience

- [ ] 3.1 Add failing frontend tests for the contact-person list and empty/loading/error states, create/edit forms, default reassignment, administrative designation and server validation messages.
- [ ] 3.2 Implement the contact-person management section in the existing contact editor with accessible labels and English and Spanish copy, without adding delete or project/invoice selection controls.

## 4. Browser journey and issue evidence

- [ ] 4.1 Add one real-browser journey that creates a second contact person, changes its default and administrative designations, verifies the former default was cleared and the sole-default invariant, and confirms the project contact and issued-invoice recipient are unchanged; tag it `Area=contacts` and select it in `tests/Raccord.EndToEndTests/per-pr-selection.txt`.
- [ ] 4.2 Capture and inspect the browser report, screenshot, video and trace for the actual Contacts screen and verify the selected journey passes in CI.

<!-- pdac:cite id="UC-CON-MANAGE-CONTACT-PERSONS" digest="sha256:8253cf3c4c3b50fc1f53ca143ee99989c9c7652e0fa822e7d13ad5306bb43385" -->
<!-- pdac:cite id="SB-CON-CHANGE-DEFAULT-CONTACT-PERSON" digest="sha256:fc39711eb0d43ad05b94f3c9fbbf6b2b2178ae806e8a05da3af3d8d1b0459e97" -->

## 5. Documentation and delivery gates

- [ ] 5.1 Update the main Contacts OpenSpec specification and affected architecture documentation, and review the process ledger for a durable lesson and a verified follow-up action.
- [ ] 5.2 Run the relevant domain, persistence, Web API, frontend, architecture and selected E2E suites; then run the full CI-equivalent checks, OpenSpec validation, ProductShape validation and citation verification.
- [ ] 5.3 Reconcile every #299 acceptance criterion with passing tests or runtime evidence, update the PR with exact-head CI and E2E evidence, and confirm every scoped limitation has a tracked follow-up.

<!-- pdac:cite id="BC-CONTACTS" digest="sha256:bdadd84cb901027c8630d88c714ba8c52bcecdf383345d35a2631c7fa8b69c85" -->
<!-- pdac:cite id="FR-CON-002" digest="sha256:d44280cb004062fbc3bd34228233dcfa03b2b6e51ee64b4f717652351f721feb" -->
