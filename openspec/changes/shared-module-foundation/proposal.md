<!-- pdac-scope: cited -->

## Why

Issue [#30](https://github.com/juangcarmona/raccord/issues/30), the first slice of epic #16, needs a shared foundation before the existing billing code can move into modules. The current primitives, ports and adapters live in layered projects; extracting them now provides the agreed module dependencies while retaining billing, audit and HTTP compatibility.

## What Changes

- Add `src/shared/Raccord.SharedKernel` for existing Common, Parties and Pricing types and the clock, current-user and audit-append ports.
- Add `src/shared/Raccord.BuildingBlocks` for dispatch/validation, expected-error conversion, HTTP result mapping, shared-connection persistence and document storage.
- Keep the current application operational through explicit compatibility adapters; move no module or host and generate no schema migration.
- Add central ErrorOr/FluentValidation versions justified by accepted ADR-0022.
- Add focused shared-foundation tests and the minimal architecture checks required to exercise the two new assemblies.
- Deliver only parent OpenSpec `module-architecture` tasks 1.1 and 1.2. This separate execution plan prevents an implementer from accidentally picking up all five module migrations.

## Capabilities

### New Capabilities

- `shared-module-foundation`: a consumable shared foundation with validation before dispatch, stable failure responses, atomic writes across participating contexts and compatible value objects and document storage.

### Modified Capabilities

None. No capability has yet been archived under `openspec/specs/`; this bounded implementation slice realizes part of the existing `module-architecture` plan.

## Impact

**Code and integration contracts:** existing internal C# namespaces and project references change (**BREAKING** for source consumers of moved types). HTTP routes, request/response schemas, current problem responses, database schema/data, migration history, storage settings, existing PDF keys/content and deployment paths remain compatible. CI adds test coverage through projects included in the solution without narrowing existing gates.

**Dependencies:** ErrorOr 2.1.1 (MIT) and FluentValidation 12.1.1 (Apache-2.0), pinned centrally. Existing EF/Npgsql/Azure versions are reused. See [ErrorOr package](https://www.nuget.org/packages/ErrorOr/2.1.1) and [FluentValidation package](https://www.nuget.org/packages/FluentValidation/12.1.1); restore/license/vulnerability verification remains an implementation gate.

**Product impact analysis:** the full 1,880-artifact baseline was inventoried and the following preserved dependencies were read and widened with `prodshape impact --depth 2`. None is altered or contradicted:

- Journey/use cases: `JRN-BILL-FINISHED-PROJECT`, `UC-CREATE-INVOICE-DRAFT`, `UC-ISSUE-INVOICE`.
- Value-object/pricing guarantees: `BR-MONEY-ROUNDING`, `BR-LINE-PRICING-BOUNDS`, `BR-TAX-ID-NORMALIZATION`, `BR-PARTY-SNAPSHOT`.
- Persistence/audit/integrity guarantees: `BR-GAPLESS-NUMBERING`, `BR-ISSUED-INVOICE-IMMUTABLE`, `BR-AUDITED-ACTIONS`, `BR-COMPLIANCE-OBLIGATION`, `BR-INTEGRITY-VERIFICATION`, `FR-AUDIT-001`, `FR-COMPLIANCE-001`.
- Quality: `QR-DETERMINISM`, `QR-CONCURRENCY-CONSISTENCY`.

<!-- pdac:cite id="JRN-BILL-FINISHED-PROJECT" digest="sha256:b9530a2b0a9aac0f9cb3a5a9608494d08be1fc1190f395887fc7bb7fb30df28e" -->
<!-- pdac:cite id="UC-CREATE-INVOICE-DRAFT" digest="sha256:f7310fed1fddd6fc7c813db19db677f8aea9feff2f179549c9a9425607804d33" -->
<!-- pdac:cite id="UC-ISSUE-INVOICE" digest="sha256:08b2bb0bd93fc7fbb0b745ee1e6e7f3f70d064d44da287ed8c478a8da7761afc" -->
<!-- pdac:cite id="BR-MONEY-ROUNDING" digest="sha256:51ad0cc84544ce872e3a93e25fb8110b701a85ec9efe737bc1ec15b8b985b87f" -->
<!-- pdac:cite id="BR-LINE-PRICING-BOUNDS" digest="sha256:652792a573f685a43f131a08b467a4de67c176c48e81ea5ba38aa1367e7c599c" -->
<!-- pdac:cite id="BR-TAX-ID-NORMALIZATION" digest="sha256:aadb655c57c6486610d3a3346f45bdd19696a407dd954d4fc37cd9cefd3391ca" -->
<!-- pdac:cite id="BR-PARTY-SNAPSHOT" digest="sha256:f278c6a11a99fe63feadf0105b0ce359e74c71580b9b070a4fbf95c3b34b1604" -->
<!-- pdac:cite id="BR-GAPLESS-NUMBERING" digest="sha256:1d6404217e241d191188f8a20bdaedda2204e10548c344df7d46793d1c204adb" -->
<!-- pdac:cite id="BR-ISSUED-INVOICE-IMMUTABLE" digest="sha256:309c88f0d7855c684b8ba796c0896ead9f6e821a19edfb731369a176a75ff8b3" -->
<!-- pdac:cite id="BR-AUDITED-ACTIONS" digest="sha256:33a6a76f11ff3ed15dcbd22798ea6b65a05d65d4130cb17c7751eb516266e54c" -->
<!-- pdac:cite id="BR-COMPLIANCE-OBLIGATION" digest="sha256:dab51b5c3c98ca25c40abec4a8e477fdc172ac986788fd2233eb2560f14a4757" -->
<!-- pdac:cite id="BR-INTEGRITY-VERIFICATION" digest="sha256:19dff5afbeb5e4cd25a3baaa4f1395ee74b0c0722572021e496e654cb44aaf34" -->
<!-- pdac:cite id="FR-AUDIT-001" digest="sha256:7646b5398f69286e802b2a3175edcf8b18cfc3b742edc9d3c2a44476c6159417" -->
<!-- pdac:cite id="FR-COMPLIANCE-001" digest="sha256:a53549df92d28a28435890b7663e0a9c92ec126ff0988b6cb103c39d5aa050d8" -->
<!-- pdac:cite id="QR-DETERMINISM" digest="sha256:2186a4638c7ad311864a05314e15638df09365c839eaa932ceb57eddcb1559e9" -->
<!-- pdac:cite id="QR-CONCURRENCY-CONSISTENCY" digest="sha256:320945ebf06be10006380b4e183377f48569c75224692b903f3cf093887736a2" -->

Neighbour review: `SB-LINE-NET-ROUNDING`, `SB-MIXED-RATE-TOTALS`, `SB-CONCURRENT-ISSUING-GAPLESS`, `SB-LINE-IN-TWO-DRAFTS` and `SB-TAMPERED-INVOICE-REFUSED` remain regression scenarios. `BR-VAT-PER-RATE`, `BR-SINGLE-BILLING`, `BR-ISSUE-READINESS`, `BR-ISSUE-DATE-ORDER` and `BR-OPERATION-DATE` remain enforced by unchanged billing logic. `UC-CORRECT-ISSUED-INVOICE`, `UC-REPORT-BILLING-RECORDS`, `JRN-FIN-STAGED-BILLING`, `FR-COMPLIANCE-002` and `BR-VERIFACTU-DIRECT-SUBMISSION` are excluded because this slice introduces neither billing capabilities nor reporting workers. `QR-ACCESS-CONTROL`, `QR-ACCESSIBILITY` and `QR-LOCALIZATION` were checked; permissions and UI behavior do not change. `CON-SIF-REGULATION` remains a constraint of the existing compliance model; this proposal changes no legal/tax interpretation or claim.

**Product definition drift:** none identified. No new Product Change is required. The applied Contacts change `CHG-RENAME-PARTIES-TO-CONTACTS-001` remains the context-name authority.

**Documentation drift:** `architecture-rules.md` still enforces the pre-ADR-0018 BCL-only Domain graph. This is an expected staged transition, but its exact-reference architecture tests will fail after extraction unless updated in this slice. Design defines that narrow interim exception; the full module graph, analyzer rules and schema migration remain in #31 and later epic #16 slices.

Read [design.md](design.md) for the selected interfaces and compatibility seams, [tasks.md](tasks.md) for the bounded execution sequence and [test-plan.md](test-plan.md) for acceptance evidence and all eleven Done dimensions.
