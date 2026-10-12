<!-- pdac-scope: cited -->

## 1. Establish the safe execution checkpoint

- [ ] 1.1 Read this change's proposal/design/spec/test plan and issue #30; record the approved plan commit and current main SHA in the PR. Execute only this task list, not the parent epic's. Verify: the work queue names #30 and excludes parent tasks 1.3 onward.
- [ ] 1.2 Restore tools/packages, build Release and run existing domain, infrastructure, architecture and web tests before edits. Record any baseline failure separately. Verify: commands in test-plan.md checkpoint A have an attributable baseline.

## 2. Extract pure shared types and ports

- [ ] 2.1 Add Raccord.SharedKernel to Raccord.slnx with a BCL-only csproj; move Common, Parties and Pricing using the exact design table. Adjust existing consumers, test usings and direct references. In the same checkpoint update only the interim architecture reference expectations and SharedKernel BCL rule. Verify: Release build plus Domain/Infrastructure/Architecture tests pass with identical financial assertions and no duplicate moved types.
- [ ] 2.2 Move IClock; rename ICurrentActor to ICurrentUser with the same Name property; update SystemClock, HttpCurrentActor, use-case constructor parameters and registration. Add a host-side test of default audit identity. Verify: existing web audit assertions and the new unauthenticated-identity test pass.
- [ ] 2.3 Write failing tests for append through IAuditAppender, including one persisted event and copied detail values. Add AuditEntry/IAuditAppender in SharedKernel; make the current IAuditTrail inherit the port and add AuditTrail's overload/alias registration as designed. Verify: append/read and actor/timestamp/action tests pass with one event, and SharedKernel references no Domain assembly.

<!-- pdac:cite id="BR-MONEY-ROUNDING" digest="sha256:51ad0cc84544ce872e3a93e25fb8110b701a85ec9efe737bc1ec15b8b985b87f" -->
<!-- pdac:cite id="BR-LINE-PRICING-BOUNDS" digest="sha256:652792a573f685a43f131a08b467a4de67c176c48e81ea5ba38aa1367e7c599c" -->
<!-- pdac:cite id="BR-TAX-ID-NORMALIZATION" digest="sha256:aadb655c57c6486610d3a3346f45bdd19696a407dd954d4fc37cd9cefd3391ca" -->
<!-- pdac:cite id="BR-PARTY-SNAPSHOT" digest="sha256:24dccb3800fada7dd1ae45a9844795011eaf44233be34465b3f9cf6a2a4e95d4" -->
<!-- pdac:cite id="BR-AUDITED-ACTIONS" digest="sha256:a29bab7607c529ef8b86f34e32758cf8557927cf7e71812bded9aac01c3ef469" -->
<!-- pdac:cite id="FR-AUDIT-001" digest="sha256:7646b5398f69286e802b2a3175edcf8b18cfc3b742edc9d3c2a44476c6159417" -->
<!-- pdac:cite id="QR-DETERMINISM" digest="sha256:2186a4638c7ad311864a05314e15638df09365c839eaa932ceb57eddcb1559e9" -->

## 3. Add BuildingBlocks and storage

- [ ] 3.1 Add Raccord.BuildingBlocks and Raccord.BuildingBlocks.Tests to the solution and the Architecture naming-test project reference list. Add only the design's allowed dependencies and centrally pinned ErrorOr/FluentValidation versions. Resolve and pin Azure.Storage.Blobs at the existing graph's version. Verify: restore, project-reference rules, package audit and license-policy command pass.
- [ ] 3.2 Move IDocumentStore, StorageOptions and both adapters; expose the narrow registration helper and preserve existing Aspire/storage selection in Infrastructure. Transfer filesystem adapter tests without weakening assertions; add overwrite/configuration/DI-selection coverage. Verify: old keys/options/defaults still work, containment tests pass and all PDF renderer tests are unchanged.
- [ ] 3.3 Add Azure adapter verification using a controlled SDK test transport for upload options/404. Cover container creation, overwrite, exact content type and byte readback. Verify: both storage providers' contract scenarios in test-plan.md pass; no production storage access is required.

<!-- pdac:cite id="JRN-BILL-FINISHED-PROJECT" digest="sha256:b9530a2b0a9aac0f9cb3a5a9608494d08be1fc1190f395887fc7bb7fb30df28e" -->
<!-- pdac:cite id="BR-ISSUED-INVOICE-IMMUTABLE" digest="sha256:309c88f0d7855c684b8ba796c0896ead9f6e821a19edfb731369a176a75ff8b3" -->
<!-- pdac:cite id="BR-INTEGRITY-VERIFICATION" digest="sha256:19dff5afbeb5e4cd25a3baaa4f1395ee74b0c0722572021e496e654cb44aaf34" -->
<!-- pdac:cite id="QR-DETERMINISM" digest="sha256:2186a4638c7ad311864a05314e15638df09365c839eaa932ceb57eddcb1559e9" -->

## 4. Implement failure mapping and dispatch in small increments

