<!-- pdac-scope: cited -->
<!-- pdac:cite id="BR-VAT-PER-RATE" digest="sha256:8ba8695a4b892aa5c46cc067105e3701a434e7be49eb7f46d43474a7c6f75bd9" -->

# Design

`LinePricing` carries a validated regime key and an optional exemption cause. Totals group by rate, regime, and cause, preventing unlike tax treatments from being merged. Issued invoice representation version 2 writes both fields; restore accepts version 1 and supplies the historical `standard` regime so existing fingerprints and records remain readable.
