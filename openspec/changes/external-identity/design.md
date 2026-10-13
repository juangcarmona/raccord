# Design

- Configure the OpenID Connect handler only when `Raccord:Authentication:Oidc:Enabled` is true; required values are authority, client id and client secret.
- Use ASP.NET Core Identity external-login records so a linked provider key resolves to an existing local user.
- Expose a browser challenge and callback under `/api/auth/external`. The callback links the provider to the current authenticated account, or signs in an already-linked account. It returns unauthorized when no local account is linked.
- Store role SSO enforcement as an Identity role claim. Role responses expose the setting and role writes accept `RequireExternalSignIn`.
- Check the role claim after a successful password verification and immediately sign out when the role requires external sign-in.

This keeps the core domain independent of the provider and follows ADR-0021's same-origin cookie decision. The external provider remains disabled by default.

<!-- pdac-scope: cited -->
