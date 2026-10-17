<!-- pdac-scope: cited -->

## MODIFIED Requirements

### Requirement: Homepage introduces Raccord accurately

The public homepage SHALL explain in Spanish and English that Raccord is open-source, self-hostable operations and invoicing software for audiovisual production and equipment-rental teams. It SHALL explain that there is no licence fee while hosting and operating the software may cost money, and SHALL describe only capabilities supported by repository evidence.

<!-- pdac:cite id="ACT-OPERATIONS-COORDINATOR" digest="sha256:c6210b5da8b0aede8884fa0c04d95492b6f47095f239677bd7d5c647040798e7" -->
<!-- pdac:cite id="ACT-BILLING-ADMINISTRATOR" digest="sha256:2efd5f923a1f37f948e7e75d735e47901e8f2eed55bd4e686730529149da7de6" -->

#### Scenario: Visitor understands the project's purpose

- **WHEN** a visitor opens the homepage in either language
- **THEN** the page identifies Raccord, its intended audience, and its operations and invoicing purpose
- **AND** it makes clear that software is free to use while hosting and operations may cost money

#### Scenario: Visitor sees the product's current maturity

- **WHEN** a visitor reads the homepage
- **THEN** the page says the MVP is implemented and identifies which workflow has browser evidence
- **AND** it distinguishes the ten-area product map from the capability inventory still under review
- **AND** it makes no legal or VERI\*FACTU compliance claim

### Requirement: Homepage keeps the application boundary clear

The homepage SHALL be served as static content independently of the Raccord application and SHALL NOT expose an application instance, account data, or operational data.

#### Scenario: Visitor follows a call to action

- **WHEN** a visitor follows a homepage link to product, manuals, self-hosting or engineering information
- **THEN** the destination is a static public page or source documentation
- **AND** it does not open an unauthenticated Raccord application instance

### Requirement: Homepage source supports future translation

The homepage content SHALL provide complete Spanish and English versions with equivalent information, navigation and maturity notices, and SHALL keep locale routes structured so additional languages can be added without changing their meaning or visual roles.

#### Scenario: Spanish is the initial published language

- **WHEN** the homepage is built for this delivery
- **THEN** it emits complete Spanish and English routes
- **AND** each route names both available languages in the selector
- **AND** it presents no untranslated or empty translation page

#### Scenario: Another language is added later

- **WHEN** a later delivery adds another language
- **THEN** it can add translated content while retaining the homepage's content roles and navigation structure
