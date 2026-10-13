<!-- pdac-scope: cited -->
<!-- pdac:cite id="TERM-INVITATION" digest="sha256:becd359c02edf57b0139e5142543eaeed23d68187809451a9c43eabfce5f7759" -->

# Design

Invitation persistence is kept in the Workspace Identity database. The service hashes random 256-bit tokens with SHA-256, applies the product's 30-day expiry, and consumes the record in the same transaction that creates the Identity user and assigns the selected role. The API returns the raw token only at creation time; the SPA turns it into a shareable join URL.
