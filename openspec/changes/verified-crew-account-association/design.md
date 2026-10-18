<!-- pdac-scope: cited -->

## Context

An account represents a person's authentication identity. A crew record represents their workspace role, planning history and crew data. Existing invitation behavior supports a new profile for a crew record, while verified-email rules ensure profile uniqueness. Those behaviors do not establish that an existing profile and an existing crew record represent the same person.

## Goals / Non-Goals

**Goals:** Define a tenant-scoped, auditable rule for associating existing records; preserve each record and its history; refuse ambiguous or conflicting matches; state how email changes, unlinking and deactivation behave.

**Non-Goals:** Implement the flow, decide a person's identity from email equality, collect identity documents, merge records, alter authentication, make compliance claims or alter production data.

## Decisions

- Add a Workspace use case and business rule with structured scenarios grouped under the Workspace bounded context.
- Recommend two-sided confirmation: an authorized planner selects one existing crew record and sends a time-limited invitation to its current email; the authenticated profile holder reviews and accepts that specific association.
- Treat that recommendation as a proposed product decision, not an approved rule. The Owner must decide whether it provides sufficient identity evidence and whether a second planner approval is required.
- Refuse to create a link when there is no plausible record, multiple plausible records, duplicate email ambiguity, or either record is already linked elsewhere. Require planner resolution without revealing data from another workspace.
- Require authorized, reasoned unlinking; retain both records and audit history.
- Keep profile email propagation governed by BR-WSP-EMAIL-CHANGE-PROPAGATES without changing the association. Deactivation blocks login but preserves the existing link and crew history.
- Apply the approved Product Change only after the Owner resolves its open questions. Track subsequent code, API, persistence and browser journey in a separate implementation issue.

## Risks / Trade-offs

Two-sided confirmation is understandable and avoids identity documents, but may not be sufficient for every threat model. A second planner approval increases assurance but adds operational friction. These are explicit Owner decisions and must not be hidden in implementation details.

## Verification Plan

- Validate the Product Change overlay with ProductShape and resolve all diagnostics before approval.
- Review the proposed Workspace and Crew artifacts and verify every association, rejection, unlink and lifecycle scenario.
- Verify that existing email uniqueness, invitation expiry, crew login permission, profile ownership, tenant isolation and audit expectations remain consistent.
- After approval and apply, open a separate runtime issue. Its verification must include authorization, tenant isolation, audit events, ambiguity, conflicts, changed email, unlinking and deactivation, plus a browser journey selected for CI.
