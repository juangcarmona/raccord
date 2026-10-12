# Operations checklist

The engineered guardrails (CI gates, hooks, E2E, security scans) verify the code. This checklist covers the practices that verify the *operation* — the items that don't fit a build pipeline because they need a human decision or a production system. The ZAP baseline scan (`.github/workflows/post-merge.yml`) is the one item that did fit a pipeline and runs automatically after every merge to main.

Work through this checklist on a regular cadence (monthly is a reasonable start; align with the real operational risk, not the calendar alone). Each item says what "done" means — a backup no one has restored is a hypothesis, not a backup.

## Backup and restore

- [ ] Run the restore drill from `backup-and-restore.md` against a scratch target and confirm the restored system boots and serves real data. **Done means:** the drill is executed end-to-end, not read; the restore command's output is checked, and any undocumented step it needed is folded back into the runbook.
- [ ] Confirm automated backups actually ran since the last check (volume snapshots or `pg_dump` history, depending on the deployment target). **Done means:** evidence of successful runs exists for the whole interval, and failed runs have a triaged reason.

## Secrets and access

- [ ] Rotate credentials on the rotation schedule (database, storage account keys, deployment tokens). **Done means:** old credentials are revoked after the new ones are live — a rotated-but-not-revoked credential is still the old credential.
- [ ] Review who has access to the repository, the container registry and the deployed environment; remove leavers and stale deploy keys. **Done means:** the access list is current, and each remaining entry has a justified owner.

## Runbook currency

- [ ] Walk one operational scenario from the runbooks (`self-hosting.md`, `azure-deployment.md`, `backup-and-restore.md`) as written, without improvising. **Done means:** the documented steps work verbatim; where they didn't, the doc is updated in the same change.

## Runtime posture

- [ ] Read the latest nightly post-merge ZAP baseline output and triage every WARN: escalate real findings (add a rule to fail the scan) or record why it's accepted. **Done means:** no untriaged WARN findings older than one cycle.
- [ ] Check dependency and vulnerability gates have been green across the interval — a chronically red gate is a finding about process, not just packages. **Done means:** red gates have either been fixed or explicitly accepted with a reason.
