<!-- pdac-scope: cited -->
<!-- pdac:cite id="BC-EQUIPMENT" digest="sha256:0e324b394e1d89bacbfa13bc9f312f0592c710fe99f8d9c515dc9c999025bf82" -->

# equipment-module Specification

## ADDED Requirements

### Requirement: Equipment SHALL have an isolated module seam

The Equipment bounded context SHALL have implementation and contracts assemblies, with consumers depending on contracts only.

#### Scenario: a consumer references equipment

- **WHEN** a future module consumes equipment identity
- **THEN** it can reference the contracts assembly without taking an implementation dependency

### Requirement: Equipment SHALL own a persistence schema

The Equipment module SHALL reserve the `equipment` schema for its future tables.

#### Scenario: the first equipment persistence slice is added

- **WHEN** an equipment feature creates its first table
- **THEN** the table is placed under the Equipment-owned schema
