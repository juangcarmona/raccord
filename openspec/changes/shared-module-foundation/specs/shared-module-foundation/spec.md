<!-- pdac-scope: cited -->

## Purpose

Provides shared application foundations that downstream module slices can consume while preserving current billing, audit, error-response and document-storage behavior.

## ADDED Requirements

### Requirement: Shared primitives preserve observable domain behavior

The extracted shared primitives SHALL preserve the existing behaviors of money, pricing, legal identities, tax identifiers and invoice representation consumers under `src/Raccord.Domain/`. Existing domain test expectations SHALL remain unchanged except for source namespaces/references.

<!-- pdac:cite id="BR-MONEY-ROUNDING" digest="sha256:51ad0cc84544ce872e3a93e25fb8110b701a85ec9efe737bc1ec15b8b985b87f" -->
<!-- pdac:cite id="BR-LINE-PRICING-BOUNDS" digest="sha256:652792a573f685a43f131a08b467a4de67c176c48e81ea5ba38aa1367e7c599c" -->
<!-- pdac:cite id="BR-TAX-ID-NORMALIZATION" digest="sha256:aadb655c57c6486610d3a3346f45bdd19696a407dd954d4fc37cd9cefd3391ca" -->
<!-- pdac:cite id="BR-PARTY-SNAPSHOT" digest="sha256:c39682fe0c3b9b93d43648b9417cb60d24ad54f2d3693a6de415029cb19a8a96" -->
<!-- pdac:cite id="QR-DETERMINISM" digest="sha256:2186a4638c7ad311864a05314e15638df09365c839eaa932ceb57eddcb1559e9" -->

#### Scenario: Existing financial and party tests after extraction

- **WHEN** the domain and PDF/representation tests run against consumers of the extracted shared primitives
- **THEN** their current calculation, normalization, snapshot, representation and byte-output assertions pass without weakened expectations

### Requirement: Current attribution and audit append remain compatible

The shared audit port SHALL append to pending persistence work and SHALL preserve the caller's audit facts. The current unauthenticated actor value and existing audit retrieval SHALL remain compatible with `src/Raccord.Web/Api/HttpCurrentActor.cs` and `src/Raccord.Infrastructure/Persistence/Repositories/AuditTrail.cs`.

<!-- pdac:cite id="BR-AUDITED-ACTIONS" digest="sha256:33a6a76f11ff3ed15dcbd22798ea6b65a05d65d4130cb17c7751eb516266e54c" -->
<!-- pdac:cite id="FR-AUDIT-001" digest="sha256:7646b5398f69286e802b2a3175edcf8b18cfc3b742edc9d3c2a44476c6159417" -->

#### Scenario: Append through the shared port

- **WHEN** an entry is appended through the shared audit port and its unit of work commits
- **THEN** exactly one event with the supplied actor, timestamp, action, entity and details is persisted and visible through current audit retrieval

#### Scenario: Default actor

- **WHEN** a request has no authenticated identity
- **THEN** its existing audit attribution remains `unauthenticated`

### Requirement: Validation gates request dispatch

The shared dispatcher SHALL validate input before invoking its handler, SHALL retain all reported validation issues in deterministic order, and SHALL propagate cancellation. It SHALL return a successful handler result unchanged when validation succeeds.

<!-- pdac:cite id="UC-CREATE-INVOICE-DRAFT" digest="sha256:f7310fed1fddd6fc7c813db19db677f8aea9feff2f179549c9a9425607804d33" -->
<!-- pdac:cite id="UC-ISSUE-INVOICE" digest="sha256:08b2bb0bd93fc7fbb0b745ee1e6e7f3f70d064d44da287ed8c478a8da7761afc" -->

#### Scenario: Invalid input is rejected before handling

- **WHEN** a registered request fails two validation checks
- **THEN** the dispatcher returns both issues and does not invoke the handler

#### Scenario: Valid input reaches one handler

- **WHEN** a valid registered request is dispatched
- **THEN** its handler runs once with the same cancellation token and its result is returned unchanged

### Requirement: Expected failures preserve stable HTTP semantics

The shared failure mapping SHALL preserve the existing status and problem-code conventions in `src/Raccord.Web/Api/ApiExceptionHandler.cs`. Validation and rule errors SHALL map to 422, conflict to 409, not found to 404, forbidden to 403 and unauthenticated to 401. Problem responses SHALL include `type = urn:raccord:problem:<code>` and `code`; validation issues SHALL retain the current `{code,message}` shape. Defects and integrity failures SHALL not be downgraded to business-rule failures.

<!-- pdac:cite id="UC-ISSUE-INVOICE" digest="sha256:08b2bb0bd93fc7fbb0b745ee1e6e7f3f70d064d44da287ed8c478a8da7761afc" -->
<!-- pdac:cite id="BR-ISSUED-INVOICE-IMMUTABLE" digest="sha256:8138f31518261c618e3fef1dc93d5deaef6f55a4c3b2bfd41dfc0c5a4b75aecc" -->
<!-- pdac:cite id="BR-INTEGRITY-VERIFICATION" digest="sha256:19dff5afbeb5e4cd25a3baaa4f1395ee74b0c0722572021e496e654cb44aaf34" -->

