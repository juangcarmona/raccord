<!-- pdac-scope: none reason="This change hardens HTTP response policy and verifies the deployed web boundary; it does not change product capabilities or domain semantics. The user authorized implementation of security issue #241." -->

# Tighten browser Content Security Policy

Issue #241 reports a wildcard protocol source in `connect-src`, `unsafe-inline` in `style-src`, and suspicious comments in the production JavaScript asset. The current frontend requires none of those allowances, and the emitted source-map reference exposes build metadata without a production need.

This change removes those allowances from the response policy, disables production source maps, asserts the complete policy in the web hosting tests, and exercises the CSP-bearing web endpoint in the existing hosted browser journey while confirming the SPA still renders.

## Product definition impact

No ProductShape artifacts are impacted. This is a transport security policy change and adds no user-visible behavior or product semantics. Product behavior remains covered by the existing seeded-project browser journey.

## Scope

- Tighten the CSP emitted by `src/Raccord.Web/Program.cs` to allow same-origin connections and styles only, and disable production source maps in `src/Raccord.Frontend/vite.config.ts`.
- Add exact unit-level policy coverage and browser-level verification against the Aspire web resource.
- Record the scanner interpretation and verification lesson in the operations process ledger.

Scanner notices about missing `Sec-Fetch-*` request headers and cacheability are not response policy directives and are outside this change.
