<!-- pdac-scope: cited -->

## Context

The product now requires sign-in for application use. A new installation creates its first administrator through a one-time anonymous endpoint; this is distinct from public user registration. The canonical guide must explain that boundary without implying that an uninitialized installation is ready for exposure.

## Decisions

- Keep `docs/operations/self-hosting.md` as the source of truth for Docker Compose installation and operations.
- Tell operators to create the first administrator only from a trusted network, then sign in and configure the issuing company.
- Keep TLS termination, database port isolation, backup and upgrade instructions in the same guide.
- Keep public-site calls to action linked to the canonical guide and the repository README; do not invent support channels.

## Risks

The one-time setup route is unauthenticated before the first account exists. Operators must complete setup on a trusted network before broader access. The documented command and password policy must continue to match the implementation.

<!-- pdac:cite id="ACT-INSTALLATION-ADMINISTRATOR" digest="sha256:2a214515187fede3e5d9b093a24cb298d21db07c0300f2772dfcccf1d8ff0815" -->
<!-- pdac:cite id="UC-WSP-SIGN-IN" digest="sha256:d3518a52b6a4950d1caa28d4ba04451e45ab0949d3cba65dffdbc1ebba0a9221" -->
