<!-- pdac-scope: none reason="The design reuses an existing responsive public-site layout class and adds static build coverage; it defines no product behavior." -->

# Design: Public homepage guide-card layout

The homepage manual links already use the `guide-card` component and the styles for `guide-grid` already provide a two-column desktop layout and a single-column mobile layout. Pass `guide-grid` to the existing `cards` renderer for this group so it uses the same layout contract as the manuals index.

The static-site build test reads each generated homepage and asserts that the manuals section contains the grid wrapper and exactly three guide cards. This covers both locales through the existing shared route test setup without adding a dependency or a new layout implementation.