#### Scenario: Already-issued draft

- **WHEN** the dispatcher observes the existing `draft.not-open` domain failure
- **THEN** result mapping yields a 409 problem retaining `draft.not-open`

#### Scenario: Readiness reports all issues

- **WHEN** the application mapper observes InvoiceValidationException with multiple readiness issues
- **THEN** the resulting 422 problem retains `draft.invalid` and each original issue code/message

#### Scenario: Integrity failure remains exceptional

- **WHEN** the dispatcher observes `issued-invoice.integrity-violation`
- **THEN** it propagates the failure to the existing critical-log/500 handling path

#### Scenario: Failure types are mapped

- **WHEN** a consumer uses each supported expected error type with a stable code
- **THEN** its HTTP status, problem type and code match the mapping above

### Requirement: Participating contexts commit or roll back together

Shared persistence SHALL execute participating context writes on the same PostgreSQL connection in one transaction with a single commit. Any callback, enlistment or persistence failure SHALL roll back all database writes in that transaction. Conflicting writes SHALL preserve the existing retryable conflict meaning. Independent connection instances SHALL be rejected rather than silently committed separately.

<!-- pdac:cite id="UC-ISSUE-INVOICE" digest="sha256:08b2bb0bd93fc7fbb0b745ee1e6e7f3f70d064d44da287ed8c478a8da7761afc" -->
<!-- pdac:cite id="BR-GAPLESS-NUMBERING" digest="sha256:1d6404217e241d191188f8a20bdaedda2204e10548c344df7d46793d1c204adb" -->
<!-- pdac:cite id="BR-AUDITED-ACTIONS" digest="sha256:33a6a76f11ff3ed15dcbd22798ea6b65a05d65d4130cb17c7751eb516266e54c" -->
<!-- pdac:cite id="BR-COMPLIANCE-OBLIGATION" digest="sha256:dab51b5c3c98ca25c40abec4a8e477fdc172ac986788fd2233eb2560f14a4757" -->
<!-- pdac:cite id="FR-AUDIT-001" digest="sha256:7646b5398f69286e802b2a3175edcf8b18cfc3b742edc9d3c2a44476c6159417" -->
<!-- pdac:cite id="FR-COMPLIANCE-001" digest="sha256:a53549df92d28a28435890b7663e0a9c92ec126ff0988b6cb103c39d5aa050d8" -->
<!-- pdac:cite id="QR-CONCURRENCY-CONSISTENCY" digest="sha256:320945ebf06be10006380b4e183377f48569c75224692b903f3cf093887736a2" -->

#### Scenario: Two successful contexts

- **WHEN** two test contexts using one connection save valid rows in a shared operation
- **THEN** a fresh connection sees both rows after a single successful commit

#### Scenario: Second context fails after first save

- **WHEN** the first context saves and the second violates a database constraint
- **THEN** a fresh connection sees neither pending row and the operation fails

#### Scenario: Cancellation after persistence begins

- **WHEN** an operation is cancelled after a participating context has saved
- **THEN** the operation remains cancelled and the transaction's rows are absent through a fresh connection

#### Scenario: Connection mismatch

- **WHEN** a participating context uses a different connection object
- **THEN** the shared operation fails before persistence and neither context independently commits

### Requirement: Current storage and API consumers remain compatible

Extracted storage SHALL retain the current `Storage` options, document keys, overwrite semantics, content types, missing-document behavior and filesystem containment rules. The current HTTP endpoints and regenerated OpenAPI schemas SHALL remain compatible; this foundation SHALL not create module migrations or remove existing end-to-end selection entries.

<!-- pdac:cite id="JRN-BILL-FINISHED-PROJECT" digest="sha256:b9530a2b0a9aac0f9cb3a5a9608494d08be1fc1190f395887fc7bb7fb30df28e" -->
<!-- pdac:cite id="BR-PARTY-SNAPSHOT" digest="sha256:c39682fe0c3b9b93d43648b9417cb60d24ad54f2d3693a6de415029cb19a8a96" -->
<!-- pdac:cite id="BR-ISSUED-INVOICE-IMMUTABLE" digest="sha256:8138f31518261c618e3fef1dc93d5deaef6f55a4c3b2bfd41dfc0c5a4b75aecc" -->
<!-- pdac:cite id="BR-INTEGRITY-VERIFICATION" digest="sha256:19dff5afbeb5e4cd25a3baaa4f1395ee74b0c0722572021e496e654cb44aaf34" -->
<!-- pdac:cite id="QR-DETERMINISM" digest="sha256:2186a4638c7ad311864a05314e15638df09365c839eaa932ceb57eddcb1559e9" -->

#### Scenario: Storage adapter extraction

- **WHEN** a document is saved, read and overwritten through either current storage provider
- **THEN** callers observe the existing bytes/content-type/key behavior and an absent document returns null

#### Scenario: Key escapes root

- **WHEN** the filesystem store receives an existing invalid absolute or traversal key
- **THEN** the operation fails with the same containment behavior as its current tests

#### Scenario: Current billing journey

- **WHEN** the existing web and four selected E2E journeys run after extraction
- **THEN** their current assertions pass, API routes/schemas retain compatibility, and no EF migration is necessary
