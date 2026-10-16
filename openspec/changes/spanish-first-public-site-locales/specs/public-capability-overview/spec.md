<!-- pdac-scope: cited -->

## ADDED Requirements

### Requirement: Public-site locales are complete and consistent

The public site SHALL provide complete Spanish and English pages with explicit locale routes, an accessible language selector and equivalent product, security and maturity information. The root route SHALL use the first supported browser language and fall back to Spanish.

#### Scenario: A visitor's browser prefers English

- **WHEN** an English-preferring browser opens the root route
- **THEN** the visitor reaches the English page and can switch directly to Spanish

#### Scenario: A visitor's browser uses an unsupported language

- **WHEN** a browser with no supported language opens the root route
- **THEN** the visitor sees Spanish, with a visible option to switch to English

#### Scenario: Search engines inspect a locale page

- **WHEN** a crawler opens the Spanish or English route
- **THEN** the page declares its own language, canonical URL and alternate Spanish and English URLs

<!-- pdac:cite id="QR-LOCALIZATION" digest="sha256:defb01ff947a5d42bdc3b6b2fe7d58c7d3aead62e91f759c0118f40749654ddf" -->
<!-- pdac:cite id="CON-NO-COMPLIANCE-CLAIM" digest="sha256:a5035db4daaf71d39067c492306313089d3916634c536c81b4b91e41c7452492" -->
