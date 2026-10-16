<!-- pdac-scope: cited -->

## Context

The Owner reports that the public site is available at `raccord.pages.dev`. The exact DNS zone, nameservers and current deployment type must be checked in Cloudflare before the apex domain is attached. The repo can document the sequence but cannot infer account-specific nameserver values or safely replace existing DNS records.

## Decisions

- Use the existing Pages project `raccord` and the Cloudflare custom-domain association flow for `raccord.dev`.
- Expect a proxied apex CNAME from `@` to `raccord.pages.dev`; let Cloudflare create or confirm the account-specific DNS record and certificate.
- Keep production branch publication disabled until the Owner approves the preview and the exact DNS state.
- Preserve a DNS snapshot and rollback by detaching the Pages custom domain and restoring only changed DNS or nameserver settings.

## Risks

Apex-domain activation requires the zone to be active in the same Cloudflare account as the Pages project. If the current project is Direct Upload, Cloudflare does not support converting it to Git integration; deployment type must be checked before creating or replacing a project. Nameserver changes can affect every record in the zone.

<!-- pdac:cite id="ACT-OPERATIONS-COORDINATOR" digest="sha256:c6210b5da8b0aede8884fa0c04d95492b6f47095f239677bd7d5c647040798e7" -->
<!-- pdac:cite id="CON-NO-COMPLIANCE-CLAIM" digest="sha256:a5035db4daaf71d39067c492306313089d3916634c536c81b4b91e41c7452492" -->
