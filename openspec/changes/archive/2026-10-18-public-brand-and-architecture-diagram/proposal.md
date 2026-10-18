<!-- pdac-scope: none reason="This corrects the public site's visual presentation and accessibility without changing product behavior or product-model semantics." -->

# Show the brand and clarify the public architecture diagram

## Why

The architecture page currently places PostgreSQL in the narrow grid column used by the arrow, causing its label to overlap the document-storage node. The public header also omits the existing Raccord mark.

## What Changes

- Show the existing Raccord mark alongside the wordmark in the public-site header and language handoff.
- Rebuild the architecture diagram grid so the browser, application, database and document storage occupy distinct responsive areas.
- Verify generated asset links in both locales and visually inspect desktop and mobile layouts in a browser.

## Capabilities

No product capabilities or product semantics change.

## Impact

Only the public static site generator, its styles, generated brand asset and public-site tests change. No runtime dependency or production deployment is introduced.
