<!-- pdac-scope: cited -->

## Purpose

Visitors can understand the product map, learn verified tasks and choose a self-hosting path through plain-language Spanish and English pages.

## ADDED Requirements

### Requirement: Product map explains the ten areas without overstating availability

The public product page SHALL explain the ten product areas in plain language and SHALL distinguish the product model's scope from capability availability. It MUST NOT label every modeled capability as implemented unless repository evidence supports that claim.

<!-- pdac:cite id="BC-CONTACTS" digest="sha256:bdadd84cb901027c8630d88c714ba8c52bcecdf383345d35a2631c7fa8b69c85" -->
<!-- pdac:cite id="BC-PROJECTS" digest="sha256:11cadbc8906365210716689609d66738e29fc576822aa2e8e7a925a8bae0933e" -->
<!-- pdac:cite id="BC-COMMERCIAL" digest="sha256:cecf438d86ba351619417a2951b5dbc53aea0c70dbb07a0dd509e80ab4391adc" -->
<!-- pdac:cite id="BC-INVOICING" digest="sha256:386343a8a3613a73a2ab60d1b5767cbe7d880fb8ded0d2d1a7c5dee68fa2a567" -->
<!-- pdac:cite id="BC-EQUIPMENT" digest="sha256:0e324b394e1d89bacbfa13bc9f312f0592c710fe99f8d9c515dc9c999025bf82" -->
<!-- pdac:cite id="BC-WAREHOUSE" digest="sha256:2604b7b7573cfde1759e4b8c0cb9c5c76a188520fac77862a06344f8d08ca2ff" -->
<!-- pdac:cite id="BC-CREW" digest="sha256:424bcce4c1dcf63261bb06395d5889f8e2e931cb7499ef4feb96c3417418fefe" -->
<!-- pdac:cite id="BC-DOCUMENTS" digest="sha256:36cfde6671027e37541e237722980667ca47ad36a8fe2f6dd8712dd75f286a54" -->
<!-- pdac:cite id="BC-WORKSPACE" digest="sha256:013841dd37789974afef821eb2c071a85acaa72c083abd2285b7c406db95f8a5" -->
<!-- pdac:cite id="BC-COMPLIANCE" digest="sha256:92fbac72cbcb8e640f0c87ff7da3629698b6bb3ee7cf5bdf742db249ca898dc0" -->

#### Scenario: Visitor explores product areas

- **WHEN** a visitor opens the product page
- **THEN** the page explains Contacts, Projects, Commercial, Invoicing, Equipment, Warehouse, Crew and transport, Documents and communication, Workspace, and Compliance and audit
- **AND** it identifies the map as product scope rather than a verified availability inventory

#### Scenario: Visitor sees a demonstrated workflow

- **WHEN** a visitor opens the invoice workflow
- **THEN** the page presents the publication-safe invoice screenshot with a caption, useful alternative text, fictional-data disclosure and a full-size link

### Requirement: Manuals teach tasks in plain language

The public manuals SHALL group task-based instructions for production operators, billing staff and installation administrators, and SHALL distinguish the demonstrated invoice workflow from installation and operational procedures.

<!-- pdac:cite id="ACT-OPERATIONS-COORDINATOR" digest="sha256:c6210b5da8b0aede8884fa0c04d95492b6f47095f239677bd7d5c647040798e7" -->
<!-- pdac:cite id="ACT-BILLING-ADMINISTRATOR" digest="sha256:2efd5f923a1f37f948e7e75d735e47901e8f2eed55bd4e686730529149da7de6" -->
<!-- pdac:cite id="ACT-INSTALLATION-ADMINISTRATOR" digest="sha256:2a214515187fede3e5d9b093a24cb298d21db07c0300f2772dfcccf1d8ff0815" -->
<!-- pdac:cite id="JRN-SET-UP-INVOICING" digest="sha256:a47b84f924d17095df0ad8bc4e3436fa67d9d51bd1d241c7228a7624cb8c548e" -->
<!-- pdac:cite id="JRN-OPERATE-INSTALLATION" digest="sha256:113619d8648d3bb2d2e7e05eeccc24a2fa6499dde56e525b4642286ada27787c" -->

#### Scenario: Billing staff follows the demonstrated invoice workflow

- **WHEN** a billing user opens the invoice manual
- **THEN** it explains the customer, project, billable-work, invoice-issue and PDF-download sequence
- **AND** it warns the reader to validate configuration before using real accounting data

#### Scenario: Installation administrator chooses a deployment guide

- **WHEN** an administrator opens the manuals index
- **THEN** they can choose Docker Compose, Azure deployment, or backup and restore instructions
- **AND** .NET Aspire is identified as a development environment rather than a production deployment path

### Requirement: Self-hosting guidance makes costs and security responsibilities clear

The public site SHALL explain that Raccord has no licence fee, that infrastructure and operations can still cost money, and that Docker Compose and Azure are separately documented self-hosting paths.

<!-- pdac:cite id="ACT-INSTALLATION-ADMINISTRATOR" digest="sha256:2a214515187fede3e5d9b093a24cb298d21db07c0300f2772dfcccf1d8ff0815" -->
<!-- pdac:cite id="JRN-OPERATE-INSTALLATION" digest="sha256:113619d8648d3bb2d2e7e05eeccc24a2fa6499dde56e525b4642286ada27787c" -->

#### Scenario: Visitor compares self-hosting paths

- **WHEN** a visitor opens self-hosting information
- **THEN** the page describes own-server, cloud, Azure and local-development paths
- **AND** it names likely infrastructure and operating costs
- **AND** it presents the first-admin setup, TLS, database exposure and backups as operator responsibilities

### Requirement: Engineering overview explains system boundaries visually

The engineering overview SHALL explain the browser, web application and API, database, document storage and installation administrator responsibilities in plain language, with .NET Aspire clearly identified as a development environment.

<!-- pdac:cite id="BC-PROJECTS" digest="sha256:11cadbc8906365210716689609d66738e29fc576822aa2e8e7a925a8bae0933e" -->
<!-- pdac:cite id="BC-INVOICING" digest="sha256:386343a8a3613a73a2ab60d1b5767cbe7d880fb8ded0d2d1a7c5dee68fa2a567" -->
<!-- pdac:cite id="BC-COMPLIANCE" digest="sha256:92fbac72cbcb8e640f0c87ff7da3629698b6bb3ee7cf5bdf742db249ca898dc0" -->

#### Scenario: Interested visitor explores the architecture

- **WHEN** a visitor opens the engineering overview
- **THEN** an accessible diagram explains the runtime parts and their relationships
- **AND** the page links the system boundary to the product area map

### Requirement: Product and guidance pages are complete in both languages

The product, manual, hosting and engineering routes SHALL provide equivalent Spanish and English navigation, claims, instructions, screenshots and maturity or security notices without mixing languages.

#### Scenario: Visitor changes locale on a nested route

- **WHEN** a visitor changes language from any public route
- **THEN** the matching page in the other locale opens when available
- **AND** all labels, page content and image descriptions use that locale

### Requirement: Static site build verifies public routes and local assets

The static build SHALL emit every documented route in both locales and SHALL fail verification if a local navigation target or required screenshot asset is missing.

#### Scenario: Build checks route and link completeness

- **WHEN** the public-site verification command runs
- **THEN** it checks all routes, locale metadata, navigation destinations, screenshot paths and required disclosures
- **AND** it emits no application, API, database or customer data
