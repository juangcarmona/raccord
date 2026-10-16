<!-- pdac-scope: none reason="This follow-up changes CI test preparation and workflow guards only; it has no product behavior or product-model requirements and is authorized under issue #261 as a correction to the Owner-requested #247 delivery." -->

## Change: Prepare the static homepage for every engineering E2E run

### Motivation

The post-merge engineering E2E job selected the new public-homepage browser journey but did not build `src/public-site/dist/`. The journey failed with `ERR_FILE_NOT_FOUND` even though pull-request CI passed because its E2E job builds the homepage first.

### Scope

Build the public homepage before the engineering E2E suite, assert that both CI entry points build it before the suite, and record the process correction.

### Out of scope

Homepage content, application behavior, Cloudflare account setup, production publication, and DNS.
