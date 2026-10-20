<!-- pdac-scope: cited -->

## Purpose

Lets authorized operations and billing users maintain the shared contacts that projects and invoices rely on, while protecting historical invoice details and existing customer references.

## ADDED Requirements

### Requirement: Maintain company and private-person contacts with role tags

The system MUST let an authorized user create, view and edit a company or private-person contact with one or more customer, supplier and venue roles. A role is a contact tag and remains distinct from the contact's company/person type. A missing contact number is assigned uniquely; a duplicate number is rejected without changing any contact.

<!-- pdac:cite id="UC-CON-MANAGE-CONTACT-DIRECTORY" digest="sha256:58989f1d3dfd59bb6021eb503753e4f9bccd6bd57f4dc43697dddf535c057901" -->
<!-- pdac:cite id="FR-CON-001" digest="sha256:9b09ae74057045edaba5e51c4d14f287a22093be7537c8867a4fee8e07ccaa5f" -->
<!-- pdac:cite id="BR-PRJ-CONTACT-ROLES-BY-TAG" digest="sha256:d7110eb5318168938b259d3f44fb022f04666454b5112e97d17539861b8604d9" -->
<!-- pdac:cite id="BR-PRJ-CONTACT-NUMBER-ASSIGNMENT" digest="sha256:aa32e12b334cd585d5d946fb961a46d5ddfda39bc4b16320a9c5f0b19d7ce8d7" -->
<!-- pdac:cite id="TERM-CONTACT" digest="sha256:1466e4347ae1ee875b87947164c4bc09a5bce5057ecc590018026d158f9bc885" -->
<!-- pdac:cite id="TERM-CONTACT-TYPE" digest="sha256:696b6459c69ff4d256d49c62bff42571e6a109878d656b82dd5b3be2e52457fc" -->
<!-- pdac:cite id="TERM-CUSTOMER" digest="sha256:e2c324d41aa93283903f70b565b9b4913520ca131a4cb40f71187f8f9b59d03c" -->
<!-- pdac:cite id="TERM-SUPPLIER-CONTACT" digest="sha256:8b634585ba261cec5e5dc78c95546c21830d360eda9f50483da6f6855243f198" -->
<!-- pdac:cite id="TERM-VENUE" digest="sha256:f923ddc29f25c19d1ac3abeb02bc5dfa1b167b9060db0d60ba807cfb809be69c" -->
<!-- pdac:cite id="TERM-CONTACT-NUMBER" digest="sha256:8a4a98e56b529bc22f08b7bd580a515a5d1914e78b30206bf83ec951ae7f8bf3" -->

#### Scenario: Create a company with two roles
- **WHEN** an authorized user saves a company contact with customer and supplier roles and no contact number
- **THEN** the system assigns a unique contact number and the record appears in both role filters

#### Scenario: Reject a duplicate contact number
- **WHEN** an authorized user creates or edits a contact with a number already assigned to another contact
- **THEN** the system reports the conflict and leaves both existing records unchanged

#### Scenario: Deny an unauthorized edit
- **WHEN** an actor without contact-edit permission attempts to change a contact
- **THEN** the system rejects the request and the saved contact remains unchanged

### Requirement: Browse contacts with stable pagination

The system MUST return contacts in a deterministic order and expose a page of results with enough metadata to navigate to adjacent pages. A role filter MUST include contacts carrying that role, including contacts with multiple roles.

<!-- pdac:cite id="UC-CON-MANAGE-CONTACT-DIRECTORY" digest="sha256:58989f1d3dfd59bb6021eb503753e4f9bccd6bd57f4dc43697dddf535c057901" -->
<!-- pdac:cite id="FR-CON-001" digest="sha256:9b09ae74057045edaba5e51c4d14f287a22093be7537c8867a4fee8e07ccaa5f" -->
<!-- pdac:cite id="BC-CONTACTS" digest="sha256:b63537573b968a16a585370ae0e4a98ec688de6e31e42e8b7e2dad5c89d3f474" -->

#### Scenario: Navigate to another page
- **WHEN** a user advances from a non-final page of contacts
- **THEN** the next page shows the next ordered records with no duplicates or gaps in an unchanged result set

#### Scenario: Find a venue on a later page
- **WHEN** a user filters the directory by venue and advances to the page containing a private-person venue
- **THEN** the venue is visible and can be opened from that page

### Requirement: Preserve issued invoice party snapshots

The system MUST use current contact details for future invoice issuance and MUST preserve the recipient details stored in every previously issued invoice when a customer contact is edited.

<!-- pdac:cite id="BR-PARTY-SNAPSHOT" digest="sha256:5ac10d9c4ab5f68acd5a18213fe4c9c0d50786549a1d94c760409214b13355f7" -->
<!-- pdac:cite id="FR-CON-001" digest="sha256:9b09ae74057045edaba5e51c4d14f287a22093be7537c8867a4fee8e07ccaa5f" -->

#### Scenario: Edit a customer after invoice issue
- **WHEN** a user changes a customer's legal name or address after an invoice has been issued to that customer
- **THEN** the existing invoice continues to show its issue-time recipient details and a future invoice uses the changed contact details

### Requirement: Preserve existing customer records and consumers

The system MUST retain existing customer identifiers and their project and invoice associations when adding contact type, role and number data. Existing customer API consumers MUST continue to resolve customer-role contacts without changing invoice recipient snapshots.

<!-- pdac:cite id="UC-REGISTER-CUSTOMER" digest="sha256:1cbbec6400a8cf4165c0a7c82742d6e71e9e434e267e6cc3a2af8c575164670f" -->
<!-- pdac:cite id="BR-PRJ-PARTIES-FROM-CONTACTS" digest="sha256:2fe8ce59516db4c08c3582e1c3dd6341fa3b658e8386d10f725671b633b2dcfe" -->
<!-- pdac:cite id="BR-PARTY-SNAPSHOT" digest="sha256:5ac10d9c4ab5f68acd5a18213fe4c9c0d50786549a1d94c760409214b13355f7" -->

#### Scenario: Upgrade an installation with existing customers
- **WHEN** the application applies the Contacts directory migration to a database containing customers and project references
- **THEN** each customer keeps its identifier, remains selectable for projects and invoices, and retains its previously issued invoice snapshots
