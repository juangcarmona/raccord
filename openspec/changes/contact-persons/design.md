<!-- pdac-scope: cited -->
<!-- pdac:cite id="UC-PRJ-ADD-CONTACT-PERSON" digest="sha256:d692c11d96c5a6a623b821ad083c2da17477b41c8ea8ec165b1c52c50d134c0a" -->

# Design

Contact persons are stored in a dedicated Contacts-owned table keyed by the contact identifier. A filtered unique index enforces one default person, while the service clears the previous default before saving a replacement.

