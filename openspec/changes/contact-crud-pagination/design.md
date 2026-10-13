<!-- pdac-scope: cited -->
<!-- pdac:cite id="UC-PRJ-ADD-CONTACT" digest="sha256:2e1a5370a1272d63a080dfaf62c97c487748110e038add310a553fce4a787881" -->
<!-- pdac:cite id="UC-REGISTER-CUSTOMER" digest="sha256:e0f754e26dd900c146a21376b0fa9bd32e7da5e4849f3ab61fff86178c785227" -->

# Design

Contacts reuse the existing party persistence and invoice snapshot boundary. The Contacts API returns a page envelope with clamped page size, while updates replace mutable contact identity data without changing already-issued invoice records. The frontend consumes the generated contract for create, edit, and paginated list flows.
