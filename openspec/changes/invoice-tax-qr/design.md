<!-- pdac-scope: cited -->
<!-- pdac:cite id="CON-SIF-REGULATION" digest="sha256:a3922433f348bd87649eede3e8a57ca51fba868ad49927883feac172775f8dc3" -->

# Design

Build the production AEAT URL in a pure formatter using the issuer NIF, invoice series/number, issue date, and total. Generate an ISO 18004 QR at error-correction level M with QRCoder, draw its modules as deterministic PDF vector rectangles, and place the mandatory label and legend on page one. No timestamp or random rendering state enters the payload or PDF layout.
