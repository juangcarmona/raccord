# External identity sign-in

Implement the optional external sign-in slice from ADR-0021. An installation may configure one OpenID Connect provider. External identities are linked to an existing local account by an authenticated user; an external sign-in never creates an account implicitly. Administrators can mark a role as requiring external sign-in, which rejects password sign-in for members of that role.

The slice implements `UC-WSP-SIGN-IN`, `UC-WSP-ENFORCE-SSO`, `UC-WSP-RESOLVE-SSO-NONCOMPLIANCE` and `TERM-EXTERNAL-SIGN-IN`.

<!-- pdac-scope: cited -->
