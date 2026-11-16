<!-- pdac-scope: cited -->
<!-- pdac:cite id="UC-WSP-SIGN-IN" digest="sha256:d3518a52b6a4950d1caa28d4ba04451e45ab0949d3cba65dffdbc1ebba0a9221" -->

# Design

Represent an untouched date input as an empty string in the draft form. Finalization maps that value to the nullable API field `issueDate: null`; an explicitly selected date is sent unchanged. The existing server workflow resolves null to `clock.Today`, then continues to enforce numbering and date-order rules.

Shared browser-journey invoice finalization uses the same seeded organization date (`Europe/Madrid`) as the explicit-date regression. Mixing a UTC default with the organization date lets test order advance a shared invoice series past the next test after local midnight; keep both helpers on the organization clock.
