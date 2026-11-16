<!-- pdac-scope: cited -->

## Context

The native candidate has a bounded current snapshot rather than every historical Git ref. The accepted product baseline is already on main, while reviewed directory/person code remains on the historical Contacts branch. The worker must receive that code explicitly.

## Decisions

Use the installed provider seed contract: one immutable commit directly based on its declared base, restricted to safe candidate paths whose preimages still match current source. Configure this seed only for issue 299. The agent must requalify the recovered code, complete its OpenSpec tasks and produce current evidence.

Prefix the existing .NET check argument arrays with env and per-attempt workspace obj cache paths. This directory is already ignored, is owned by the disposable candidate and survives across that candidate’s check containers; it introduces no shared cache or production secret. Keep the same checks, filters and per-PR browser selection.

The repository GitHub Actions setting now permits PR creation while retaining read-only default workflow permissions. This does not itself create a PR, approve a candidate or authorize a merge. The failed publication created branch chore/299-lighthouse-implement-e6f829f62edc at 6ee88ccf46d0e625a09129d1877b398080f861cc and no PR; preserve it as evidence rather than treating its plan-only candidate as implemented.

## Verification

Validate the exact seed with the installed provider loader against current source. Restore the real solution inside the pinned implementation image with the same private 2 GiB tmpfs and fresh workspace caches. Run repository OpenSpec, ProductShape, citation, Markdown and policy validation. Observe the next native worker and actual PR publication independently.

<!-- pdac:cite id="FR-CON-002" digest="sha256:050518c8af11e5d7f1e90fa0e7e4f71e949c4923ac5e8d8335697cd4c3fa235b" -->
