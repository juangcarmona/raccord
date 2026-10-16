<!-- pdac-scope: none reason="Implementation tasks concern CI and static delivery infrastructure and do not depend on product requirements; this exemption is approved by the Owner for issue #247." -->

## Tasks

- [x] Add and test changed-path classification for non-empty OpenSpec-only, public-site-only, mixed, and empty diffs.
- [x] Route main CI so OpenSpec-only and public-site-only diffs retain Markdown, OpenSpec, ProductShape, and security checks while skipping unrelated application jobs.
- [x] Prevent OpenSpec-only and public-site-only pushes to `main` from starting the post-merge runtime probe.
- [x] Rename the GitHub Pages publisher to `engineering-site.yml` and exclude `src/public-site/**` while retaining application-source triggers.
- [x] Add a separate public-site validation workflow for pull requests and relevant `main` updates; verify the generated output is limited to public-site assets.
- [x] Add Cloudflare Pages project/output configuration and document the repository integration, preview and production roles, and production approval boundary without a stored deployment token.
- [x] Update the architecture and operations records to describe the split publishers and site ownership.
- [x] Pass workflow path-routing fixtures, static-site tests, public-site build checks, Markdown lint, OpenSpec validation, and ProductShape citation verification.
- [x] Obtain a green hosted CI run on the complete change.
