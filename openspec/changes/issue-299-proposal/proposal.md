# Proposal

## Why

Operations coordinators need to maintain a contact directory and the people associated with each contact. The approved Contacts baseline now defines these behaviors, but the current runtime has no Contacts API or editor, so the approved work cannot yet be used.

## What Changes

- Restore the approved contact directory prerequisite and add contact-person list, create, read, and edit behavior.
- Allow supported contact details and default/administrative designations, enforcing zero-or-one of each per contact with atomic reassignment.
- Keep people attached to their original contact, respect the installation's existing data boundary, and preserve project associations and issued-invoice snapshots.
- Add authenticated API, persistence, editor UI, domain/persistence/Web tests, and a selected Contacts browser journey.

## Capabilities

### New Capabilities

- `contact-person-management`: maintain contacts and their people/designations without changing project associations or issued-invoice snapshots.

### Modified Capabilities

None.

## Impact

Contacts module contracts and runtime, persistence and migrations, authorization/audit integration, Web API, frontend contact editor, and domain, persistence, API, frontend and browser tests. No project contact-selection or invoice-delivery behavior is introduced.
