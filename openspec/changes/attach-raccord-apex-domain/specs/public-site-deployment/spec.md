<!-- pdac-scope: cited -->

## ADDED Requirements

### Requirement: raccord.dev serves only the approved static public site

The Cloudflare Pages project SHALL bind `raccord.dev` to the reviewed static public-site output after the Owner approves the exact DNS and production-publication actions. The setup record SHALL identify the Pages project, apex DNS prerequisites, expected CNAME target, HTTPS verification and rollback path. The apex domain SHALL NOT route to the Raccord application or API.

#### Scenario: A visitor opens the approved public domain

- **WHEN** the Owner-approved Pages custom domain is active
- **THEN** `https://raccord.dev` serves the approved static homepage with working locale links and no application instance or account data

#### Scenario: Domain activation must be rolled back

- **WHEN** Pages activation or HTTPS verification fails
- **THEN** the operator can detach the custom domain and restore the recorded prior DNS or nameserver state while retaining the `raccord.pages.dev` hostname

<!-- pdac:cite id="ACT-OPERATIONS-COORDINATOR" digest="sha256:c6210b5da8b0aede8884fa0c04d95492b6f47095f239677bd7d5c647040798e7" -->
<!-- pdac:cite id="CON-NO-COMPLIANCE-CLAIM" digest="sha256:a5035db4daaf71d39067c492306313089d3916634c536c81b4b91e41c7452492" -->
