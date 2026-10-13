<!-- pdac-scope: cited -->
<!-- pdac:cite id="BR-VAT-PER-RATE" digest="sha256:8ba8695a4b892aa5c46cc067105e3701a434e7be49eb7f46d43474a7c6f75bd9" -->
<!-- pdac:cite id="CON-SIF-REGULATION" digest="sha256:a3922433f348bd87649eede3e8a57ca51fba868ad49927883feac172775f8dc3" -->

# VAT regime and exemption cause

## ADDED Requirements

### Requirement: Invoice tax groups preserve their tax treatment

The domain and issued representation MUST preserve a VAT regime key and optional exemption cause for each VAT group, and MUST NOT merge groups that have different tax treatment.

#### Scenario: Two exemption causes at the same rate

- **WHEN** two zero-rated lines have different exemption causes
- **THEN** the totals contain two VAT groups with their respective causes

#### Scenario: Restore a legacy issued invoice

- **WHEN** an issued representation uses schema version 1 and has no regime fields
- **THEN** the system verifies its existing hash and restores it with the historical standard regime
