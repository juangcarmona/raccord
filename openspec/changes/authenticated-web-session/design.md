<!-- pdac-scope: cited -->
<!-- pdac:cite id="UC-WSP-SIGN-IN" digest="sha256:d3518a52b6a4950d1caa28d4ba04451e45ab0949d3cba65dffdbc1ebba0a9221" -->

# Design

Keep session state in a small frontend provider backed by generated OpenAPI calls. Route protection is navigation UX only; the server remains the authorization boundary. Restore `/api/auth/me` on startup, remember the requested route, and clear state only after sign-out succeeds.
