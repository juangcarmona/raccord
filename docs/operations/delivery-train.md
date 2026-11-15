# Serial delivery train

The owner approved the existing written product intent and proposals on 2026-11-15 and requested unattended PR delivery for every remaining epic. [Issue #437](https://github.com/juangcarmona/raccord/issues/437) records the authorization, complete queue, dependencies and current blocker. The committed [inventory](delivery-train.json) binds the approved source issue bodies by SHA-256 at source revision `38366dc88bccffe5dfa4797cb4d976c423b56733`; subsequent issue updates record that approval and add the queue hold.

## Execution order

Run one implementation at a time. Recover existing Contacts work first, then Workspace prerequisites, Equipment, Warehouse and Projects, Crew, Documents, Commercial catalogs and quotes, Invoicing, Compliance, multi-organization work, and the verified public inventory. Cross-context prerequisites override numeric epic order: #300 follows #323/#417/#419, #304/#384/#387 follow #310/#311, and #335 follows #329. Nested epics #342 and #24 remain under their existing parents.

The ordered inventory is an operator queue, not a scheduler or a native Lighthouse delivery receipt. `future` holds queued issues until an authenticated controller or owner releases the selected issue. Do not remove all holds or fan out implementations. Before releasing an item, inspect predecessor PRs, accepted product artifacts and the actual delivery base. Requalify historical code and tests; an issue closed on an epic branch does not establish delivery to main.

## Installed bootstrap

All selected workflow sources use the published SDLC 0.24.3 package. The installer enables refinement-to-implementation continuation, Product Change preparation/apply workers and the optional native serial epic controller. The explicit implementation policy runs backend, frontend, ProductShape and the selected browser journeys independently; GitHub readiness also observes the declared existing CI jobs. The candidate cannot change harness/workflow policy. Required repository CI remains unchanged.

Product approval covers the owner's written intent. ProductShape validation and explicit application still run. An unknown contradictory requirement must identify the conflicting artifacts and existing decisions. Legal/protocol evidence remains evidence, not a product-approval checkbox. Native Product Change approval commands and epic merges retain the package's revision-bound authorization contracts; the issue authorization does not pretend that a future review key or exact-head review already exists.

## Activation blocker

The earlier [#301](https://github.com/juangcarmona/raccord/actions/runs/43770193530) and [#302](https://github.com/juangcarmona/raccord/actions/runs/43770207832) refinements failed before agent execution because eager ProductShape reports exceeded their byte bounds. [Lighthouse #706](https://github.com/juangcarmona/lighthouse/pull/706) corrects that path with issue-scoped detail, a complete inventory and explicit unavailable evidence. [Release 43775678986](https://github.com/juangcarmona/lighthouse/actions/runs/43775678986) published Lighthouse 0.13.14 and SDLC 0.24.3 from `1ade56e7e462de6e7e2ff746b90b0a908601b4a0`; registry metadata, package integrity and installed source hashes were checked before adoption. The same 1,889-artifact accepted model now produces 0.6–2.4 MiB scoped bundles for #299/#301/#302. Native worker qualification remains a separate step.

The optional epic controller is installed but remains disabled until a valid current refinement plan and native qualification exist. Set `LIGHTHOUSE_EPIC_CAPACITY=1` when qualifying it. Native acceptance additionally requires the adopted protected evidence branch/profile, eligible CI, retained source-bound receipts and owner decisions for actual candidate heads. No protected evidence branch or merge delegation is provisioned by this bootstrap. Implementation workers can publish PRs after the evidence blocker is fixed; installation alone does not establish a continuously advancing all-epic controller.

## Resume and verify

1. Confirm the adopted SDLC 0.24.3 package and regenerated worker pins; the scoped-evidence correction is installed through the supported upgrade/installer route.
2. Release only the selected queue item and request a fresh refinement against the current source and owner intent.
3. Inspect the validated result and separate implementation continuation; observe the first PR and its real head/base, independent commands and CI.
4. Advance a dependent issue only when its prerequisites are available in its declared base. Respect the existing native controller's per-action authorization contract.
5. Keep credentials, production, releases and deployment in their repository-owned paths. Record failed or unavailable evidence without reporting it as acceptance.
