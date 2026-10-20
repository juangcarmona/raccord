<!-- pdac-scope: cited -->

## 1. Contacts domain and persistence

- [ ] 1.1 Add failing domain tests for company/person contact creation, multiple supported role tags, required legal name, unique number rules and invalid role rejection; implement the Contact behavior after the tests fail for the expected reason.
- [ ] 1.2 Add failing persistence tests for creating and editing contacts, role filtering, deterministic paginated reads and number uniqueness under concurrent creation; implement the repository and database-backed allocator.
- [ ] 1.3 Add an EF migration that preserves all existing customer IDs, project references and invoice source references, classifies existing rows as company/customer, assigns deterministic contact numbers, and seeds the next number; add an upgrade preservation test from the previous schema.
- [ ] 1.4 Preserve `IPartyDirectory`/`IPartyWriter` behavior for project and invoice modules and add regression tests proving customer-role filtering and ID resolution.

## 2. API contracts and application behavior

- [ ] 2.1 Add failing application tests for create, detail, update, role membership, authorization failures, pagination bounds and duplicate-number errors; implement contact-directory use cases.
- [ ] 2.2 Add `/api/contacts` create/list/detail/update endpoints with bounded pagination, role filtering and validated errors; keep `/api/customers` responses limited to customer-role contacts and preserve existing request/response compatibility.
- [ ] 2.3 Regenerate OpenAPI and TypeScript contracts, then verify the contract-drift guard and existing project/invoice API tests.

## 3. Contacts web experience

- [ ] 3.1 Add failing frontend tests for loading/empty/error states, role and type entry, create/edit submission, page navigation, role filtering and preservation of server validation messages.
- [ ] 3.2 Implement the Contacts list, detail/edit flow and create form with English and Spanish copy, keyboard labels and responsive table behavior; keep customer selection screens on the compatibility customer projection.
- [ ] 3.3 Verify project creation and invoice preparation still list only contacts with the customer role.

## 4. Browser journeys and issue evidence

- [ ] 4.1 Add a real-browser directory journey with synthetic data: create a company carrying customer+supplier roles and a private-person venue, edit one record, and find the records across paginated and role-filtered results. Tag it `Area=contacts` and add it to `tests/Raccord.EndToEndTests/per-pr-selection.txt`.
- [ ] 4.2 Add a real-browser issued-snapshot journey: create customer/project/billable work, issue an invoice, edit the customer, then reload the issued invoice and assert original issue-time recipient details remain; verify a future invoice sees edited details if the UI/API journey can do so without unrelated setup.
- [ ] 4.3 Keep the existing customer registration journey passing and inspect captured screenshots/report rows to ensure each image shows the actual Contacts or Invoice page exercised.

## 5. Documentation and delivery gates

- [ ] 5.1 Update arc42 building-block/runtime and risks/debt documentation to describe the Contacts directory, API compatibility seam, pagination, migration and immutable invoice interaction.
- [ ] 5.2 Run migration, domain, application, Web API, frontend, architecture and selected E2E suites; include failing-first TDD evidence and the full CI-equivalent checks required by the lifecycle.
- [ ] 5.3 Verify ProductShape, OpenSpec and citation gates; attach actual browser screenshots/runtime evidence; reconcile each acceptance criterion to a passing test or evidence and record known limitations with follow-up issues.
- [ ] 5.4 Review `docs/operations/process-ledger.md`, record one durable process lesson if this loop found a repeatable failure, and verify any resulting action.

<!-- pdac:cite id="FR-CON-001" digest="sha256:9b09ae74057045edaba5e51c4d14f287a22093be7537c8867a4fee8e07ccaa5f" -->
<!-- pdac:cite id="UC-CON-MANAGE-CONTACT-DIRECTORY" digest="sha256:58989f1d3dfd59bb6021eb503753e4f9bccd6bd57f4dc43697dddf535c057901" -->
<!-- pdac:cite id="BR-PARTY-SNAPSHOT" digest="sha256:5ac10d9c4ab5f68acd5a18213fe4c9c0d50786549a1d94c760409214b13355f7" -->
<!-- pdac:cite id="BR-PRJ-PARTIES-FROM-CONTACTS" digest="sha256:2fe8ce59516db4c08c3582e1c3dd6341fa3b658e8386d10f725671b633b2dcfe" -->
<!-- pdac:cite id="BR-PRJ-CONTACT-NUMBER-ASSIGNMENT" digest="sha256:aa32e12b334cd585d5d946fb961a46d5ddfda39bc4b16320a9c5f0b19d7ce8d7" -->
<!-- pdac:cite id="BR-PRJ-CONTACT-ROLES-BY-TAG" digest="sha256:d7110eb5318168938b259d3f44fb022f04666454b5112e97d17539861b8604d9" -->
<!-- pdac:cite id="UC-REGISTER-CUSTOMER" digest="sha256:1cbbec6400a8cf4165c0a7c82742d6e71e9e434e267e6cc3a2af8c575164670f" -->
