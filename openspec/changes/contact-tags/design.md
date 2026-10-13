<!-- pdac-scope: cited -->
<!-- pdac:cite id="UC-PRJ-TAG-RECORDS" digest="sha256:4848a8e7720a327080902073b520293c7f2635891f2ff92dcf7e64c7b5ef1986" -->
<!-- pdac:cite id="UC-PRJ-MANAGE-RECORD-FOLDERS" digest="sha256:c6084e098fbeb011a1838333a9dec324dda5c224051ca0485c8e42d3c03549ac" -->

# Design

Contact taxonomy is stored with the customer row. Writes normalize duplicate tags and whitespace at the boundary, while the customer list applies case-insensitive tag and folder filters. The taxonomy endpoints expose read and update operations for an individual contact.
