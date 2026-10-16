<!-- pdac-scope: cited -->
<!-- pdac:cite id="ACT-OPERATIONS-COORDINATOR" digest="sha256:c6210b5da8b0aede8884fa0c04d95492b6f47095f239677bd7d5c647040798e7" -->
<!-- pdac:cite id="ACT-BILLING-ADMINISTRATOR" digest="sha256:2efd5f923a1f37f948e7e75d735e47901e8f2eed55bd4e686730529149da7de6" -->
<!-- pdac:cite id="JRN-BILL-FINISHED-PROJECT" digest="sha256:b9530a2b0a9aac0f9cb3a5a9608494d08be1fc1190f395887fc7bb7fb30df28e" -->
<!-- pdac:cite id="JRN-SET-UP-INVOICING" digest="sha256:a47b84f924d17095df0ad8bc4e3436fa67d9d51bd1d241c7228a7624cb8c548e" -->

## Purpose

The public homepage gives audiovisual production teams a truthful, understandable introduction to Raccord and its current maturity.

## ADDED Requirements

### Requirement: Homepage introduces Raccord accurately

The public homepage SHALL explain in Spanish that Raccord is an open-source, self-hostable operations and invoicing platform for audiovisual production teams, and SHALL describe only capabilities supported by the current product and repository documentation.
<!-- pdac:cite id="ACT-OPERATIONS-COORDINATOR" digest="sha256:c6210b5da8b0aede8884fa0c04d95492b6f47095f239677bd7d5c647040798e7" -->
<!-- pdac:cite id="ACT-BILLING-ADMINISTRATOR" digest="sha256:2efd5f923a1f37f948e7e75d735e47901e8f2eed55bd4e686730529149da7de6" -->
<!-- pdac:cite id="JRN-BILL-FINISHED-PROJECT" digest="sha256:b9530a2b0a9aac0f9cb3a5a9608494d08be1fc1190f395887fc7bb7fb30df28e" -->

#### Scenario: Visitor understands the project's purpose

- **WHEN** a visitor opens the homepage
- **THEN** the page identifies Raccord, its intended audience, and its operations and invoicing purpose in Spanish

#### Scenario: Visitor sees the product's current maturity

- **WHEN** a visitor reads the homepage
- **THEN** the page distinguishes the implemented first vertical slice from capabilities that are not yet implemented
- **AND** it makes no legal or VERI*FACTU compliance claim

### Requirement: Homepage keeps the application boundary clear

The public homepage SHALL be served as static content independently of the Raccord application and SHALL NOT expose an application instance, account data, or operational data.

#### Scenario: Visitor follows a call to action

- **WHEN** a visitor follows a homepage link to project or self-hosting information
- **THEN** the destination is public project documentation or the source repository
- **AND** it does not open an unauthenticated Raccord application instance

### Requirement: Homepage source supports future translation

The homepage content SHALL be organized so that additional language versions can be added without changing its intended meaning or visual structure.

#### Scenario: Spanish is the initial published language

- **WHEN** the homepage is built for this delivery
- **THEN** its published visitor-facing content is Spanish
- **AND** no untranslated language selector or empty translation page is presented

#### Scenario: Another language is added later

- **WHEN** a later delivery adds another language
- **THEN** it can add translated content while retaining the homepage's existing content roles and navigation structure
