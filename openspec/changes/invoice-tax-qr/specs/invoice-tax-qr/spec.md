<!-- pdac-scope: cited -->
<!-- pdac:cite id="CON-SIF-REGULATION" digest="sha256:a3922433f348bd87649eede3e8a57ca51fba868ad49927883feac172775f8dc3" -->
<!-- pdac:cite id="QR-DETERMINISM" digest="sha256:2186a4638c7ad311864a05314e15638df09365c839eaa932ceb57eddcb1559e9" -->

# Invoice tax QR

## ADDED Requirements

### Requirement: Issued invoice PDFs carry the tax QR

The first page MUST contain one deterministic AEAT verification QR, the label `QR tributario:`, and the `VERI*FACTU` legend.

#### Scenario: Render an issued invoice

- **WHEN** the same issued invoice is rendered twice
- **THEN** the PDF bytes are identical
- **AND** the first page contains the tax label and legend

#### Scenario: Encode invoice identity

- **WHEN** the QR payload is built
- **THEN** it contains the issuer NIF, series/number, issue date, and total in the required parameter order with UTF-8 URL encoding
