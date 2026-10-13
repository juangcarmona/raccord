# Design

- Configure the OpenID Connect handler only when `Raccord:Authentication:Oidc:Enabled` is true; required values are authority, client id and client secret.
- Use ASP.NET Core Identity external-login records so a linked provider key resolves to an existing local user.
- Expose a browser challenge and callback under `/api/auth/external`. The callback links the provider to the current authenticated account, or signs in an already-linked account. It returns unauthorized when no local account is linked.
- Store role SSO enforcement as an Identity role claim. Role responses expose the setting and role writes accept `RequireExternalSignIn`.
- Check the role claim after a successful password verification and immediately sign out when the role requires external sign-in.

This keeps the core domain independent of the provider and follows ADR-0021's same-origin cookie decision. The external provider remains disabled by default.

<!-- pdac-scope: cited -->
<!-- pdac:cite id="UC-WSP-SIGN-IN" digest="sha256:d3518a52b6a4950d1caa28d4ba04451e45ab0949d3cba65dffdbc1ebba0a9221" -->
<!-- pdac:cite id="UC-WSP-ENFORCE-SSO" digest="sha256:a402b4279d39c467098866fe83778275580e44742ae06be035a9441f44a4e1bc" -->
<!-- pdac:cite id="UC-WSP-RESOLVE-SSO-NONCOMPLIANCE" digest="sha256:5f38b0e2f9b76ac7379b9911e20594c216f5b1009e0a32722e9edb28b90e6397" -->
<!-- pdac:cite id="TERM-EXTERNAL-SIGN-IN" digest="sha256:6e527bd8033f922ceb45bfe8b9798028a1922a58254935fc13436d2ee1d9c277" -->
