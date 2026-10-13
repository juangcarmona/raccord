<!-- pdac-scope: cited -->
<!-- pdac:cite id="BR-COMPLIANCE-OBLIGATION" digest="sha256:dab51b5c3c98ca25c40abec4a8e477fdc172ac986788fd2233eb2560f14a4757" -->

# Design

The compliance recorder creates the chain entry in the same unit of work as the issued invoice. Each row stores the predecessor hash and a SHA-256 record hash over the invoice identity, record kind, canonical invoice hash, predecessor, and creation timestamp. Existing append-only database triggers continue to protect the rows from deletion.
