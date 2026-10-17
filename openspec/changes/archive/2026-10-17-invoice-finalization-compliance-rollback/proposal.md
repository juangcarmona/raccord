<!-- pdac-scope: cited -->

# Prove invoice finalization rolls back when compliance persistence fails

## Outcome

A deterministic failure while persisting an invoice's compliance obligation proves that cross-module finalization rolls back every issuance side effect and leaves the number available for a retry.

## Motivation

Invoice finalization allocates a gapless number, stores an immutable invoice, marks project lines invoiced, appends audit events and creates a compliance record in one shared transaction. Existing evidence covers successful finalization and concurrency, but does not force the compliance write to fail after other contexts have performed their saves.

## Scope

Add a focused PostgreSQL integration test using synthetic data and a test-only persistence failure. Assert rollback across invoicing, projects, audit, compliance and numbering. If the test reveals a defect, make only the smallest correction required by the existing transaction invariant.

## Non-goals

Do not add invoice behavior, change the public API, alter the numbering policy, make compliance claims, or touch production configuration or data.

## Product grounding

This verifies existing invoice issuance, gapless numbering, immutable issued records and transaction-consistency rules; product semantics do not change, so no Product Change or delta spec is required.

<!-- pdac:cite id="UC-ISSUE-INVOICE" digest="sha256:08b2bb0bd93fc7fbb0b745ee1e6e7f3f70d064d44da287ed8c478a8da7761afc" -->
<!-- pdac:cite id="BR-COMPLIANCE-OBLIGATION" digest="sha256:dab51b5c3c98ca25c40abec4a8e477fdc172ac986788fd2233eb2560f14a4757" -->
<!-- pdac:cite id="BR-GAPLESS-NUMBERING" digest="sha256:1d6404217e241d191188f8a20bdaedda2204e10548c344df7d46793d1c204adb" -->
<!-- pdac:cite id="BR-ISSUED-INVOICE-IMMUTABLE" digest="sha256:309c88f0d7855c684b8ba796c0896ead9f6e821a19edfb731369a176a75ff8b3" -->
<!-- pdac:cite id="QR-CONCURRENCY-CONSISTENCY" digest="sha256:320945ebf06be10006380b4e183377f48569c75224692b903f3cf093887736a2" -->
