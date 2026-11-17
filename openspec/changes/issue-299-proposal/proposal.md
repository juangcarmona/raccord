<!-- pdac-scope: cited -->

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

<!-- pdac:cite id="FR-CON-001" digest="sha256:a7e73bb8c5c496226d8074b5fda85a5f974703596c57ee0f7b10c49eba3e040e" -->
<!-- pdac:cite id="FR-CON-002" digest="sha256:050518c8af11e5d7f1e90fa0e7e4f71e949c4923ac5e8d8335697cd4c3fa235b" -->
<!-- pdac:cite id="BR-CON-CONTACT-PERSON-DESIGNATIONS" digest="sha256:b80ca1deda21cf11609d89248e6aa165275057b80e02519feb6ea2cbb967c033" -->
<!-- pdac:cite id="BR-PRJ-CONTACT-PERSON-NOT-MOVABLE" digest="sha256:de5fef396fa13aa8f696c139c356dc719be7dde97995a7db49bf9c8d15111c8b" -->
<!-- pdac:cite id="BR-PRJ-INVOICE-RECIPIENT" digest="sha256:5bd94b44ca62c6c328b9dcfee904b108a0543af7e5a048e4637c7daca3816e52" -->
<!-- pdac:cite id="SB-CON-CHANGE-DEFAULT-CONTACT-PERSON" digest="sha256:5cd239965aa4275900b2daef3a6f6d520dbe10391c8f83e0aa8ae3c517d2d8ea" -->
<!-- pdac:cite id="SB-CON-REASSIGN-ADMINISTRATIVE-CONTACT-PERSON" digest="sha256:0513919df0cd30d0d16fc75d98b69f1c76d8824d19e38a292620a9c0c661abb4" -->
<!-- pdac:cite id="SB-CON-CONTACT-PERSON-PRESERVES-PARTY-SNAPSHOT" digest="sha256:93235b2bbd6b5add5a7f27ceed023329ab7ca3af5b2c4a2e4f3178d997095124" -->
