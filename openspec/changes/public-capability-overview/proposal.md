<!-- pdac-scope: cited -->

## Why

Visitors cannot tell which parts of Raccord they can use today: the homepage describes only a vague production-to-invoice flow, while the repository contains a working sign-in, customer and project screens, billable work, invoice drafts, issued invoices, PDF retrieval and audit history. The current public copy also says authentication is missing, which conflicts with the implemented sign-in and protected API.

## What Changes

- Describe only capabilities confirmed in the current frontend, backend and passing browser journeys.
- Explain the verified customer → project → billable work → invoice draft → issue → PDF and audit path in concrete Spanish.
- State that sign-in protects the application and that no hosted public demo or self-service registration is offered.
- Keep unsupported payment and tax-submission claims out of the available-capabilities list; preserve the explicit no-legal-compliance-claim boundary.
- Update the public-site journey assertions and the public README maturity statement to match current behavior.

## Capabilities

### New Capabilities

- `public-capability-overview`: public visitors can distinguish verified current workflows from unavailable or unverified capabilities.

### Modified Capabilities

None. No application product behavior changes; this adds accurate public information about existing behavior.

## Product definition impact

This change describes existing customer registration, project creation, billable work, invoice drafting, invoice issue, invoice documents, audit review and sign-in. It does not change those product behaviors or add product scope.

<!-- pdac:cite id="ACT-OPERATIONS-COORDINATOR" digest="sha256:c6210b5da8b0aede8884fa0c04d95492b6f47095f239677bd7d5c647040798e7" -->
<!-- pdac:cite id="ACT-BILLING-ADMINISTRATOR" digest="sha256:2efd5f923a1f37f948e7e75d735e47901e8f2eed55bd4e686730529149da7de6" -->
<!-- pdac:cite id="JRN-BILL-FINISHED-PROJECT" digest="sha256:b9530a2b0a9aac0f9cb3a5a9608494d08be1fc1190f395887fc7bb7fb30df28e" -->
<!-- pdac:cite id="UC-REGISTER-CUSTOMER" digest="sha256:2ed3ef0366ab20037c6770f083c9883517370afc4cae9deb633f20bf6e960f23" -->
<!-- pdac:cite id="UC-CREATE-PROJECT" digest="sha256:2416d85758de14ca9785a48a1aa604ba6fbba2b85b520092035d55a407e51a10" -->
<!-- pdac:cite id="UC-RECORD-BILLABLE-WORK" digest="sha256:01fdd6ec53f1b0be369bfe034bd4bfea3a00a5b3ff5ba54198b254b7db538336" -->
<!-- pdac:cite id="UC-CREATE-INVOICE-DRAFT" digest="sha256:19acf3217f0478eb0624b44b2f11a30f5d2e589895ea162cda25924f79e7c4cf" -->
<!-- pdac:cite id="UC-ISSUE-INVOICE" digest="sha256:342ce2d34a022b8a621fd91efff10751c2dd841e6f42b28c5530dbc5ead7eb63" -->
<!-- pdac:cite id="UC-OBTAIN-INVOICE-DOCUMENT" digest="sha256:ba891b44976370dba63dc987cbe9fd4ba8d78de034684e9e5beb4cbfbfb07933" -->
<!-- pdac:cite id="UC-REVIEW-AUDIT-TRAIL" digest="sha256:76998e018b1b1b591ab6278c52a376fb388b6c12665e1c48648332eab6495d9a" -->
<!-- pdac:cite id="UC-WSP-SIGN-IN" digest="sha256:d3518a52b6a4950d1caa28d4ba04451e45ab0949d3cba65dffdbc1ebba0a9221" -->
<!-- pdac:cite id="BR-ISSUED-INVOICE-IMMUTABLE" digest="sha256:309c88f0d7855c684b8ba796c0896ead9f6e821a19edfb731369a176a75ff8b3" -->
<!-- pdac:cite id="CON-NO-COMPLIANCE-CLAIM" digest="sha256:a5035db4daaf71d39067c492306313089d3916634c536c81b4b91e41c7452492" -->

The impact review checked the direct neighbors of these artifacts, including customer normalization, project periods and numbering, priced billable lines, one-time billing, invoice numbering and immutability, concurrent billing, document integrity, audit events, access control and sign-in constraints. These rules remain unchanged. Broader modeled crew, warehouse, document-authoring and integration workflows are excluded from public availability claims because this change has not verified them as visitor-ready UI workflows.

## Impact

- `src/public-site/index.html` and its public-site checks.
- `tests/Raccord.EndToEndTests/PublicSite/PublicHomepageJourney_Should_IntroduceTheProject.cs`.
- The public maturity section in `README.md`.
- No API, database schema, runtime dependency, or deployment configuration changes.
