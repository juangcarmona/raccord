<!-- pdac-scope: cited -->

## Locale routes

The root page remains Spanish as the no-script fallback. A small root-only browser-language resolver redirects to `/es/` or `/en/` using the first supported browser preference; if neither Spanish nor English appears, it uses Spanish. Explicit locale routes do not redirect, so the selector choice stays in effect.

## Translation and search metadata

Each locale is a complete static page with its own `lang`, title, description, canonical URL and reciprocal Spanish/English `hreflang` links. The visible selector uses language names and marks the current locale. Shared screenshots and styles are served from the static output root.

## Review and expansion

Spanish is the source copy. A fluent reviewer checks each English update for meaning and readability. Product, security and legal statements require the same review in both languages. All locale edits ship together, and build tests check route completeness and key disclosures. Additional official EU languages are added only when a fluent reviewer can review the full page and future claim updates.

<!-- pdac:cite id="QR-LOCALIZATION" digest="sha256:defb01ff947a5d42bdc3b6b2fe7d58c7d3aead62e91f759c0118f40749654ddf" -->
<!-- pdac:cite id="CON-NO-COMPLIANCE-CLAIM" digest="sha256:a5035db4daaf71d39067c492306313089d3916634c536c81b4b91e41c7452492" -->
