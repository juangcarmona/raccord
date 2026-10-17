<!-- pdac-scope: cited -->

## 1. AppHost coverage

- [x] 1.1 Add the blank-date journey and prove the frozen browser date differs from the issued organization's date; verify request null and saved date matches `IssuedAt` in `Europe/Madrid` (AppHost journey passed).
- [x] 1.2 Add the explicit-date journey; verify the submitted and saved invoice dates match the selected value (AppHost journey passed).
- [x] 1.3 Add both journey names to the committed Invoicing selection; verify the selector resolves both test cases (31 journeys discovered, manifest matches).
- [x] 1.4 Detach the named development Postgres volume from the E2E-only resource before applying session lifetime; verify AppHost creates a fresh database without changing the developer volume (AppHost health and browser journeys passed).

## 2. Verification and integration

- [x] 2.1 Run both targeted AppHost journeys and the complete per-PR AppHost battery; verify synthetic evidence records both as passed (31/31 passed; journey results, screenshots, videos, and traces saved under `/tmp/raccord-337-evidence`).
- [x] 2.2 Run the full CI-equivalent deterministic checks and OpenSpec/ProductShape validation; verify all required checks pass on the current main base (CI run 472 passed on head `b409c3613168a216eef0473cc264985ad6d7d263`: [CI run 472](https://github.com/juangcarmona/raccord/actions/runs/39028885746); local OpenSpec, ProductShape citations and deterministic checks passed).
- [x] 2.3 Review the implementation diff and process ledger; record any durable process lesson before merge (final review and local checks complete; hosted CI remains required by 2.2).

<!-- pdac:cite id="UC-ISSUE-INVOICE" digest="sha256:08b2bb0bd93fc7fbb0b745ee1e6e7f3f70d064d44da287ed8c478a8da7761afc" -->
<!-- pdac:cite id="FR-INVOICE-006" digest="sha256:fee328e3745e8275c00914dd830f3c7cbd3a1aec267f18ed117ef24ddb8e2466" -->
<!-- pdac:cite id="BR-GAPLESS-NUMBERING" digest="sha256:1d6404217e241d191188f8a20bdaedda2204e10548c344df7d46793d1c204adb" -->
<!-- pdac:cite id="BR-ISSUE-DATE-ORDER" digest="sha256:330d7738dac277884a78466f76c771e7a88c63c2cd3211a5318c33d418792aca" -->
