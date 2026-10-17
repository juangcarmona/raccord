<!-- pdac-scope: cited -->

## Why

Issue #337 records that the browser-level regression for the server-authoritative invoice date needs the same proof through the real AppHost browser journey. This change adds that evidence to the committed per-PR battery while preserving the existing invoice contract.

## What Changes

- Add an AppHost journey that freezes the browser clock on a clearly different date, leaves the issue date blank, asserts that finalization sends `null`, and verifies the issued date against `IssuedAt` in the organization's configured time zone.
- Add a companion AppHost journey that explicitly selects a date and verifies both the finalize request and issued invoice preserve it.
- Select both journeys in the committed Invoicing per-PR battery.
- Make the E2E Postgres resource omit the AppHost development volume so each session-lifetime test database starts with fresh credentials and data while the developer volume remains untouched.
- Change no invoice behavior, API contract, validation, or product-model semantics.

## Capabilities

No product capability requirement changes. This is test-only evidence for the existing invoice finalization contract, so `skip_specs: true` is declared in `.openspec.yaml`.

## Product definition impact

The tests exercise the invoice issue-date choice and immutable invoice result governed by `UC-ISSUE-INVOICE` and `FR-INVOICE-006`; they preserve `BR-GAPLESS-NUMBERING` and `BR-ISSUE-DATE-ORDER` without changing either rule. The structural impact review found adjacent requirements and behaviors for invoice ordering, concurrency, and immutability; those are not changed by this test-only slice. No product drift is identified.

<!-- pdac:cite id="UC-ISSUE-INVOICE" digest="sha256:08b2bb0bd93fc7fbb0b745ee1e6e7f3f70d064d44da287ed8c478a8da7761afc" -->
<!-- pdac:cite id="FR-INVOICE-006" digest="sha256:fee328e3745e8275c00914dd830f3c7cbd3a1aec267f18ed117ef24ddb8e2466" -->
<!-- pdac:cite id="BR-GAPLESS-NUMBERING" digest="sha256:1d6404217e241d191188f8a20bdaedda2204e10548c344df7d46793d1c204adb" -->
<!-- pdac:cite id="BR-ISSUE-DATE-ORDER" digest="sha256:330d7738dac277884a78466f76c771e7a88c63c2cd3211a5318c33d418792aca" -->

## Impact

Only the invoice E2E journey, its selection manifest, and E2E-only PostgreSQL fixture configuration change. There are no API, domain, dependency, deployment, or production changes. Runtime evidence is synthetic and captured through the existing AppHost harness; the persistent development volume is preserved.
