<!-- pdac-scope: cited -->

# Spec Delta

## Purpose

Provides authorized operations users with an installation-isolated directory of contacts and their affiliated people. It maintains supported designations without changing project associations or issued-invoice party snapshots.

## ADDED Requirements

### Requirement: Maintain contact persons under a contact

The system MUST let an authorized user list, create, read, and edit contact persons under an existing contact, including a required name and supported email and phone details. Each person MUST remain attached to its original contact.

#### Scenario: Create and edit a person

- **WHEN** an authorized user creates a person with valid details under an existing contact and later edits that person
- **THEN** the person is listed and readable under that contact with the saved details

#### Scenario: Reject invalid details

- **WHEN** a user submits a missing required name or invalid contact detail
- **THEN** the request is rejected and no invalid record or partial update is saved

#### Scenario: Do not move a person between contacts

- **WHEN** an update attempts to change a person's parent contact
- **THEN** the request is rejected and the person remains attached to its original contact

#### Scenario: Enforce authorization and installation ownership

- **WHEN** a user without Contacts permission attempts to read or change a person's record, or a record is accessed outside its owning installation
- **THEN** access is denied and no data changes

### Requirement: Enforce unique default and administrative designations

A contact MUST have at most one default person and at most one administrative person. The same person MAY hold both designations. Reassigning either designation MUST clear its previous holder atomically, and a rejected or conflicting change MUST NOT leave a partial reassignment.

#### Scenario: Reassign the default designation

- **WHEN** an authorized user designates a different person as default
- **THEN** the new person is the sole default and the former default remains under the contact without that designation

#### Scenario: Reassign the administrative designation independently

- **WHEN** an authorized user designates a different person as administrative
- **THEN** the new person is the sole administrative person and any default designation remains unchanged

#### Scenario: Preserve designation integrity on conflict

- **WHEN** concurrent or invalid designation changes conflict
- **THEN** no contact is left with duplicate designations or a partially completed reassignment

### Requirement: Preserve project and issued-invoice history

Contact-person writes MUST NOT change project contact associations or immutable issued-invoice party snapshots. A person's designations MUST NOT select a project contact or determine invoice delivery.

#### Scenario: Edit contact people with existing project and invoice history

- **WHEN** an authorized user creates or edits a person or reassigns a designation for a contact with existing project and issued-invoice records
- **THEN** those project associations and invoice party snapshots remain unchanged

<!-- pdac:cite id="FR-CON-001" digest="sha256:a7e73bb8c5c496226d8074b5fda85a5f974703596c57ee0f7b10c49eba3e040e" -->
<!-- pdac:cite id="FR-CON-002" digest="sha256:050518c8af11e5d7f1e90fa0e7e4f71e949c4923ac5e8d8335697cd4c3fa235b" -->
<!-- pdac:cite id="BR-CON-CONTACT-PERSON-DESIGNATIONS" digest="sha256:b80ca1deda21cf11609d89248e6aa165275057b80e02519feb6ea2cbb967c033" -->
<!-- pdac:cite id="BR-PRJ-CONTACT-PERSON-NOT-MOVABLE" digest="sha256:de5fef396fa13aa8f696c139c356dc719be7dde97995a7db49bf9c8d15111c8b" -->
<!-- pdac:cite id="BR-PRJ-INVOICE-RECIPIENT" digest="sha256:5bd94b44ca62c6c328b9dcfee904b108a0543af7e5a048e4637c7daca3816e52" -->
<!-- pdac:cite id="SB-CON-CHANGE-DEFAULT-CONTACT-PERSON" digest="sha256:5cd239965aa4275900b2daef3a6f6d520dbe10391c8f83e0aa8ae3c517d2d8ea" -->
<!-- pdac:cite id="SB-CON-REASSIGN-ADMINISTRATIVE-CONTACT-PERSON" digest="sha256:0513919df0cd30d0d16fc75d98b69f1c76d8824d19e38a292620a9c0c661abb4" -->
<!-- pdac:cite id="SB-CON-CONTACT-PERSON-PRESERVES-PARTY-SNAPSHOT" digest="sha256:93235b2bbd6b5add5a7f27ceed023329ab7ca3af5b2c4a2e4f3178d997095124" -->
