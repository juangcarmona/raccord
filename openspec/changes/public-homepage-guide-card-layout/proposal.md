<!-- pdac-scope: none reason="This corrects the responsive layout of existing public-site guide links and adds static rendering coverage; it changes no product capabilities or domain semantics." -->

# Restore the public homepage guide-card layout

## Why

The three guide links on the public homepage are rendered with the `card-grid` class, which has no stylesheet rule. The links therefore lose their intended card layout and can appear as clipped, overlapping fragments.

## What Changes

- Render the homepage guide links with the existing responsive `guide-grid` layout used by the manuals index.
- Add a generated-page regression check for Spanish and English homepages, including all three guide cards.

## Product definition impact

No ProductShape artifacts are affected. This is a presentation correction for existing public links; it changes no product behavior or domain semantics.

## Scope

Only the public static site's homepage rendering, its build test, and the repository's change record are affected.
