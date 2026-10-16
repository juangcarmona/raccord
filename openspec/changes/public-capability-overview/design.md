<!-- pdac-scope: cited -->

## Context

The public homepage is a dependency-free Spanish HTML and CSS site in `src/public-site/`. Its visitor journey is covered by `PublicHomepageJourney_Should_IntroduceTheProject`. The existing route map documents customer, project and invoicing UI boundaries; authentication is implemented in the React shell and protected API, but the public homepage and README still claim it is unavailable.

<!-- pdac:cite id="JRN-BILL-FINISHED-PROJECT" digest="sha256:b9530a2b0a9aac0f9cb3a5a9608494d08be1fc1190f395887fc7bb7fb30df28e" -->
<!-- pdac:cite id="UC-REGISTER-CUSTOMER" digest="sha256:e0f754e26dd900c146a21376b0fa9bd32e7da5e4849f3ab61fff86178c785227" -->
<!-- pdac:cite id="UC-CREATE-PROJECT" digest="sha256:b8bbb65b0f50ceeb882d7f30ec3219623b5f1242292f19240785f442960b755f" -->
<!-- pdac:cite id="UC-RECORD-BILLABLE-WORK" digest="sha256:01fdd6ec53f1b0be369bfe034bd4bfea3a00a5b3ff5ba54198b254b7db538336" -->
<!-- pdac:cite id="UC-CREATE-INVOICE-DRAFT" digest="sha256:f7310fed1fddd6fc7c813db19db677f8aea9feff2f179549c9a9425607804d33" -->
<!-- pdac:cite id="UC-ISSUE-INVOICE" digest="sha256:08b2bb0bd93fc7fbb0b745ee1e6e7f3f70d064d44da287ed8c478a8da7761afc" -->
<!-- pdac:cite id="UC-OBTAIN-INVOICE-DOCUMENT" digest="sha256:ba891b44976370dba63dc987cbe9fd4ba8d78de034684e9e5beb4cbfbfb07933" -->
<!-- pdac:cite id="UC-REVIEW-AUDIT-TRAIL" digest="sha256:76998e018b1b1b591ab6278c52a376fb388b6c12665e1c48648332eab6495d9a" -->
<!-- pdac:cite id="BR-ISSUED-INVOICE-IMMUTABLE" digest="sha256:309c88f0d7855c684b8ba796c0896ead9f6e821a19edfb731369a176a75ff8b3" -->
<!-- pdac:cite id="UC-WSP-SIGN-IN" digest="sha256:d3518a52b6a4950d1caa28d4ba04451e45ab0949d3cba65dffdbc1ebba0a9221" -->
<!-- pdac:cite id="CON-NO-COMPLIANCE-CLAIM" digest="sha256:a5035db4daaf71d39067c492306313089d3916634c536c81b4b91e41c7452492" -->

## Goals / Non-Goals

**Goals:**

- Give visitors a precise account of the current customer, project and invoice workflow.
- Remove the false statement that authentication is absent while retaining clear limits on public trial access and legal claims.
- Tie public claims to application routes and the existing E2E journey.

**Non-Goals:**

- Describing crew, warehouse, document-authoring or integration workflows as available public UI until separately verified.
- Adding a hosted demo, new application features, product analytics, translated pages or screenshots in this change.
- Claiming legal, tax or VERI*FACTU compliance.

## Decisions

- Keep the page static and dependency-free. This change only edits existing HTML, its existing checks, the browser journey assertions and the README status; it does not alter application routes, APIs or data.
- Describe only workflows visible in the current frontend and covered by browser tests. The broader ProductShape model and API-only journeys are not sufficient evidence for claims about a visitor-ready application screen.
- State that sign-in protects application access. State separately that there is no hosted public demo or self-service registration; do not repeat the obsolete claim that authentication is missing.
- Keep the no-compliance-claim warning explicit. Payment management and tax submission remain outside the available-capabilities list until the current application exposes and verifies them.

<!-- pdac:cite id="JRN-BILL-FINISHED-PROJECT" digest="sha256:b9530a2b0a9aac0f9cb3a5a9608494d08be1fc1190f395887fc7bb7fb30df28e" -->
<!-- pdac:cite id="UC-REGISTER-CUSTOMER" digest="sha256:e0f754e26dd900c146a21376b0fa9bd32e7da5e4849f3ab61fff86178c785227" -->
<!-- pdac:cite id="UC-CREATE-PROJECT" digest="sha256:b8bbb65b0f50ceeb882d7f30ec3219623b5f1242292f19240785f442960b755f" -->
<!-- pdac:cite id="UC-RECORD-BILLABLE-WORK" digest="sha256:01fdd6ec53f1b0be369bfe034bd4bfea3a00a5b3ff5ba54198b254b7db538336" -->
<!-- pdac:cite id="UC-CREATE-INVOICE-DRAFT" digest="sha256:f7310fed1fddd6fc7c813db19db677f8aea9feff2f179549c9a9425607804d33" -->
<!-- pdac:cite id="UC-ISSUE-INVOICE" digest="sha256:08b2bb0bd93fc7fbb0b745ee1e6e7f3f70d064d44da287ed8c478a8da7761afc" -->
<!-- pdac:cite id="UC-OBTAIN-INVOICE-DOCUMENT" digest="sha256:ba891b44976370dba63dc987cbe9fd4ba8d78de034684e9e5beb4cbfbfb07933" -->
<!-- pdac:cite id="UC-REVIEW-AUDIT-TRAIL" digest="sha256:76998e018b1b1b591ab6278c52a376fb388b6c12665e1c48648332eab6495d9a" -->
<!-- pdac:cite id="BR-ISSUED-INVOICE-IMMUTABLE" digest="sha256:309c88f0d7855c684b8ba796c0896ead9f6e821a19edfb731369a176a75ff8b3" -->
<!-- pdac:cite id="UC-WSP-SIGN-IN" digest="sha256:d3518a52b6a4950d1caa28d4ba04451e45ab0949d3cba65dffdbc1ebba0a9221" -->
<!-- pdac:cite id="CON-NO-COMPLIANCE-CLAIM" digest="sha256:a5035db4daaf71d39067c492306313089d3916634c536c81b4b91e41c7452492" -->

## Risks / Trade-offs

The narrow inventory may understate backend capabilities that do not yet have visitor-ready UI. That is preferable to presenting a draft ProductShape workflow or API endpoint as a complete business workflow. The dedicated screenshots issue can add visual evidence after this inventory is agreed.
