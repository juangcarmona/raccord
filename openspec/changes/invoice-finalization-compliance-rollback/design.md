<!-- pdac-scope: cited -->

# Design: invoice finalization compliance rollback evidence

## Approach

Extend the existing isolated-PostgreSQL API integration tests. Arrange a valid synthetic issuer, customer, project, billable line and invoice draft through the existing test API. Install a database trigger on the compliance-record table that raises a deterministic error for the test database. The trigger exercises the actual persistence boundary without replacing the recorder or bypassing the shared connection and transaction.

Attempt finalization, then inspect the API and database directly. The failure must leave the draft open, its billable line unbilled, and no issued invoice, compliance row or invoice-issuance audit event. The draft-creation audit event must remain unchanged. Drop the trigger and retry the same draft; it must issue successfully with the first number in the isolated series/year, demonstrating that the failed attempt consumed none.

## Transaction boundary

The API invokes finalization through the existing shared unit of work. All participating module contexts enlist in the same PostgreSQL transaction and save before one commit. The trigger raises during compliance persistence after earlier context saves may have run; PostgreSQL rollback must undo every write from that transaction. Do not change the unit-of-work design unless the test disproves this invariant.

## Test isolation and safety

Each API test already receives a dedicated Testcontainers database and a temporary document root. Create and remove the failure trigger only in that database. Use fixed synthetic invoice data and never access production services or data. Keep injection unavailable to production configuration.

## Architecture and product impact

No production dependency, API, product behavior, schema or deployment change is planned. The test protects ADR-0019 and the existing compliance, numbering and immutability rules. If a runtime fix is needed, keep it inside the existing transaction boundary and update this plan before widening scope.

## Documentation

Update the process ledger with a durable lesson only if implementation or verification reveals a process gap. No user or operator documentation changes are expected for test-only coverage.

## Risks

The trigger must fail only the intended compliance insert, remain deterministic, and be removed before retry. Assertions must distinguish pre-existing draft-creation audit from issuance audit writes.

<!-- pdac:cite id="UC-ISSUE-INVOICE" digest="sha256:08b2bb0bd93fc7fbb0b745ee1e6e7f3f70d064d44da287ed8c478a8da7761afc" -->
<!-- pdac:cite id="BR-COMPLIANCE-OBLIGATION" digest="sha256:dab51b5c3c98ca25c40abec4a8e477fdc172ac986788fd2233eb2560f14a4757" -->
<!-- pdac:cite id="BR-GAPLESS-NUMBERING" digest="sha256:1d6404217e241d191188f8a20bdaedda2204e10548c344df7d46793d1c204adb" -->
<!-- pdac:cite id="BR-ISSUED-INVOICE-IMMUTABLE" digest="sha256:309c88f0d7855c684b8ba796c0896ead9f6e821a19edfb731369a176a75ff8b3" -->
<!-- pdac:cite id="QR-CONCURRENCY-CONSISTENCY" digest="sha256:320945ebf06be10006380b4e183377f48569c75224692b903f3cf093887736a2" -->
