<!-- pdac-scope: cited -->

## Purpose

Lets authorized operations coordinators maintain people affiliated with each Contacts record and keep the person's supported designations consistent.

## ADDED Requirements

### Requirement: Maintain contact persons and their designations

The system MUST let an authorized user create, read and edit contact-person records under an existing contact, with a name and supported contact details, and record default and administrative designations. A contact MUST have at most one default contact person and at most one administrative contact person. Setting another person as default MUST clear the previous default atomically; a rejected change MUST leave the previous default intact.

<!-- pdac:cite id="UC-CON-MANAGE-CONTACT-PERSONS" digest="sha256:8253cf3c4c3b50fc1f53ca143ee99989c9c7652e0fa822e7d13ad5306bb43385" -->
<!-- pdac:cite id="FR-CON-002" digest="sha256:d44280cb004062fbc3bd34228233dcfa03b2b6e51ee64b4f717652351f721feb" -->
<!-- pdac:cite id="BR-CON-CONTACT-PERSON-DESIGNATIONS" digest="sha256:a0f4d168bcd4dc9375a7f21fd3341b41eb0d100c57fe6a1a25c54ea21986e27e" -->
<!-- pdac:cite id="BR-PRJ-CONTACT-PERSON-NOT-MOVABLE" digest="sha256:5b2f5e3280eb10ed7873b1e8d55d3c9f3eb57a339aef663b822a17da062c86af" -->
<!-- pdac:cite id="TERM-CONTACT-PERSON" digest="sha256:9c97fade3763d3b45e1db235ec2976385be91307202534246a099b14b9ef2c9a" -->
<!-- pdac:cite id="TERM-DEFAULT-CONTACT-PERSON" digest="sha256:683b1f3e9005fb4dd022a1316c1f9047d6ca6b5ebb7c644e9163976693fdb233" -->
<!-- pdac:cite id="TERM-ADMINISTRATIVE-CONTACT-PERSON" digest="sha256:7410d2a935398a179077058351365d12109e0fcf038d425f43bb174aebc0a058" -->

#### Scenario: Change the default contact person

- **WHEN** an authorized user marks a second person as the default for a contact
- **THEN** the second person is the sole default and the former default remains associated with the contact without that designation

#### Scenario: Keep a rejected default change unchanged

- **WHEN** a default change fails validation or persistence
- **THEN** the previously saved default remains the contact's sole default

#### Scenario: Maintain an administrative contact person

- **WHEN** an authorized user designates a person as administrative for a contact that already has another administrative person
- **THEN** the system rejects the change or atomically replaces the prior administrative designation, and no contact has more than one administrative person

#### Scenario: Reject a person moved to another contact

- **WHEN** an edit attempts to change a contact person's parent contact
- **THEN** the system rejects the request and preserves the saved person under its original contact

#### Scenario: Create a person under a second contact

- **WHEN** the same individual is associated with another contact
- **THEN** the system stores a separate person record under the second contact without moving or changing the first record

### Requirement: Keep designations within Contacts

Changing a contact person's default or administrative designation MUST NOT change a project's selected contact or any invoice recipient details. Contact-person records MUST NOT become project or invoice recipients through this capability.

<!-- pdac:cite id="FR-CON-002" digest="sha256:d44280cb004062fbc3bd34228233dcfa03b2b6e51ee64b4f717652351f721feb" -->
<!-- pdac:cite id="SB-CON-CHANGE-DEFAULT-CONTACT-PERSON" digest="sha256:fc39711eb0d43ad05b94f3c9fbbf6b2b2178ae806e8a05da3af3d8d1b0459e97" -->

#### Scenario: Change designations without project or invoice side effects

- **WHEN** an authorized user changes the default and administrative person for a contact
- **THEN** the associated project retains its selected contact and any issued invoice retains its stored recipient details
