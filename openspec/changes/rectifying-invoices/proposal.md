<!-- pdac-scope: cited -->
<!-- pdac:cite id="UC-CORRECT-ISSUED-INVOICE" digest="sha256:383b5851f22c6f2ab00014f0bffe0e2c4d24bd7efe038bd33a568572ec41804f" -->
<!-- pdac:cite id="BR-RECTIFYING-INVOICE" digest="sha256:e635a992493382932590a0dcb7e97081e6bbe3a7dcb8b4b248da1834f0e168bd" -->

# Proposal: Rectifying invoices

Add a rectification workflow that creates a new invoice from an issued invoice without mutating the original. The workflow supports differences and substitution, records the reason and references the original document.

The first product slice exposes the workflow through the existing invoicing API and preserves the existing append-only invoice and compliance boundaries.

## Constraints

- A rectifying invoice is issued in series `R`.
- Failed validation consumes no number.
- The original invoice remains immutable and independently retrievable.
