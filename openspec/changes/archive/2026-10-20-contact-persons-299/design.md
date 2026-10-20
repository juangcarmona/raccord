<!-- pdac-scope: cited -->

## Context

The Contacts module currently owns a company/private-person contact and exposes it through module contracts, an application directory and host-mapped API endpoints. The existing editor is the only contact maintenance screen. Contact persons therefore need a parent-scoped management surface that composes with the current directory without changing party identity or downstream project and invoice behavior.

<!-- pdac:cite id="BC-CONTACTS" digest="sha256:bdadd84cb901027c8630d88c714ba8c52bcecdf383345d35a2631c7fa8b69c85" -->
<!-- pdac:cite id="FR-CON-002" digest="sha256:d44280cb004062fbc3bd34228233dcfa03b2b6e51ee64b4f717652351f721feb" -->

## Goals / Non-Goals

**Goals:** Store and maintain a contact's people and designations; enforce that each person stays under its original contact; ensure designation updates remain consistent when concurrent or rejected.

**Non-Goals:** Delete contact persons, select a person for projects or invoices, deliver messages, add a separate service or production dependency, or change the immutable invoice party snapshot.

## Decisions

1. Model contact persons as Contacts-owned records with a required parent contact ID and a narrow domain contract for supported contact details and designations; do not represent them as crew members or user accounts.
2. Keep person creation and editing behind Contacts use cases and module persistence ports. The host maps nested contact-person endpoints and reuses the existing Contacts authorization policies.
3. Persist default and administrative designations as contact-scoped state with one default and one administrative person at most per contact. A default reassignment runs in one unit of work, clears the previous default and sets the new one atomically; database constraints protect the invariant if competing writes race.
4. Treat the parent contact ID as immutable in edit requests. A request that supplies or attempts a different parent is rejected without changing the person or any designation.
5. Add management controls to the existing contact edit screen rather than introducing a new top-level route. Keep all strings localized in English and Spanish and consume only generated API contracts.
6. Prove the user path with one browser journey that creates two people, changes default/administrative designations, checks the previous default was cleared, and verifies the contact's project association and issued invoice recipient remain unchanged.

## Risks / Trade-offs

- Unique partial indexes and concurrent default reassignment must be designed for the actual database provider and verified with integration tests; an in-memory-only invariant is insufficient.
- The existing contact editor saves the parent contact separately from nested data. Person edits must not resubmit stale parent fields or overwrite contact changes.
- Updating the main Contacts spec and ProductShape citations is required for the accepted behavior and must stay in the same issue branch.
