<!-- pdac-scope: cited -->

## Integration evidence

- [x] 1.1 Add a deterministic compliance-persistence failure test to the isolated PostgreSQL API test lane; first run it against unchanged production code (focused test passed).
- [x] 1.2 Assert the failed attempt leaves the draft open, project line reserved in `in-draft`, no issued invoice, no issuance audit event, no compliance row, and the pre-existing draft audit intact; assert a successful retry marks the line `invoiced` (focused test passed).
- [x] 1.3 Remove the failure injection and retry; assert successful issuance receives the first expected invoice number for the isolated series and year (retry issued `F2026-00001`).
- [x] 1.4 Run focused invoicing integration tests, the complete backend test suite, and the existing selected invoice-finalization AppHost journey (14 invoicing rules tests, 138 backend tests, and 3 invoice-finalization AppHost journeys passed).
- [x] 1.5 Not needed: the focused test passes against the current transaction implementation; no runtime correction was required.
- [x] 1.6 Review the full diff and process ledger; record and verify the durable assertion lesson in `docs/operations/process-ledger.md`.

## Definition of Done

- [ ] DoD 1, 2 and 3: each criterion maps to passing test evidence; the task list is reconciled; CI tests are green and provide the required coverage.
- [ ] DoD 4: CSharpier, analyzers, Prettier/ESLint/typecheck, commitlint, EF model and contract-drift checks pass as applicable.
- [ ] DoD 5: security, package, license and SBOM gates pass; no authentication, authorization, personal-data or network surface changes are introduced.
- [ ] DoD 6 and 7: architecture conforms to the accepted plan; no architecture or operations docs require runtime updates for a test-only change.
- [ ] DoD 8 and 9: OpenSpec and ProductShape validations pass, citations are current, and final-head CI evidence is linked.
- [ ] DoD 10 and 11: Owner review is recorded; known limitations and their follow-up issue are stated in the PR.

<!-- pdac:cite id="UC-ISSUE-INVOICE" digest="sha256:08b2bb0bd93fc7fbb0b745ee1e6e7f3f70d064d44da287ed8c478a8da7761afc" -->
<!-- pdac:cite id="BR-COMPLIANCE-OBLIGATION" digest="sha256:dab51b5c3c98ca25c40abec4a8e477fdc172ac986788fd2233eb2560f14a4757" -->
<!-- pdac:cite id="BR-GAPLESS-NUMBERING" digest="sha256:1d6404217e241d191188f8a20bdaedda2204e10548c344df7d46793d1c204adb" -->
<!-- pdac:cite id="BR-ISSUED-INVOICE-IMMUTABLE" digest="sha256:309c88f0d7855c684b8ba796c0896ead9f6e821a19edfb731369a176a75ff8b3" -->
<!-- pdac:cite id="QR-CONCURRENCY-CONSISTENCY" digest="sha256:320945ebf06be10006380b4e183377f48569c75224692b903f3cf093887736a2" -->
