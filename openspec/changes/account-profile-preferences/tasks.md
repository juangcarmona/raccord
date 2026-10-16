<!-- pdac-scope: cited -->
<!-- pdac:cite id="UC-WSP-EDIT-PROFILE" digest="sha256:110a8e418914abd5f8a156d5729ec9a0055972823b845b5d17c5e1873d6b99ae" -->
<!-- pdac:cite id="UC-WSP-SET-LANGUAGE" digest="sha256:4e1830ebc5acc09bdedf5900f8543f6d29ee5a13439f608dfb055a43bf972aed" -->
<!-- pdac:cite id="BR-WSP-PROFILE-OWNED-BY-PERSON" digest="sha256:cc05019851b877d398a0a749334a59b40bc905731e3744f87b67532ba493f7c6" -->
<!-- pdac:cite id="BR-WSP-LANGUAGE-SCOPES" digest="sha256:495045d198c600a95725b0796363ad8d29d8690f67b494c4b12be5275178b335" -->

- [x] Add display name and preferred-language fields to the identity user and create/review the EF Core migration.
- [x] Extend `/api/auth/me` and sign-in responses; add a self-only profile update endpoint with validation and permission summary.
- [x] Regenerate the OpenAPI client and implement current-user profile state in the auth/i18n composition.
- [x] Add the account page, header account entry, permission-aware role link, and Spanish/English copy.
- [x] Add API/web and frontend tests for ownership, validation, permission visibility, saved preferences, and language restoration.
- [x] Add and select an AppHost Playwright journey that edits profile details, reloads/signs in again, and verifies language and access behavior.
- [x] Update relevant Workspace architecture and operation documentation if the implementation changes the documented API or identity persistence behavior.
- [ ] Run `prodshape validate`, `openspec validate --all`, citation verification, generated-contract drift checks, focused tests, the per-PR E2E selection, and the project CI-derived deterministic lane.
- [ ] Capture and inspect runtime evidence for the account page and its permission-aware navigation.
- [x] Review `docs/operations/process-ledger.md` and record durable process lessons from the feature loop.

<!-- pdac:cite id="ACT-CREW-MEMBER" digest="sha256:7e8350f158cc4ac6c30857753bcf74655a043878f28b0d5561343b7092ecfdbe" -->
<!-- pdac:cite id="BC-WORKSPACE" digest="sha256:013841dd37789974afef821eb2c071a85acaa72c083abd2285b7c406db95f8a5" -->
