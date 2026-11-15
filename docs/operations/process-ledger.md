# Process ledger

- 2026-11-15: After applying a Product Change, keep its `change.md` decision record and remove the archived `proposed/` copies in the same reviewed change. Check links to those copies, then run ProductShape validation and OpenSpec citation verification. The accepted model remains in `docs/product/model/`; Git history retains earlier artifact versions.
- 2026-11-15: When adding a terminal domain state, inspect rules and use cases that describe the old state as "not finalized". Narrow their conditions where necessary, and acknowledge unchanged downstream artifacts with ProductShape's digest-pinned `unaffected` ledger before applying the change.
