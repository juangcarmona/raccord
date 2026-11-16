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
<!-- pdac:cite id="UC-CREATE-INVOICE-DRAFT" digest="sha256:19acf3217f0478eb0624b44b2f11a30f5d2e589895ea162cda25924f79e7c4cf" -->
<!-- pdac:cite id="UC-ISSUE-INVOICE" digest="sha256:342ce2d34a022b8a621fd91efff10751c2dd841e6f42b28c5530dbc5ead7eb63" -->
<!-- pdac:cite id="BR-MONEY-ROUNDING" digest="sha256:51ad0cc84544ce872e3a93e25fb8110b701a85ec9efe737bc1ec15b8b985b87f" -->
<!-- pdac:cite id="BR-LINE-PRICING-BOUNDS" digest="sha256:540c744a9100bf27b3094353016316aa7e50ea96791665e3f6c5be545ae611cc" -->
<!-- pdac:cite id="BR-TAX-ID-NORMALIZATION" digest="sha256:2a2b07a88ddffe4bd6b3de5750620c50efbf8e58001278b0a1da2215e5a69d7a" -->
<!-- pdac:cite id="BR-PARTY-SNAPSHOT" digest="sha256:eec706ec68420337799bb8bcbeb1c690876f8c071a823de6e087387d8f3f6ac5" -->
<!-- pdac:cite id="BR-GAPLESS-NUMBERING" digest="sha256:a41628aa11dfdcd37418fb32f9d94a6240857646cd8cad02b084e573f0c6f613" -->
<!-- pdac:cite id="BR-ISSUED-INVOICE-IMMUTABLE" digest="sha256:309c88f0d7855c684b8ba796c0896ead9f6e821a19edfb731369a176a75ff8b3" -->
<!-- pdac:cite id="BR-AUDITED-ACTIONS" digest="sha256:33a6a76f11ff3ed15dcbd22798ea6b65a05d65d4130cb17c7751eb516266e54c" -->
<!-- pdac:cite id="BR-COMPLIANCE-OBLIGATION" digest="sha256:895fc96f4b9c40db18c3ad4c529c4bfb3188fac61ccb68b4cd46e79ac3d3c12b" -->
<!-- pdac:cite id="BR-INTEGRITY-VERIFICATION" digest="sha256:16d52ea384cf2be82ec2a7c754500a52ca21f9bbd2c62827a24d2c3f75e3026f" -->
<!-- pdac:cite id="FR-AUDIT-001" digest="sha256:7646b5398f69286e802b2a3175edcf8b18cfc3b742edc9d3c2a44476c6159417" -->
<!-- pdac:cite id="FR-COMPLIANCE-001" digest="sha256:a53549df92d28a28435890b7663e0a9c92ec126ff0988b6cb103c39d5aa050d8" -->
<!-- pdac:cite id="QR-DETERMINISM" digest="sha256:2186a4638c7ad311864a05314e15638df09365c839eaa932ceb57eddcb1559e9" -->
<!-- pdac:cite id="QR-CONCURRENCY-CONSISTENCY" digest="sha256:320945ebf06be10006380b4e183377f48569c75224692b903f3cf093887736a2" -->

Neighbour review: `SB-LINE-NET-ROUNDING`, `SB-MIXED-RATE-TOTALS`, `SB-CONCURRENT-ISSUING-GAPLESS`, `SB-LINE-IN-TWO-DRAFTS` and `SB-TAMPERED-INVOICE-REFUSED` remain regression scenarios. `BR-VAT-PER-RATE`, `BR-SINGLE-BILLING`, `BR-ISSUE-READINESS`, `BR-ISSUE-DATE-ORDER` and `BR-OPERATION-DATE` remain enforced by unchanged billing logic. `UC-CORRECT-ISSUED-INVOICE`, `UC-REPORT-BILLING-RECORDS`, `JRN-FIN-STAGED-BILLING`, `FR-COMPLIANCE-002` and `BR-VERIFACTU-DIRECT-SUBMISSION` are excluded because this slice introduces neither billing capabilities nor reporting workers. `QR-ACCESS-CONTROL`, `QR-ACCESSIBILITY` and `QR-LOCALIZATION` were checked; permissions and UI behavior do not change. `CON-SIF-REGULATION` remains a constraint of the existing compliance model; this proposal changes no legal/tax interpretation or claim.

**Product definition drift:** none identified. No new Product Change is required. The applied Contacts change `CHG-RENAME-PARTIES-TO-CONTACTS-001` remains the context-name authority.

**Documentation drift:** `architecture-rules.md` still enforces the pre-ADR-0018 BCL-only Domain graph. This is an expected staged transition, but its exact-reference architecture tests will fail after extraction unless updated in this slice. Design defines that narrow interim exception; the full module graph, analyzer rules and schema migration remain in #31 and later epic #16 slices.

Read [design.md](design.md) for the selected interfaces and compatibility seams, [tasks.md](tasks.md) for the bounded execution sequence and [test-plan.md](test-plan.md) for acceptance evidence and all eleven Done dimensions.
