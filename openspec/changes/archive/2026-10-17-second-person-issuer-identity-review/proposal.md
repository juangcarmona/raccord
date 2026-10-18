---
pdac-scope: cited
---

# Protect issuer legal identity changes after invoicing starts

<!-- pdac:cite id="FR-ISSUER-001" digest="sha256:1d443dcade4a5af9dce47d75a383a243a298dd37de5a2d1be22bc80d5dfe1712" -->
<!-- pdac:cite id="UC-DEFINE-ISSUER-PROFILE" digest="sha256:fd058a02a9f6f90de16249ea291336c0193cfefce4236e0081a00f6da3223b34" -->
<!-- pdac:cite id="BR-PARTY-SNAPSHOT" digest="sha256:f278c6a11a99fe63feadf0105b0ce359e74c71580b9b070a4fbf95c3b34b1604" -->
<!-- pdac:cite id="BR-AUDITED-ACTIONS" digest="sha256:33a6a76f11ff3ed15dcbd22798ea6b65a05d65d4130cb17c7751eb516266e54c" -->
<!-- pdac:cite id="CON-SINGLE-ISSUER" digest="sha256:f41d4ba8da941e762b3ee568fa15b478f1a60443cb2ea905d4826b13cdb1e1cd" -->

## Problem

Issuer settings currently apply legal-name and tax-identifier edits immediately, even after an invoice has been issued. This bypasses the approved second-person safeguard and leaves no request/decision workflow.

## Proposed change

After the first issued invoice, a legal-name or tax-identifier update becomes a pending request with a reason. A distinct administrator with issuer-settings permission may approve or reject it. Approval applies the proposed identity for future invoices only. Address and contact edits remain immediate. Before any invoice is issued, legal-identity edits remain immediate. Requests and decisions are append-only audit events; rejected or abandoned requests remain retained and unapplied.

The accepted Product Change `CHG-ISSUER-TAX-IDENTITY-001` defines the product behavior. The implementation adds no product scope and makes no legal-compliance claim. A different legal entity remains outside this workflow and requires a separate installation.

## Impacted product definition

Impacted and cited artifacts: `FR-ISSUER-001`, `UC-DEFINE-ISSUER-PROFILE`, `BR-PARTY-SNAPSHOT`, `BR-AUDITED-ACTIONS`, and `CON-SINGLE-ISSUER`.

The wider impact check also inspected `BR-TAX-ID-NORMALIZATION`, `BC-CONTACTS`, `BC-INVOICING`, `FR-INVOICE-006`, `JRN-SET-UP-INVOICING`, `TERM-ISSUER`, and `TERM-AUDIT-EVENT`. The tax-id normalization rule and invoice snapshot requirements remain in force; the journey is unchanged at product level; no new vocabulary or context boundary is needed. No product-definition drift was found.

## Scope

- Add an issuer legal-identity change request lifecycle and reviewer decision endpoints.
- Enforce distinct actors and issuer-settings permission on both request and decision.
- Preserve one-admin address/contact changes and first-invoice setup behavior.
- Record request and decision details in append-only audit events.
- Add focused backend, UI, and AppHost end-to-end coverage.

Out of scope: changing any real issuer identity, modifying finalized invoices, determining invoice rectification, switching legal entities, or making compliance claims.

## Definition of Done

- Every acceptance criterion maps to passing automated tests or recorded runtime evidence.
- New behavior starts with failing tests; CI suites and quality/security checks pass.
- Architecture boundaries and accepted ADRs are respected; Owner judgement remains required for security, architecture, product consistency, and test adequacy.
- A green CI run for the final PR head is linked from the PR; runtime evidence accompanies the UI journey.
- Current OpenSpec artifacts validate and all product citations are current.
- The process ledger is reviewed and records any durable lesson before integration.

## Red lines

No credentials, shared history, production configuration/data, or live deployment are touched.
