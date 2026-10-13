<!-- pdac-scope: cited -->
<!-- pdac:cite id="UC-EQP-DEFINE-PHYSICAL-KIT" digest="sha256:c0983ff454bf1a167823a0012d203f10353642455341ba3d4fe0237f798d3418" -->
<!-- pdac:cite id="UC-EQP-DEFINE-VIRTUAL-KIT" digest="sha256:ed80622fec90f88a106db26f2270755f002d89be36433761615a0e7f3dd44f84" -->

# Equipment kits

## ADDED Requirements

### Requirement: Equipment items support physical and virtual kits

An equipment item MUST persist its kit kind and default content lines.

#### Scenario: Configure a kit

- **WHEN** a client configures an item with a physical or virtual kind and existing content items
- **THEN** the API persists and returns the kit configuration

### Requirement: Kit content is safe to persist

Kit content MUST use positive quantities, existing catalogue item identifiers, and MUST NOT contain the kit itself.

#### Scenario: Reject invalid content

- **WHEN** a client submits a self-reference, missing item, or non-positive quantity
- **THEN** the API rejects the configuration and leaves the item unchanged
