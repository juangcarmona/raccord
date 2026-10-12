---
name: local-identity-with-cookie-sessions-and-scoped-permissions
description: Raccord authenticates users with ASP.NET Core Identity and a same-origin cookie, optionally through an external OpenID Connect provider, and authorizes with editable roles made of module permissions with scopes.
status: Accepted
date: 2026-10-12
deciders: ["Juan G. Carmona"]
tags: ["security", "authentication", "authorization", "workspace"]
---

# ADR-0021: Authenticate with local Identity and cookie sessions, authorize with scoped role permissions

## Context

Raccord has no authentication. `HttpCurrentActor` returns "unauthenticated", and arc42 §11 rates this as risk R-1. ADR-0012 fixes one organization per installation and still requires authentication and roles inside it. Arc42 §11 D-9 leaves the identity provider open.

The product model (BC-WORKSPACE) requires:

- **Sign-in:** local user accounts with passwords, optionally an external sign-in through a Microsoft or Google account, or both on one profile (`term-external-sign-in`).
- **Roles:** four predefined roles (Admin, Office, Warehouse, Crew) that administrators can edit, plus custom roles. Every user has exactly one role (`term-predefined-role`, `term-user-role`).
- **Scoped permissions:** a role is a set of permissions, several of which are scoped rather than boolean. Examples: visible projects "own or all", changing projects "none, own or all" (`term-role-permission`).
- **Other ways in:** the same rights apply in the web product, the mobile web app and calendar feeds. A public API with tokens is planned (`term-api-token`).

Raccord is self-hosted with Docker Compose by a single company (ADR-0013) and serves its SPA from the same origin as the API (ADR-0011). The sibling projects use Keycloak behind a BFF, and Entra ID with tokens issued by the API. Both suit their context and both add an identity dependency to every installation.

## Decision

We will authenticate with ASP.NET Core Identity and an HttpOnly cookie on the same origin. Authorization will use editable roles made of module permissions that can carry a scope.

- **Identity store:**
    - ASP.NET Core Identity (`Microsoft.AspNetCore.Identity.EntityFrameworkCore`), with its tables in the `workspace` schema owned by the Workspace module (ADR-0019).
    - Default password hashing, lockout after repeated failures, and security-stamp validation so that a role change or deactivation ends existing sessions.
- **Session:**
    - An application cookie that is HttpOnly and `Secure`, uses `SameSite=Lax`, and has a sliding expiry.
    - The API answers 401/403 and never redirects.
    - State-changing API requests must carry an antiforgery header that the SPA reads from a non-HttpOnly token cookie.
    - Data-protection keys are persisted in the database, so sessions survive container restarts and work across replicas.
- **External sign-in:**
    - An optional OpenID Connect provider, configured per installation (`Authentication:Oidc:*`) and disabled by default.
    - An external login links to an existing local account. It never creates an account silently unless the administrator enables that.
    - A profile may have a password, an external login, or both.
- **First administrator:** created by the setup flow of journey `jrn-wsp-set-up-workspace`. That flow decides between a first-run page and a `migrate --create-admin` command.
- **Permissions:**
    - Each module declares its permissions as keys `<module>:<action>`, with an optional scope (`none`, `own`, `all`) where the product model defines one.
    - Roles, and their value for each permission, are data in the Workspace module. The four predefined roles are seeded and editable.
    - Endpoints declare the permission they need (`RequirePermission("invoicing:issue")`).
    - Handlers that depend on scope ask the current user for it (`ICurrentUser.ScopeOf(...)`) and filter accordingly.
    - The fallback policy requires an authenticated user, so a new endpoint fails closed.
- **Current user:**
    - `ICurrentUser` (SharedKernel) replaces `ICurrentActor`. It exposes the user id, display name, role and permissions.
    - Audit events record the user id and name, so they no longer say "unauthenticated".
- **Calendar feeds and API tokens** are out of scope. Each will get its own authentication scheme, selected by a policy scheme, when its journey is implemented.

## Consequences

- **Positive:**
    - No additional container or identity service per installation.
    - Tokens never reach the browser's JavaScript.
    - Same-origin cookies need no CORS.
    - Roles and scopes follow the product model directly.
    - Closes risk R-1 once implemented.
- **Negative:**
    - Raccord owns password storage, lockout and account recovery. Recovery needs e-mail, which does not exist yet; until then an administrator resets passwords.
    - No server-side single sign-out across external providers.
    - Scoped permissions need handler logic and tests per scoped action.
    - Antiforgery adds a header the SPA client must send.
- **Neutral:**
    - Adds one production dependency (`Microsoft.AspNetCore.Identity.EntityFrameworkCore`), justified here.
    - Arc42 §8 (security) and §11 (R-1, D-9) must be updated on implementation.
    - ADR-0012 is unchanged.

## Alternatives considered

- **Keycloak behind a BFF:** rejected because every self-hosted installation would have to run and upgrade an identity server and its realm configuration for a single company.
- **Entra ID with tokens issued by the API:** rejected because it ties sign-in to one cloud directory, while Raccord's users are independent companies that need local accounts.
- **Bearer tokens in the SPA:** rejected because tokens stored in the browser are exposed to XSS and gain nothing on a same-origin deployment.
- **Static role→permission table in code:** rejected because the product model requires editable and custom roles.

## References

- Related: ADR-0011, ADR-0012, ADR-0013, ADR-0018, ADR-0019, ADR-0022
- Product model: `docs/product/model/domain/bounded-contexts/bc-workspace.md`, `docs/product/model/domain/terms/term-role-permission.md`, `term-predefined-role.md`, `term-user-role.md`, `term-external-sign-in.md`
- Journey: `docs/product/model/journeys/jrn-wsp-set-up-workspace.md`
- Risk R-1 and debt D-9 in `docs/architecture/arc42/11-risks-and-technical-debt.md`
