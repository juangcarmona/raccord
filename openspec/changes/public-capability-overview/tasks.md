<!-- pdac-scope: cited -->

## 1. Update the verified capability overview

- [x] 1.1 Replace the homepage's vague workflow description with the verified customer, project, billable-work, draft, issue, PDF and audit steps; verify the public-site content assertions pass.
- [x] 1.2 Replace the obsolete unauthenticated-app warning with accurate sign-in and no-hosted-demo wording; verify the browser journey checks sign-in and compliance disclosures.
- [x] 1.3 Update the repository README maturity statement so it no longer says authentication is missing; verify its status matches the implemented sign-in flow and excludes unsupported tax claims.

<!-- pdac:cite id="JRN-BILL-FINISHED-PROJECT" digest="sha256:b9530a2b0a9aac0f9cb3a5a9608494d08be1fc1190f395887fc7bb7fb30df28e" -->
<!-- pdac:cite id="UC-REGISTER-CUSTOMER" digest="sha256:e0f754e26dd900c146a21376b0fa9bd32e7da5e4849f3ab61fff86178c785227" -->
<!-- pdac:cite id="UC-CREATE-PROJECT" digest="sha256:b8bbb65b0f50ceeb882d7f30ec3219623b5f1242292f19240785f442960b755f" -->
<!-- pdac:cite id="UC-RECORD-BILLABLE-WORK" digest="sha256:01fdd6ec53f1b0be369bfe034bd4bfea3a00a5b3ff5ba54198b254b7db538336" -->
<!-- pdac:cite id="UC-CREATE-INVOICE-DRAFT" digest="sha256:f7310fed1fddd6fc7c813db19db677f8aea9feff2f179549c9a9425607804d33" -->
<!-- pdac:cite id="UC-ISSUE-INVOICE" digest="sha256:08b2bb0bd93fc7fbb0b745ee1e6e7f3f70d064d44da287ed8c478a8da7761afc" -->
<!-- pdac:cite id="UC-OBTAIN-INVOICE-DOCUMENT" digest="sha256:ba891b44976370dba63dc987cbe9fd4ba8d78de034684e9e5beb4cbfbfb07933" -->
<!-- pdac:cite id="UC-REVIEW-AUDIT-TRAIL" digest="sha256:76998e018b1b1b591ab6278c52a376fb388b6c12665e1c48648332eab6495d9a" -->
<!-- pdac:cite id="UC-WSP-SIGN-IN" digest="sha256:d3518a52b6a4950d1caa28d4ba04451e45ab0949d3cba65dffdbc1ebba0a9221" -->
<!-- pdac:cite id="CON-NO-COMPLIANCE-CLAIM" digest="sha256:a5035db4daaf71d39067c492306313089d3916634c536c81b4b91e41c7452492" -->

## 2. Verify the visitor journey

- [x] 2.1 Extend the public-site browser journey to assert the current workflow, sign-in disclosure, lack of a hosted demo, and the compliance boundary; run the journey at desktop and mobile viewports.
- [x] 2.2 Run the public-site build verification and verify the README and public copy contain no stale authentication claim.

## 3. Complete delivery evidence

- [ ] 3.1 Capture current application screenshots with synthetic data under the dependent screenshot issue #267 and include the verified workflow evidence on the public site.
- [ ] 3.2 Run the relevant CI workflows and attach passing results and rendered-page evidence to the pull request.
- [ ] 3.3 Validate this OpenSpec change and ProductShape citations; verify there are no stale, tampered, unresolved, unclassified or unbound citations.
