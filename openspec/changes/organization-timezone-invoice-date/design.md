<!-- pdac-scope: cited -->
<!-- pdac:cite id="UC-WSP-SIGN-IN" digest="sha256:d3518a52b6a4950d1caa28d4ba04451e45ab0949d3cba65dffdbc1ebba0a9221" -->

# Design

Represent an untouched date input as an empty string in the draft form. Finalization maps that value to the nullable API field `issueDate: null`; an explicitly selected date is sent unchanged. The existing server workflow resolves null to `clock.Today`, then continues to enforce numbering and date-order rules.
