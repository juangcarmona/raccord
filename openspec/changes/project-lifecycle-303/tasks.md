<!-- pdac-scope: cited -->
# Tasks

- [x] Add the fixed project status model, default Draft status, manual status mutation and domain tests.
- [x] Persist project status with an EF Core migration.
- [x] Expose project status in API responses and add the manual status endpoint.
- [x] Add project status selection in the project detail page and status in project listings.
- [x] Add a browser journey proving status changes persist after reload and register it in the PR E2E selection.
- [x] Regenerate the OpenAPI and frontend API contracts for the status endpoint and response fields.
- [ ] Add editable project detail fields and duplicate project number validation.
- [ ] Add domain and persistence models for planning and usage periods with invariant tests.
- [ ] Expose period create/edit/order/display operations and linked-period propagation through the Projects module.
- [ ] Add project detail UI for the accepted editable fields and schedule.
- [ ] Add manual status effects, cancellation action discovery and explicit confirmation, including persistent warning for remaining work.
- [ ] Prove status and cancellation leave issued invoices and invoiced work unchanged.
- [ ] Add Playwright journeys for project editing, schedule validation, and cancellation with invoice history; add them to the PR E2E selection.
- [ ] Update affected architecture and operations documentation.
- [ ] Run the repository Done checks and reconcile all evidence.
- [ ] Run the repository Done checks and reconcile all evidence.

<!-- pdac:cite id="UC-PRJ-EDIT-PROJECT-DETAILS" digest="sha256:cc87eb90988fdbd32fb2e626a02aa7154783ffc7b161702a3c2bf4ace9c2bdb7" -->
<!-- pdac:cite id="UC-PRJ-MANAGE-TIME-SCHEDULE" digest="sha256:63f5e75776fe129ece805663868a322547da608fd04023944189c071b9c04bae" -->
<!-- pdac:cite id="UC-PRJ-CHANGE-PROJECT-STATUS" digest="sha256:1a5c43dfd663a08a833301fa25ccad7dc02057bfbc8c444f4fb18b3347905acc" -->
<!-- pdac:cite id="UC-PRJ-CANCEL-PROJECT" digest="sha256:80e15e9d11dba906cb7d753fd78b7ce8b29c30651bfe9cdb9ac1d7cdc3118d96" -->
