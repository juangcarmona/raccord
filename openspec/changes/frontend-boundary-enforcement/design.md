<!-- pdac-scope: cited -->
<!-- pdac:cite id="UC-WSP-SIGN-IN" digest="sha256:d3518a52b6a4950d1caa28d4ba04451e45ab0949d3cba65dffdbc1ebba0a9221" -->

# Design

Use a dependency-free Node checker over relative TypeScript imports under `src/app`, `src/features` and `src/shared`. The checker permits app composition and feature-to-shared imports, rejects reverse-layer and peer-feature imports, and runs as a named step in the existing frontend CI job. Vitest fixtures exercise the rule without changing production folder structure.
