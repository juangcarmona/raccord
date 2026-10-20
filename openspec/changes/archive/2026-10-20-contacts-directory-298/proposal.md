<!-- pdac-scope: cited -->

## Why

Raccord currently stores customers only, while production teams also need a shared directory for suppliers and venues, with safe edits and pagination as records grow. Issue #298 implements the approved Contacts directory behavior from CHG-CONTACTS-DIRECTORY-001 while retaining existing customer identifiers and issued invoice history.

## What Changes

- Maintain company and private-person contacts with one or more customer, supplier and venue roles.
- Add contact-number assignment and uniqueness, editable contact details, and a stable paginated directory.
- Keep existing customer/project/invoice references readable and preserve issued invoice party snapshots when a contact changes.
- Add backend, migration, frontend and selected browser coverage for the user-visible flows.
- Preserve the existing customer API behavior for current project and invoicing consumers while introducing the complete contact directory contract.
- Do not add contact persons, commercial defaults, imports, generic tags or folders; those remain #299–#302.

## Capabilities

### New Capabilities

- `contacts-directory`: authorized users maintain and browse contacts with customer, supplier and venue roles.

### Modified Capabilities

- None. No existing OpenSpec capability describes the Contacts directory; this is the first implementation contract for the approved product behavior.

## Impact

The Contacts domain and persistence adapter, HTTP contracts and endpoints, generated OpenAPI client, React contact screens, project/customer compatibility, migration tests, and Playwright E2E selection. No new production dependency or deployment contract is planned. Existing invoice issuance, numbering, tax calculation and finalized representations remain unchanged.

## Product definition impact

This change implements CHG-CONTACTS-DIRECTORY-001 and depends on every direct product artifact listed below. ProductShape impact analysis also reaches invoice issuance, project selection, audit, contact persons, commercial defaults, tags, folders and other bounded contexts transitively. The invoice and project neighbors are protected by compatibility and snapshot tests; contact persons, commercial defaults, imports, generic tags/folders and workspace series configuration are checked and left out because they belong to #299–#302 or later Workspace work.

<!-- pdac:cite id="BC-CONTACTS" digest="sha256:b63537573b968a16a585370ae0e4a98ec688de6e31e42e8b7e2dad5c89d3f474" -->
<!-- pdac:cite id="UC-CON-MANAGE-CONTACT-DIRECTORY" digest="sha256:58989f1d3dfd59bb6021eb503753e4f9bccd6bd57f4dc43697dddf535c057901" -->
<!-- pdac:cite id="FR-CON-001" digest="sha256:9b09ae74057045edaba5e51c4d14f287a22093be7537c8867a4fee8e07ccaa5f" -->
<!-- pdac:cite id="UC-REGISTER-CUSTOMER" digest="sha256:1cbbec6400a8cf4165c0a7c82742d6e71e9e434e267e6cc3a2af8c575164670f" -->
<!-- pdac:cite id="BR-PARTY-SNAPSHOT" digest="sha256:5ac10d9c4ab5f68acd5a18213fe4c9c0d50786549a1d94c760409214b13355f7" -->
<!-- pdac:cite id="BR-PRJ-PARTIES-FROM-CONTACTS" digest="sha256:2fe8ce59516db4c08c3582e1c3dd6341fa3b658e8386d10f725671b633b2dcfe" -->
<!-- pdac:cite id="BR-PRJ-CONTACT-NUMBER-ASSIGNMENT" digest="sha256:aa32e12b334cd585d5d946fb961a46d5ddfda39bc4b16320a9c5f0b19d7ce8d7" -->
<!-- pdac:cite id="BR-PRJ-CONTACT-ROLES-BY-TAG" digest="sha256:d7110eb5318168938b259d3f44fb022f04666454b5112e97d17539861b8604d9" -->
<!-- pdac:cite id="TERM-CONTACT" digest="sha256:1466e4347ae1ee875b87947164c4bc09a5bce5057ecc590018026d158f9bc885" -->
<!-- pdac:cite id="TERM-CONTACT-TYPE" digest="sha256:696b6459c69ff4d256d49c62bff42571e6a109878d656b82dd5b3be2e52457fc" -->
<!-- pdac:cite id="TERM-CUSTOMER" digest="sha256:e2c324d41aa93283903f70b565b9b4913520ca131a4cb40f71187f8f9b59d03c" -->
<!-- pdac:cite id="TERM-SUPPLIER-CONTACT" digest="sha256:8b634585ba261cec5e5dc78c95546c21830d360eda9f50483da6f6855243f198" -->
<!-- pdac:cite id="TERM-VENUE" digest="sha256:f923ddc29f25c19d1ac3abeb02bc5dfa1b167b9060db0d60ba807cfb809be69c" -->
<!-- pdac:cite id="TERM-CONTACT-NUMBER" digest="sha256:8a4a98e56b529bc22f08b7bd580a515a5d1914e78b30206bf83ec951ae7f8bf3" -->