- [ ] 4.1 Write parameterized failing ApiResults tests for every row of design section 4, including generic safe 500 fallback and all validation issues. Implement ProblemIssue metadata and ToProblem only. Verify: assert actual HTTP response JSON, status, content type, type/code and omitted issues for nonvalidation failures.
- [ ] 4.2 Write failing tests for valid single handler invocation, invalid request short-circuit, multiple validators/order and cancellation. Implement request/handler/sender interfaces, explicit generic registration and validation-first dispatch. Verify: each test observes handler invocation count and token propagation.
- [ ] 4.3 Add domain exception conversion with the draft.not-open conflict special case and integrity passthrough; add Infrastructure ApplicationExceptionMapper for InvoiceValidationException and current concurrency exception. Register the application mapper explicitly. Verify: original codes/messages/issue objects survive; unknown exceptions and cancellation propagate; BuildingBlocks has no Application/Domain reference.
- [ ] 4.4 Exercise sender and ApiResults together through a test-only HTTP seam, including valid response, invalid shape, readiness issues, domain rule, conflict and critical integrity passthrough. Verify: current production endpoint behavior and tests remain unchanged.

<!-- pdac:cite id="UC-CREATE-INVOICE-DRAFT" digest="sha256:f7310fed1fddd6fc7c813db19db677f8aea9feff2f179549c9a9425607804d33" -->
<!-- pdac:cite id="UC-ISSUE-INVOICE" digest="sha256:08b2bb0bd93fc7fbb0b745ee1e6e7f3f70d064d44da287ed8c478a8da7761afc" -->
<!-- pdac:cite id="BR-ISSUED-INVOICE-IMMUTABLE" digest="sha256:309c88f0d7855c684b8ba796c0896ead9f6e821a19edfb731369a176a75ff8b3" -->
<!-- pdac:cite id="BR-INTEGRITY-VERIFICATION" digest="sha256:19dff5afbeb5e4cd25a3baaa4f1395ee74b0c0722572021e496e654cb44aaf34" -->

## 5. Prove atomic shared persistence

- [ ] 5.1 Add a Testcontainers PostgreSQL fixture and two test-only contexts/tables with a single external NpgsqlConnection. Tests must inspect committed data via a separate connection. Write failing success, duplicate-registry and connection-mismatch tests. Verify: the failure genuinely identifies missing shared UoW behavior.
- [ ] 5.2 Implement the ordered deduplicating registry, shared IUnitOfWork and shared transaction lifecycle from design section 5. Verify: success persists both rows, mismatched connections persist none, and contexts registered during Execute join the transaction.
- [ ] 5.3 Write failing rollback tests with a constraint failure after the first context has saved, exercising both registration orders; add callback failure and cancellation-after-first-save tests. Complete rollback/detach/connection ownership handling. Verify: fresh reads show no partial rows and original failures/cancellation are preserved.
- [ ] 5.4 Test EF concurrency and PostgreSQL unique violation mapping, successful scope reuse after commit, nested/active-transaction rejection and unsupported retry configuration. Implement the documented fail-fast safeguards. Verify: no case silently commits separately, retries an individual save or disposes a caller-owned connection.
- [ ] 5.5 Replace Infrastructure UnitOfWork with the current Application-port adapter; register the shared service against the current context's connection, retaining Aspire and design-time configuration. Verify: existing web finalization/conflict/audit tests pass, pending-model check reports no changes, and original Application exception semantics are retained.

<!-- pdac:cite id="UC-ISSUE-INVOICE" digest="sha256:08b2bb0bd93fc7fbb0b745ee1e6e7f3f70d064d44da287ed8c478a8da7761afc" -->
<!-- pdac:cite id="BR-GAPLESS-NUMBERING" digest="sha256:1d6404217e241d191188f8a20bdaedda2204e10548c344df7d46793d1c204adb" -->
<!-- pdac:cite id="BR-AUDITED-ACTIONS" digest="sha256:a29bab7607c529ef8b86f34e32758cf8557927cf7e71812bded9aac01c3ef469" -->
<!-- pdac:cite id="BR-COMPLIANCE-OBLIGATION" digest="sha256:dab51b5c3c98ca25c40abec4a8e477fdc172ac986788fd2233eb2560f14a4757" -->
<!-- pdac:cite id="FR-AUDIT-001" digest="sha256:7646b5398f69286e802b2a3175edcf8b18cfc3b742edc9d3c2a44476c6159417" -->
<!-- pdac:cite id="FR-COMPLIANCE-001" digest="sha256:a53549df92d28a28435890b7663e0a9c92ec126ff0988b6cb103c39d5aa050d8" -->
<!-- pdac:cite id="QR-CONCURRENCY-CONSISTENCY" digest="sha256:320945ebf06be10006380b4e183377f48569c75224692b903f3cf093887736a2" -->

## 6. Close compatibility and documentation

