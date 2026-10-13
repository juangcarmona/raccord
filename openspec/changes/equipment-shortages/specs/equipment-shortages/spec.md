<!-- pdac-scope: cited -->
<!-- pdac:cite id="UC-WHS-REVIEW-SHORTAGES" digest="sha256:e157f54441bcab688aa8a6173f360a637269bab9e0b4ddd8e30918dd7064caa9" -->
<!-- pdac:cite id="UC-WHS-SOLVE-WITH-ALTERNATIVE" digest="sha256:13078f3fff630039f5b1d9333a2ed3705f43bd4f3a2a27dfe78dfd995fca9b3d" -->

# Equipment shortages

## ADDED Requirements

### Requirement: Shortages can be reviewed and resolved

The API MUST list shortage rows and MUST allow resolution only with an alternative declared on the source item.

#### Scenario: Resolve a shortage

- **WHEN** a coordinator selects a declared alternative for a shortage line
- **THEN** the planned line uses that alternative and the shortage is recalculated away when capacity permits

#### Scenario: Reject an undeclared alternative

- **WHEN** a coordinator selects an item not declared as an alternative
- **THEN** the planned line remains unchanged
