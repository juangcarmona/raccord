<!-- pdac-scope: none reason="This follow-up changes CI test preparation and workflow guards only; it has no product behavior or product-model requirements and is authorized under issue #261 as a correction to the Owner-requested #247 delivery." -->

## Design

Add the same static homepage build step to the engineering-site E2E job before building the .NET E2E suite. Extend the existing workflow-routing script to verify the public homepage build occurs before the E2E build in both `ci.yml` and `engineering-site.yml`, so either lane fails early if the fixture input is omitted.

The workflow fix restores the post-merge journey. The deterministic guard hardens the process by checking both E2E entry points on every workflow validation run.
