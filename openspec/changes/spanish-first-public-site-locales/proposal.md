<!-- pdac-scope: cited -->

## Why

Raccord is first aimed at teams in Spain, so Spanish must remain the default public-site language. Visitors who prefer another language also need complete page content and matching security and maturity disclosures; a selector that leads to partially translated pages would be misleading.

## What Changes

- Keep complete Spanish and English pages at `/es/` and `/en/`, with the root route choosing the first supported browser language and falling back to Spanish.
- Add an accessible language selector and retain explicit locale URLs when visitors switch languages.
- Set locale-specific page language, canonical URLs and `hreflang` declarations.
- Test that every shipped locale has complete navigation and content and document how translations are reviewed and extended to the 24 official EU languages.

## Capabilities

### Modified Capabilities

- `public-capability-overview`: every visitor can read the full project overview and safety disclosures in Spanish or English.

## Product definition impact

This translates current public information without changing application behavior. Product, security and legal claims retain the same meaning in both locales.

<!-- pdac:cite id="QR-LOCALIZATION" digest="sha256:defb01ff947a5d42bdc3b6b2fe7d58c7d3aead62e91f759c0118f40749654ddf" -->
<!-- pdac:cite id="CON-NO-COMPLIANCE-CLAIM" digest="sha256:a5035db4daaf71d39067c492306313089d3916634c536c81b4b91e41c7452492" -->
