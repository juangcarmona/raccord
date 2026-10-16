<!-- pdac-scope: none reason="This follow-up changes CI test preparation and workflow guards only; it has no product behavior or product-model requirements and is authorized under issue #261 as a correction to the Owner-requested #247 delivery." -->

## Tasks

- [x] Add a public homepage build step to engineering-site E2E before compiling and running the suite.
- [x] Make the workflow-routing guard verify that `ci.yml` and `engineering-site.yml` build the homepage before their E2E build.
- [x] Record the observed failure and durable guard in the process ledger.
- [x] Pass the complete CI run and the post-merge engineering E2E run on the fix.