- [ ] 6.1 Check new shared project reference directions and all test naming; update architecture-rules.md and arc42 sections 5/8 with the exact interim graph and port/adapter ownership. Verify: documents do not claim module/schema migration is complete and architecture tests still detect forbidden dependencies.
- [ ] 6.2 Regenerate OpenAPI/client types and inspect the diff; verify unchanged routes, schemas and existing problem responses. Verify: contract-drift check and existing web tests pass; explain any operation-id/tag-only difference in the PR rather than changing schemas.
- [ ] 6.3 Build Release, run the four existing E2E journeys and the selection guard; leave per-pr-selection.txt unchanged. Verify: all four journeys pass with no quarantine or narrowed filter.
- [ ] 6.4 Mark parent module-architecture tasks 1.1/1.2 complete only when their checks have passed; leave all later tasks untouched. Record acceptance/test mapping and actual results in the PR. Verify: each issue criterion AC1–AC8 has evidence from test-plan.md.
- [ ] 6.5 Validate ProductShape/OpenSpec and all current citations. Refresh only genuinely stale citations by inspect/cite; never edit the accepted model or remove citations to suppress drift. Verify: validation and grounding commands are green.

<!-- pdac:cite id="JRN-BILL-FINISHED-PROJECT" digest="sha256:b9530a2b0a9aac0f9cb3a5a9608494d08be1fc1190f395887fc7bb7fb30df28e" -->
<!-- pdac:cite id="UC-CREATE-INVOICE-DRAFT" digest="sha256:f7310fed1fddd6fc7c813db19db677f8aea9feff2f179549c9a9425607804d33" -->
<!-- pdac:cite id="UC-ISSUE-INVOICE" digest="sha256:08b2bb0bd93fc7fbb0b745ee1e6e7f3f70d064d44da287ed8c478a8da7761afc" -->
<!-- pdac:cite id="BR-MONEY-ROUNDING" digest="sha256:51ad0cc84544ce872e3a93e25fb8110b701a85ec9efe737bc1ec15b8b985b87f" -->
<!-- pdac:cite id="BR-LINE-PRICING-BOUNDS" digest="sha256:652792a573f685a43f131a08b467a4de67c176c48e81ea5ba38aa1367e7c599c" -->
<!-- pdac:cite id="BR-TAX-ID-NORMALIZATION" digest="sha256:aadb655c57c6486610d3a3346f45bdd19696a407dd954d4fc37cd9cefd3391ca" -->
<!-- pdac:cite id="BR-PARTY-SNAPSHOT" digest="sha256:24dccb3800fada7dd1ae45a9844795011eaf44233be34465b3f9cf6a2a4e95d4" -->
<!-- pdac:cite id="BR-GAPLESS-NUMBERING" digest="sha256:1d6404217e241d191188f8a20bdaedda2204e10548c344df7d46793d1c204adb" -->
<!-- pdac:cite id="BR-ISSUED-INVOICE-IMMUTABLE" digest="sha256:309c88f0d7855c684b8ba796c0896ead9f6e821a19edfb731369a176a75ff8b3" -->
<!-- pdac:cite id="BR-AUDITED-ACTIONS" digest="sha256:a29bab7607c529ef8b86f34e32758cf8557927cf7e71812bded9aac01c3ef469" -->
<!-- pdac:cite id="BR-COMPLIANCE-OBLIGATION" digest="sha256:dab51b5c3c98ca25c40abec4a8e477fdc172ac986788fd2233eb2560f14a4757" -->
<!-- pdac:cite id="BR-INTEGRITY-VERIFICATION" digest="sha256:19dff5afbeb5e4cd25a3baaa4f1395ee74b0c0722572021e496e654cb44aaf34" -->
<!-- pdac:cite id="FR-AUDIT-001" digest="sha256:7646b5398f69286e802b2a3175edcf8b18cfc3b742edc9d3c2a44476c6159417" -->
<!-- pdac:cite id="FR-COMPLIANCE-001" digest="sha256:a53549df92d28a28435890b7663e0a9c92ec126ff0988b6cb103c39d5aa050d8" -->
<!-- pdac:cite id="QR-DETERMINISM" digest="sha256:2186a4638c7ad311864a05314e15638df09365c839eaa932ceb57eddcb1559e9" -->
<!-- pdac:cite id="QR-CONCURRENCY-CONSISTENCY" digest="sha256:320945ebf06be10006380b4e183377f48569c75224692b903f3cf093887736a2" -->

## 7. Finish at the Done gate

- [ ] 7.1 Run remaining applicable CI checks from test-plan.md checkpoint C and current ci.yml: formatting, lint/commitlint, pending model, frontend/API contract, security, package/license/SBOM and build/deployment artifact checks. Verify: record commands/results and the green CI run for the exact PR head.
- [ ] 7.2 Complete the eleven-item Done matrix in test-plan.md, list deliberate limitations with follow-up issue links (#31–#34 and epic #16 where relevant), and request Owner review. Verify: every checkbox is backed by evidence or a recorded drop reason; draft is marked ready only after implementation, never merged or deployed automatically.
