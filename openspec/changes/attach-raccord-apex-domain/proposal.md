<!-- pdac-scope: cited -->

## Why

The public homepage is deployed at `raccord.pages.dev`, while the purchased `raccord.dev` apex domain is not yet confirmed as attached. The repository's Wrangler project name also needs to match the deployed Pages project.

## What Changes

- Align the Wrangler Pages project name and build assertion with `raccord.pages.dev`.
- Document the Pages Git integration settings, domain association sequence, expected CNAME target, DNS prerequisites, HTTPS checks and rollback steps.
- Keep DNS changes and production publication as Owner-approved operations; do not expose an application or API at the public domain.

## Capabilities

### Modified Capabilities

- `public-site-deployment`: the static project has a reviewed, repeatable custom-domain setup and rollback path.

## Product definition impact

This changes only static-site hosting configuration and operator documentation. It adds no Raccord application behavior or product claims.

<!-- pdac:cite id="ACT-OPERATIONS-COORDINATOR" digest="sha256:c6210b5da8b0aede8884fa0c04d95492b6f47095f239677bd7d5c647040798e7" -->
<!-- pdac:cite id="CON-NO-COMPLIANCE-CLAIM" digest="sha256:a5035db4daaf71d39067c492306313089d3916634c536c81b4b91e41c7452492" -->
