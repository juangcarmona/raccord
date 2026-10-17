<!-- pdac-scope: cited -->

## 1. AppHost coverage

- [ ] 1.1 Add the blank-date journey and prove the frozen browser date differs from the issued organization's date; verify request null and saved date matches `IssuedAt` in `Europe/Madrid`.
- [ ] 1.2 Add the explicit-date journey; verify the submitted and saved invoice dates match the selected value.
- [ ] 1.3 Add both journey names to the committed Invoicing selection; verify the selector resolves both test cases.

## 2. Verification and integration

- [ ] 2.1 Run both targeted AppHost journeys and the complete per-PR AppHost battery; verify synthetic report evidence records both as passed.
- [ ] 2.2 Run the full CI-equivalent deterministic checks and OpenSpec/ProductShape validation; verify all required checks pass on the current main base.
- [ ] 2.3 Review the final diff and process ledger; record any durable process lesson before merge.

<!-- pdac:cite id="UC-ISSUE-INVOICE" digest="sha256:08b2bb0bd93fc7fbb0b745ee1e6e7f3f70d064d44da287ed8c478a8da7761afc" -->
<!-- pdac:cite id="FR-INVOICE-006" digest="sha256:fee328e3745e8275c00914dd830f3c7cbd3a1aec267f18ed117ef24ddb8e2466" -->
<!-- pdac:cite id="BR-GAPLESS-NUMBERING" digest="sha256:1d6404217e241d191188f8a20bdaedda2204e10548c344df7d46793d1c204adb" -->
<!-- pdac:cite id="BR-ISSUE-DATE-ORDER" digest="sha256:330d7738dac277884a78466f76c771e7a88c63c2cd3211a5318c33d418792aca" -->
