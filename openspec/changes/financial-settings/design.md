<!-- pdac-scope: cited -->
<!-- pdac:cite id="UC-FIN-CONFIGURE-FINANCIAL-SETTINGS" digest="sha256:c8d6c60d06a57fc49ca312afeb07badd80ffa259b9a23e76a97f09a8a52537df" -->
<!-- pdac:cite id="CON-WSP-ONE-COMPANY-PER-WORKSPACE" digest="sha256:bca1432a6cfd1820c106f7310e849a0d1b98e716a4b0b4ee883d09c295e988d7" -->

# Design

The settings are a single persisted aggregate with domain validation. The existing organization use case exposes idempotent GET and PUT endpoints and records changes in the audit trail. Quote generation and signing consume this seam in later slices.
