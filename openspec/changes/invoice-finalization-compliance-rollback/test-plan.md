<!-- pdac-scope: cited -->

# Test plan

1. Add the PostgreSQL failure-injection integration test before changing production code.
2. Confirm the test reaches compliance persistence, fails finalization, and verifies all persisted state through a fresh connection and the public API.
3. Drop the database trigger and retry the same draft; verify the expected gapless invoice number and normal success state.
4. Run the focused test, the full non-E2E backend suite, the selected invoice-finalization AppHost journey, the per-PR E2E battery, and all CI gates on the final head.

<!-- pdac:cite id="UC-ISSUE-INVOICE" digest="sha256:08b2bb0bd93fc7fbb0b745ee1e6e7f3f70d064d44da287ed8c478a8da7761afc" -->
<!-- pdac:cite id="BR-COMPLIANCE-OBLIGATION" digest="sha256:dab51b5c3c98ca25c40abec4a8e477fdc172ac986788fd2233eb2560f14a4757" -->
<!-- pdac:cite id="BR-GAPLESS-NUMBERING" digest="sha256:1d6404217e241d191188f8a20bdaedda2204e10548c344df7d46793d1c204adb" -->
<!-- pdac:cite id="BR-ISSUED-INVOICE-IMMUTABLE" digest="sha256:309c88f0d7855c684b8ba796c0896ead9f6e821a19edfb731369a176a75ff8b3" -->
<!-- pdac:cite id="QR-CONCURRENCY-CONSISTENCY" digest="sha256:320945ebf06be10006380b4e183377f48569c75224692b903f3cf093887736a2" -->
