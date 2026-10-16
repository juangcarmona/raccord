# Public-site localization

Spanish is the source language and default fallback for the public site. English is the first additional locale. The static site keeps complete pages at `/es/` and `/en/`; the root route follows the first supported browser language and otherwise sends visitors to Spanish. Explicit locale routes always stay in the selected language.

## Translation workflow

Keep the Spanish source and every shipped translation in the same pull request. A fluent English reviewer checks meaning, natural wording, navigation, metadata and page completeness. The maintainer reviews all changes to capability, security, maturity, product, tax and legal statements in every shipped language. Do not publish an unreviewed machine translation or allow a page to mix languages.

The build checks locale routes, current page language, reciprocal `hreflang` links, selector targets, required disclosures and browser-language fallback. When copy changes, update both locales and run the public-site build and browser journey.

## Language rollout

- **Phase 1 — shipped:** Spanish and English, with full-page review for both languages.
- **Phase 2 — demand-led:** add French, German, Portuguese, Italian and other requested official EU languages as fluent reviewers become available. Each initial translation is estimated at 4–8 reviewer hours for this page, including claim review and responsive checks.
- **Phase 3 — complete EU coverage:** add the remaining official languages and keep all 24 current. Each copy change is estimated at 1–2 reviewer hours per shipped locale for terminology, product/security claims, metadata and link checks.

The 24 official EU languages are Bulgarian, Croatian, Czech, Danish, Dutch, English, Estonian, Finnish, French, German, Greek, Hungarian, Irish, Italian, Latvian, Lithuanian, Maltese, Polish, Portuguese, Romanian, Slovak, Slovenian, Spanish and Swedish. Estimates are planning ranges; each translation issue records the language, fluent reviewer, pages changed and actual review effort.
