<!-- pdac-scope: cited -->

## Purpose

Help prospective users judge which Raccord workflows they can use today, using claims tied to current code and repeatable browser evidence.

## ADDED Requirements

### Requirement: Show verified customer and billing workflows

The public site SHALL describe only user-visible workflows verified in the current application and SHALL identify the customer, project, billable-work, invoice-draft, invoice-issue, invoice-document and audit steps that are supported today.
<!-- pdac:cite id="JRN-BILL-FINISHED-PROJECT" digest="sha256:b9530a2b0a9aac0f9cb3a5a9608494d08be1fc1190f395887fc7bb7fb30df28e" -->
<!-- pdac:cite id="UC-REGISTER-CUSTOMER" digest="sha256:e0f754e26dd900c146a21376b0fa9bd32e7da5e4849f3ab61fff86178c785227" -->
<!-- pdac:cite id="UC-CREATE-PROJECT" digest="sha256:b8bbb65b0f50ceeb882d7f30ec3219623b5f1242292f19240785f442960b755f" -->
<!-- pdac:cite id="UC-RECORD-BILLABLE-WORK" digest="sha256:01fdd6ec53f1b0be369bfe034bd4bfea3a00a5b3ff5ba54198b254b7db538336" -->
<!-- pdac:cite id="UC-CREATE-INVOICE-DRAFT" digest="sha256:f7310fed1fddd6fc7c813db19db677f8aea9feff2f179549c9a9425607804d33" -->
<!-- pdac:cite id="UC-ISSUE-INVOICE" digest="sha256:08b2bb0bd93fc7fbb0b745ee1e6e7f3f70d064d44da287ed8c478a8da7761afc" -->
<!-- pdac:cite id="UC-OBTAIN-INVOICE-DOCUMENT" digest="sha256:ba891b44976370dba63dc987cbe9fd4ba8d78de034684e9e5beb4cbfbfb07933" -->
<!-- pdac:cite id="UC-REVIEW-AUDIT-TRAIL" digest="sha256:76998e018b1b1b591ab6278c52a376fb388b6c12665e1c48648332eab6495d9a" -->
<!-- pdac:cite id="BR-ISSUED-INVOICE-IMMUTABLE" digest="sha256:8138f31518261c618e3fef1dc93d5deaef6f55a4c3b2bfd41dfc0c5a4b75aecc" -->

#### Scenario: A visitor reviews the available workflow

- **WHEN** a visitor opens the Spanish homepage
- **THEN** the page explains the customer-to-invoice workflow using specific actions that are present in the application
- **AND** the visitor can distinguish those actions from capabilities that are unavailable or not verified

### Requirement: Describe authentication and legal limits accurately

The public site SHALL state that application routes require sign-in, SHALL not offer an unauthenticated application demo, and SHALL make no claim of legal or tax compliance without verified implementation and review.
<!-- pdac:cite id="UC-WSP-SIGN-IN" digest="sha256:d3518a52b6a4950d1caa28d4ba04451e45ab0949d3cba65dffdbc1ebba0a9221" -->
<!-- pdac:cite id="CON-NO-COMPLIANCE-CLAIM" digest="sha256:a5035db4daaf71d39067c492306313089d3916634c536c81b4b91e41c7452492" -->

#### Scenario: A visitor checks access and compliance limits

- **WHEN** a visitor reads the project maturity information
- **THEN** the page accurately says that sign-in is required and that no hosted public demo is available
- **AND** the page does not claim legal, tax or VERI*FACTU compliance
