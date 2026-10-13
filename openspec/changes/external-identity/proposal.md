# External identity sign-in

Implement the optional external sign-in slice from ADR-0021. An installation may configure one OpenID Connect provider. External identities are linked to an existing local account by an authenticated user; an external sign-in never creates an account implicitly. Administrators can mark a role as requiring external sign-in, which rejects password sign-in for members of that role.

The slice implements `UC-WSP-SIGN-IN`, `UC-WSP-ENFORCE-SSO`, `UC-WSP-RESOLVE-SSO-NONCOMPLIANCE` and `TERM-EXTERNAL-SIGN-IN`.

<!-- pdac-scope: cited -->
<!-- pdac:cite id="UC-WSP-SIGN-IN" digest="sha256:d3518a52b6a4950d1caa28d4ba04451e45ab0949d3cba65dffdbc1ebba0a9221" -->
<!-- pdac:cite id="UC-WSP-ENFORCE-SSO" digest="sha256:a402b4279d39c467098866fe83778275580e44742ae06be035a9441f44a4e1bc" -->
<!-- pdac:cite id="UC-WSP-RESOLVE-SSO-NONCOMPLIANCE" digest="sha256:5f38b0e2f9b76ac7379b9911e20594c216f5b1009e0a32722e9edb28b90e6397" -->
<!-- pdac:cite id="TERM-EXTERNAL-SIGN-IN" digest="sha256:6e527bd8033f922ceb45fbe8b9798028a1922a58254935fc13436d2ee1d9c277" -->
