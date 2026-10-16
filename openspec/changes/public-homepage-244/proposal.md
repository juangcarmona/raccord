<!-- pdac-scope: cited -->
<!-- pdac:cite id="ACT-OPERATIONS-COORDINATOR" digest="sha256:c6210b5da8b0aede8884fa0c04d95492b6f47095f239677bd7d5c647040798e7" -->
<!-- pdac:cite id="ACT-BILLING-ADMINISTRATOR" digest="sha256:2efd5f923a1f37f948e7e75d735e47901e8f2eed55bd4e686730529149da7de6" -->
<!-- pdac:cite id="JRN-BILL-FINISHED-PROJECT" digest="sha256:b9530a2b0a9aac0f9cb3a5a9608494d08be1fc1190f395887fc7bb7fb30df28e" -->
<!-- pdac:cite id="JRN-SET-UP-INVOICING" digest="sha256:a47b84f924d17095df0ad8bc4e3436fa67d9d51bd1d241c7228a7624cb8c548e" -->

## Why

Work item: [#244 — Raccord’s homepage explains its purpose and current maturity](https://github.com/juangcarmona/raccord/issues/244).

Raccord needs a clear public entry point that tells audiovisual production teams what the project is and where it stands. The purchased `raccord.dev` domain gives the project a stable home for this introduction.

## What Changes

- Add a Spanish-first public homepage that introduces Raccord and accurately presents its current maturity.
- Keep the site static and separate from the Raccord application so visiting the public site never exposes an unauthenticated application.
- Place editable site source under `src/public-site/`; the workflow boundary and publishing setup are tracked by [issue #247](https://github.com/juangcarmona/raccord/issues/247).
- Leave the content structure ready for future translations without delivering bilingual or multilingual content in this slice.

## Capabilities

### New Capabilities

- `public-homepage`: A public, Spanish-first homepage that introduces Raccord and accurately communicates its current maturity.

### Modified Capabilities

None.

## Impact

- Adds static site source under `src/public-site/`.
- Uses the project brand direction in `docs/brand/README.md`, which is exploratory and does not establish a final logo.
- Links to existing repository documentation for current status and self-hosting guidance.
- Does not change application APIs, database state, product behavior, or the product model.

## Product Definition Review

This site describes the roles and invoicing journeys in the accepted product definition; it does not add or promise new product behavior. The adjacent invoicing use cases and localization quality requirement were checked and left out: the homepage does not change product behavior or apply the product UI language requirement to the site. Detailed workflow claims and non-Spanish translations are outside this homepage slice and remain for later issues.

## Test Impact

- No application API, database, or product end-to-end journey changes.
- Verify the static build, internal and external links, responsive layout, keyboard navigation, and accessibility expectations.
- Verify the published copy against current README and self-hosting documentation, especially maturity and authentication disclosures.
- Attach rendered desktop and mobile screenshots or equivalent runtime evidence to the pull request.
- Run the repository CI checks that apply to the files and tools introduced by the implementation.

## Definition of Done

- Acceptance criteria map to passing checks or captured runtime evidence, with Owner confirmation.
- All implementation tasks are completed or explicitly dropped with a reason.
- Relevant CI and security checks pass; the Owner reviews any new public network surface.
- The Owner confirms the implementation follows this approved plan and applicable architecture decisions.
- Relevant architecture, operations, or ADR documentation is updated if the implementation changes those areas.
- ProductShape/OpenSpec validation and citation checks pass, with the Owner confirming product/spec/code consistency.
- A green CI run and visual evidence are linked from the pull request, and the Owner approves it.
- The process ledger is reviewed and durable lessons are recorded at the feature-loop close.
