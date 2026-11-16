---
id: CHG-CONTACTS-RECOVERY
type: product-change
title: Reconcile approved Contacts workflows with the current product baseline
status: applied
base-revision: 4cf56c86891d500e1220051009bb16e8cd6b05d3
operations:
  add:
    - BR-CON-CONTACT-PERSON-DESIGNATIONS
    - BR-CON-CONTACT-TAG-SCOPE
    - BR-CON-FOLDER-DELETE-NONEMPTY
    - BR-CON-FOLDER-HIERARCHY
    - BR-CON-IMPORT-AUDIT
    - BR-CON-IMPORT-ROW-ATOMICITY
    - BR-CON-TAG-FILTER-MODES
    - CON-CON-IMPORT-FILE-FORMAT
    - FR-CON-001
    - FR-CON-002
    - FR-CON-003
    - FR-CON-004
    - FR-CON-005
    - SB-CON-CHANGE-DEFAULT-CONTACT-PERSON
    - SB-CON-CONTACT-PERSON-PRESERVES-PARTY-SNAPSHOT
    - SB-CON-CONTACTS-ISOLATED-BY-WORKSPACE
    - SB-CON-IMPORT-RECHECKS-PREVIEW
    - SB-CON-IMPORT-VALID-AND-INVALID-ROWS
    - SB-CON-ORGANIZE-AND-FILTER-CONTACTS
    - SB-CON-REASSIGN-ADMINISTRATIVE-CONTACT-PERSON
    - SB-CON-TAG-FILTER-ANY-AND-ALL
    - TERM-CONTACT-FOLDER
    - TERM-CONTACT-TAG
    - UC-CON-FILTER-CONTACT-DIRECTORY
    - UC-CON-IMPORT-CONTACTS
    - UC-CON-MANAGE-CONTACT-DIRECTORY
    - UC-CON-MANAGE-CONTACT-FOLDERS
    - UC-CON-MANAGE-CONTACT-PERSONS
    - UC-CON-TAG-CONTACTS
  modify:
    - BC-CONTACTS
    - BR-PARTY-SNAPSHOT
    - BR-PRJ-CONTACT-IMPORT-MODES
    - BR-PRJ-CONTACT-NUMBER-ASSIGNMENT
    - BR-PRJ-CONTACT-PERSON-IMPORT-ROWS
    - BR-PRJ-CONTACT-PERSON-NOT-MOVABLE
    - BR-PRJ-CONTACT-ROLES-BY-TAG
    - BR-PRJ-FOLDER-DELETE-NONEMPTY
    - BR-PRJ-IMPORT-FOLDER-PLACEMENT
    - BR-PRJ-IMPORT-RESULT-REPORT
    - BR-PRJ-IMPORT-TAG
    - BR-PRJ-PARTIES-FROM-CONTACTS
    - FR-CUSTOMER-001
    - FR-PRJ-032
    - FR-PRJ-034
    - JRN-PRJ-SET-UP-CONTACT-DATABASE
    - SB-PRJ-CONTACT-IMPORT-SKIP-EXISTING
    - SB-PRJ-IMPORT-FAILED-ROWS-FILE
    - SB-PRJ-NONEMPTY-FOLDER-DELETE-REFUSED
    - TERM-ADMINISTRATIVE-CONTACT-PERSON
    - TERM-CONTACT
    - TERM-CONTACT-NUMBER
    - TERM-CONTACT-PERSON
    - TERM-CONTACT-TYPE
    - TERM-CUSTOMER
    - TERM-DEFAULT-CONTACT-PERSON
    - TERM-RECORD-FOLDER
    - TERM-SUPPLIER-CONTACT
    - TERM-VENUE
    - UC-PRJ-ADD-CONTACT
    - UC-PRJ-ADD-CONTACT-PERSON
    - UC-PRJ-IMPORT-CONTACTS
    - UC-PRJ-MANAGE-RECORD-FOLDERS
    - UC-REGISTER-CUSTOMER
  remove: []
unaffected:
  - id: BR-CRW-IMPORT-FOLDER-PLACEMENT
    cause: TERM-RECORD-FOLDER
    relationship: uses-terms
    digest: sha256:74755cefee4be391830e36aed65039e10383040eaa9d0ac91ef57933bdf1caae
    cause-digest: sha256:84a8bbb6da5aab3d49d19b3b4af8040bc59cef97e91215840b5183d5e687ee9a
    reason: Only Contacts gains optional membership. Crew, vehicles, crew lists and resource imports retain their own folder requirements and all modules retain separate folder structures.
  - id: BR-ISSUE-READINESS
    cause: TERM-CUSTOMER
    relationship: uses-terms
    digest: sha256:ee44a70a4b3c26c6b5d0b46a863bf69c6dd8949de761dcf9d4359331a094140c
    cause-digest: sha256:3e5e2b709352e3374527d591cea5fb66178c7b1ed02a77d3f63a5a36ee530085
    reason: The customer remains a contact role usable on projects and invoices; legal readiness, address data and financial defaults retain their own rules.
  - id: BR-PRJ-CONTACT-SELECTION-EFFECTS
    cause: TERM-DEFAULT-CONTACT-PERSON
    relationship: uses-terms
    digest: sha256:018ddac1a90e2a3f16bb9e16a5489d885d53f6a57db7cb60abfe5529ad95c467
    cause-digest: sha256:9147ac4d7ce309dce4b1d0d90b50d70cb3d763f96d9a3040bf5ee4f8d0125627
    reason: Zero-or-one default designation preserves explicit project selection and the existing invoice fallback; maintenance does not rewrite saved project associations.
  - id: BR-PRJ-FOLDER-PER-MODULE
    cause: TERM-RECORD-FOLDER
    relationship: uses-terms
    digest: sha256:7c056d3c19dc4216daeb29899d42d5ed2ca098b949533083b25339e5685eefe8
    cause-digest: sha256:84a8bbb6da5aab3d49d19b3b4af8040bc59cef97e91215840b5183d5e687ee9a
    reason: Only Contacts gains optional membership. Crew, vehicles, crew lists and resource imports retain their own folder requirements and all modules retain separate folder structures.
  - id: BR-PRJ-INVOICE-RECIPIENT
    cause: TERM-ADMINISTRATIVE-CONTACT-PERSON
    relationship: uses-terms
    digest: sha256:5bd94b44ca62c6c328b9dcfee904b108a0543af7e5a048e4637c7daca3816e52
    cause-digest: sha256:c4e7dffe0de236bbb7f83cf43da7e5005abbe0e1d843576818546856fcbda854
    reason: Zero-or-one administrative designation preserves the existing selection of that person when present and the existing default-person fallback; no invoice is sent by maintenance.
  - id: BR-PRJ-INVOICE-RECIPIENT
    cause: TERM-DEFAULT-CONTACT-PERSON
    relationship: uses-terms
    digest: sha256:5bd94b44ca62c6c328b9dcfee904b108a0543af7e5a048e4637c7daca3816e52
    cause-digest: sha256:9147ac4d7ce309dce4b1d0d90b50d70cb3d763f96d9a3040bf5ee4f8d0125627
    reason: Zero-or-one default designation preserves explicit project selection and the existing invoice fallback; maintenance does not rewrite saved project associations.
  - id: BR-PRJ-ONE-CONTACT-PER-AREA
    cause: TERM-VENUE
    relationship: uses-terms
    digest: sha256:3518d93557bcb65d41dc2035da538f45c593659928d695cafee367a7ba4e7a9f
    cause-digest: sha256:6349295a8e46a12f3799c590ade9dd6a67a99c8a6b03594c3ede3b3b2ad023f2
    reason: A venue remains a location contact, including a separate contact per distinct area. Directory promotion does not change project-location or per-area identity.
  - id: BR-WSP-MASTER-CHANGES-NOT-RETROACTIVE
    cause: TERM-CONTACT
    relationship: uses-terms
    digest: sha256:4baafaec26cd65efe8197f9eee50f2c1a76469e9a9451e34694b38b76e1b2c6f
    cause-digest: sha256:765bb0dbb94cad61730e1e3b91034b2b45afded759fa10ab67cddec377e4d880
    reason: The contact remains the same stable selectable party; optional filing and directory maintenance do not change this consumer’s address, project, finance or reference semantics.
  - id: CON-PRJ-IMPORT-FILE-FORMAT
    cause: UC-PRJ-IMPORT-CONTACTS
    relationship: applies-to
    digest: sha256:3dcf59a381641739c2db4f2648c2ae6d9e51929fe8e3430f321f3088fcc9a17b
    cause-digest: sha256:5557773077854fa60362d23aaa693001e79541de8990b98e3a5b29b53dbd79c3
    reason: This artifact remains the extended import profile. Existing plain-text, worksheet, column and preview constraints still apply; the separately scoped create-only profile has its own limits.
  - id: CON-SINGLE-ISSUER
    cause: BC-CONTACTS
    relationship: applies-to
    digest: sha256:f41d4ba8da941e762b3ee568fa15b478f1a60443cb2ea905d4826b13cdb1e1cd
    cause-digest: sha256:48d05be4499e68dc216bca722744448025ac982a2be5fc0d3726d6b289c97977
    reason: Directory, people and organization remain within Contacts; no issuer, registry, commercial-default or project-selection workflow is added or changed.
  - id: FR-INVOICE-006
    cause: BR-PARTY-SNAPSHOT
    relationship: derived-from
    digest: sha256:fee328e3745e8275c00914dd830f3c7cbd3a1aec267f18ed117ef24ddb8e2466
    cause-digest: sha256:eec706ec68420337799bb8bcbeb1c690876f8c071a823de6e087387d8f3f6ac5
    reason: Only status is promoted; the complete later issuer-review and immutable-snapshot rule body is preserved, including the distinct-second-administrator decision.
  - id: FR-ISSUER-001
    cause: BR-PARTY-SNAPSHOT
    relationship: derived-from
    digest: sha256:1d443dcade4a5af9dce47d75a383a243a298dd37de5a2d1be22bc80d5dfe1712
    cause-digest: sha256:eec706ec68420337799bb8bcbeb1c690876f8c071a823de6e087387d8f3f6ac5
    reason: Only status is promoted; the complete later issuer-review and immutable-snapshot rule body is preserved, including the distinct-second-administrator decision.
  - id: FR-PRJ-002
    cause: BR-PRJ-PARTIES-FROM-CONTACTS
    relationship: derived-from
    digest: sha256:7d7b51fe0be472a183cd048ae459cbf9dd0101835afcbe15e756c3e55bf7fbc5
    cause-digest: sha256:b539abeef519e679b5a5da2cebcbe60bc04796718262d3489e703f5f57100344
    reason: The same contact directory still supplies project parties; project editing and saved associations retain their existing semantics.
  - id: FR-PRJ-016
    cause: SB-PRJ-IMPORT-FAILED-ROWS-FILE
    relationship: verification[].scenario-ref
    digest: sha256:795dc1388e40e16768c805ae795ce1fe7d3ce2729c7f2b8806f39eef7f463589
    cause-digest: sha256:cf184a6b534baaf829357de27d50c00cb9cd80fb2a233f86794cc8b3ddc3a184
    reason: Downloadable failure files remain required for equipment and extended imports; only the distinct create-only Contacts profile excludes that capability.
  - id: FR-PRJ-027
    cause: BR-PRJ-IMPORT-TAG
    relationship: derived-from
    digest: sha256:d56ef1349d3239db3e73ad42daa7d55d2c5441d9c81d273f2c51d3e9be78d762
    cause-digest: sha256:28796997820643251297eb67b65908701248c1d69550dfd01ac3621450737479
    reason: The exception is restricted to create-only Contacts import. Equipment and task imports retain their tagging and existing undo/archive behavior.
  - id: FR-PRJ-035
    cause: BR-PRJ-FOLDER-DELETE-NONEMPTY
    relationship: derived-from
    digest: sha256:0069d36fdd941121bc23f9c5caa3e03d1a0b4f0151a34021d4efdc785c1d5ecf
    cause-digest: sha256:67674d5eb77a49e26e74231db2e8c2d4c455f1369df2b575435fd8fc04905bf9
    reason: The optional Contacts-membership exception does not weaken protection of nonempty folders or independent per-module hierarchies.
  - id: FR-PRJ-035
    cause: UC-PRJ-MANAGE-RECORD-FOLDERS
    relationship: derived-from
    digest: sha256:0069d36fdd941121bc23f9c5caa3e03d1a0b4f0151a34021d4efdc785c1d5ecf
    cause-digest: sha256:bfda13ef9b5a1ef79987dc4325f483862d8dcedfc73ac5ec72e685ef6e74b4a8
    reason: Contacts-specific optional placement delegates to its dedicated use case; the requirement for independent structures, subfolders and refusing nonempty deletion remains valid.
  - id: FR-PRJ-035
    cause: TERM-RECORD-FOLDER
    relationship: uses-terms
    digest: sha256:0069d36fdd941121bc23f9c5caa3e03d1a0b4f0151a34021d4efdc785c1d5ecf
    cause-digest: sha256:84a8bbb6da5aab3d49d19b3b4af8040bc59cef97e91215840b5183d5e687ee9a
    reason: Only Contacts gains optional membership. Crew, vehicles, crew lists and resource imports retain their own folder requirements and all modules retain separate folder structures.
  - id: FR-PRJ-035
    cause: SB-PRJ-NONEMPTY-FOLDER-DELETE-REFUSED
    relationship: verification[].scenario-ref
    digest: sha256:0069d36fdd941121bc23f9c5caa3e03d1a0b4f0151a34021d4efdc785c1d5ecf
    cause-digest: sha256:babed2b1090084b1df8c6ca90812664d4329b2f270189a072e7a157e40d19f3d
    reason: The example still refuses deletion of a folder containing contacts. Optional unfiled contacts do not weaken that deletion protection.
  - id: FR-PRJ-101
    cause: TERM-CONTACT
    relationship: uses-terms
    digest: sha256:d1bddd6292b69997e4c2819f9894c9842f364c6564dbb549d2d241fc466075ca
    cause-digest: sha256:765bb0dbb94cad61730e1e3b91034b2b45afded759fa10ab67cddec377e4d880
    reason: The contact remains the same stable selectable party; optional filing and directory maintenance do not change this consumer’s address, project, finance or reference semantics.
  - id: JRN-SET-UP-INVOICING
    cause: UC-REGISTER-CUSTOMER
    relationship: steps[].use-case
    digest: sha256:a47b84f924d17095df0ad8bc4e3436fa67d9d51bd1d241c7228a7624cb8c548e
    cause-digest: sha256:2ed3ef0366ab20037c6770f083c9883517370afc4cae9deb633f20bf6e960f23
    reason: The customer-registration entry remains available before invoicing; directory maintenance does not relax the complete recipient legal details required by invoice issue readiness.
  - id: QR-PRJ-IMPORT-SIZE
    cause: UC-PRJ-IMPORT-CONTACTS
    relationship: applies-to
    digest: sha256:0a35bd22bc477e49d15758f8b50014929b851d6c9e700fd6ce446180c19d0f34
    cause-digest: sha256:5557773077854fa60362d23aaa693001e79541de8990b98e3a5b29b53dbd79c3
    reason: This artifact remains the extended import profile. Existing plain-text, worksheet, column and preview constraints still apply; the separately scoped create-only profile has its own limits.
  - id: SB-ISSUER-IDENTITY-CHANGE-HISTORICAL-INVOICES
    cause: BR-PARTY-SNAPSHOT
    relationship: illustrates
    digest: sha256:465ae9a486b56de625a3f406a952fd9f806fad48ffb827984c41b12275206458
    cause-digest: sha256:eec706ec68420337799bb8bcbeb1c690876f8c071a823de6e087387d8f3f6ac5
    reason: Only status is promoted; the complete later issuer-review and immutable-snapshot rule body is preserved, including the distinct-second-administrator decision.
  - id: TERM-AUTOMATIC-PROJECT-NOTE
    cause: BC-CONTACTS
    relationship: defined-in
    digest: sha256:01d9aabceef53ac350fd8aa83b03c9c56b689e9a29050c7c1fe081f577d45cdb
    cause-digest: sha256:48d05be4499e68dc216bca722744448025ac982a2be5fc0d3726d6b289c97977
    reason: Directory, people and organization remain within Contacts; no issuer, registry, commercial-default or project-selection workflow is added or changed.
  - id: TERM-AUTOMATIC-PROJECT-NOTE
    cause: TERM-CONTACT
    relationship: uses-terms
    digest: sha256:01d9aabceef53ac350fd8aa83b03c9c56b689e9a29050c7c1fe081f577d45cdb
    cause-digest: sha256:765bb0dbb94cad61730e1e3b91034b2b45afded759fa10ab67cddec377e4d880
    reason: The contact remains the same stable selectable party; optional filing and directory maintenance do not change this consumer’s address, project, finance or reference semantics.
  - id: TERM-BILLING-ADDRESS
    cause: BC-CONTACTS
    relationship: defined-in
    digest: sha256:192c3b5eebc9dac27e4a1dfdea2afd8a97d9964b1b2ce7951109189793a5c8b5
    cause-digest: sha256:48d05be4499e68dc216bca722744448025ac982a2be5fc0d3726d6b289c97977
    reason: Directory, people and organization remain within Contacts; no issuer, registry, commercial-default or project-selection workflow is added or changed.
  - id: TERM-BILLING-ADDRESS
    cause: TERM-CONTACT
    relationship: uses-terms
    digest: sha256:192c3b5eebc9dac27e4a1dfdea2afd8a97d9964b1b2ce7951109189793a5c8b5
    cause-digest: sha256:765bb0dbb94cad61730e1e3b91034b2b45afded759fa10ab67cddec377e4d880
    reason: The contact remains the same stable selectable party; optional filing and directory maintenance do not change this consumer’s address, project, finance or reference semantics.
  - id: TERM-CONTACT-COMMERCIAL-DEFAULTS
    cause: BC-CONTACTS
    relationship: defined-in
    digest: sha256:8c758c17aa93b1fdb5a68beb216a032008ef8350e09a4de82309ebc03084e390
    cause-digest: sha256:48d05be4499e68dc216bca722744448025ac982a2be5fc0d3726d6b289c97977
    reason: Directory, people and organization remain within Contacts; no issuer, registry, commercial-default or project-selection workflow is added or changed.
  - id: TERM-CONTACT-COMMERCIAL-DEFAULTS
    cause: TERM-CONTACT
    relationship: uses-terms
    digest: sha256:8c758c17aa93b1fdb5a68beb216a032008ef8350e09a4de82309ebc03084e390
    cause-digest: sha256:765bb0dbb94cad61730e1e3b91034b2b45afded759fa10ab67cddec377e4d880
    reason: The contact remains the same stable selectable party; optional filing and directory maintenance do not change this consumer’s address, project, finance or reference semantics.
  - id: TERM-CONTACT-COMMERCIAL-DEFAULTS
    cause: TERM-CUSTOMER
    relationship: uses-terms
    digest: sha256:8c758c17aa93b1fdb5a68beb216a032008ef8350e09a4de82309ebc03084e390
    cause-digest: sha256:3e5e2b709352e3374527d591cea5fb66178c7b1ed02a77d3f63a5a36ee530085
    reason: The customer remains a contact role usable on projects and invoices; legal readiness, address data and financial defaults retain their own rules.
  - id: TERM-CONTACT-WARNING
    cause: BC-CONTACTS
    relationship: defined-in
    digest: sha256:492c6ef65e5e01b1353617530f5c19ae2d0e8857224af4233fb5dfbc85808b1b
    cause-digest: sha256:48d05be4499e68dc216bca722744448025ac982a2be5fc0d3726d6b289c97977
    reason: Directory, people and organization remain within Contacts; no issuer, registry, commercial-default or project-selection workflow is added or changed.
  - id: TERM-CONTACT-WARNING
    cause: TERM-CONTACT
    relationship: uses-terms
    digest: sha256:492c6ef65e5e01b1353617530f5c19ae2d0e8857224af4233fb5dfbc85808b1b
    cause-digest: sha256:765bb0dbb94cad61730e1e3b91034b2b45afded759fa10ab67cddec377e4d880
    reason: The contact remains the same stable selectable party; optional filing and directory maintenance do not change this consumer’s address, project, finance or reference semantics.
  - id: TERM-CREW-LIST
    cause: TERM-RECORD-FOLDER
    relationship: uses-terms
    digest: sha256:ebffcfa57ddcd7307069c12023e6c466ef0eb91a17092cf568f55c6249792e8c
    cause-digest: sha256:84a8bbb6da5aab3d49d19b3b4af8040bc59cef97e91215840b5183d5e687ee9a
    reason: Only Contacts gains optional membership. Crew, vehicles, crew lists and resource imports retain their own folder requirements and all modules retain separate folder structures.
  - id: TERM-CREW-MEMBER
    cause: TERM-RECORD-FOLDER
    relationship: uses-terms
    digest: sha256:be1b9db1fba459e769e76964f44ebf5b4dc94b31f75ee257b060b6f2604b492e
    cause-digest: sha256:84a8bbb6da5aab3d49d19b3b4af8040bc59cef97e91215840b5183d5e687ee9a
    reason: Only Contacts gains optional membership. Crew, vehicles, crew lists and resource imports retain their own folder requirements and all modules retain separate folder structures.
  - id: TERM-CUSTOMER-FINANCIAL-DEFAULTS
    cause: TERM-CUSTOMER
    relationship: uses-terms
    digest: sha256:0fdd0638179f9987866f5095962d76a1c6b0efa38aaf65b3d8b1e671c86d2004
    cause-digest: sha256:3e5e2b709352e3374527d591cea5fb66178c7b1ed02a77d3f63a5a36ee530085
    reason: The customer remains a contact role usable on projects and invoices; legal readiness, address data and financial defaults retain their own rules.
  - id: TERM-ISSUER
    cause: BC-CONTACTS
    relationship: defined-in
    digest: sha256:8d7ebcc9579d815793cf84c1f42fc9f9e8c9104fa8a636c1c65252c578f8dc3f
    cause-digest: sha256:48d05be4499e68dc216bca722744448025ac982a2be5fc0d3726d6b289c97977
    reason: Directory, people and organization remain within Contacts; no issuer, registry, commercial-default or project-selection workflow is added or changed.
  - id: TERM-MAILING-ADDRESS
    cause: BC-CONTACTS
    relationship: defined-in
    digest: sha256:78039a0d30898aecccf531e00c6c1f4b4580294141cd18d519f8773ff3d62414
    cause-digest: sha256:48d05be4499e68dc216bca722744448025ac982a2be5fc0d3726d6b289c97977
    reason: Directory, people and organization remain within Contacts; no issuer, registry, commercial-default or project-selection workflow is added or changed.
  - id: TERM-MAILING-ADDRESS
    cause: TERM-CONTACT
    relationship: uses-terms
    digest: sha256:78039a0d30898aecccf531e00c6c1f4b4580294141cd18d519f8773ff3d62414
    cause-digest: sha256:765bb0dbb94cad61730e1e3b91034b2b45afded759fa10ab67cddec377e4d880
    reason: The contact remains the same stable selectable party; optional filing and directory maintenance do not change this consumer’s address, project, finance or reference semantics.
  - id: TERM-NUMBER-SERIES
    cause: TERM-CONTACT-NUMBER
    relationship: uses-terms
    digest: sha256:dff06afe7502e618b279c14c6f37ee5c5efc3b7a57744c65a46e9db9751ef449
    cause-digest: sha256:d1b38eeed72be6d63b7b21932324eaaf74a8fb6483a1b5f748cd43a62aaf7719
    reason: The identifier remains a stable unique contact number assigned when omitted. No configurable series or invoice/project numbering policy changes.
  - id: TERM-PROJECT
    cause: TERM-CUSTOMER
    relationship: uses-terms
    digest: sha256:340b6c47620e6ee28d1e5e1c3ec4d3bdc8b43a13111abc1d13c0b0925f8cad70
    cause-digest: sha256:3e5e2b709352e3374527d591cea5fb66178c7b1ed02a77d3f63a5a36ee530085
    reason: The customer remains a contact role usable on projects and invoices; legal readiness, address data and financial defaults retain their own rules.
  - id: TERM-PROJECT-LOCATION
    cause: TERM-VENUE
    relationship: uses-terms
    digest: sha256:f360818bde0fa3e171c2cfafd0984eda615a8bd81a96e158a7dabea593a0d15c
    cause-digest: sha256:6349295a8e46a12f3799c590ade9dd6a67a99c8a6b03594c3ede3b3b2ad023f2
    reason: A venue remains a location contact, including a separate contact per distinct area. Directory promotion does not change project-location or per-area identity.
  - id: TERM-RESOURCE-IMPORT
    cause: TERM-RECORD-FOLDER
    relationship: uses-terms
    digest: sha256:3fea87668135511ccf097d539da22149cde907a7fd172971567925496c203e3d
    cause-digest: sha256:84a8bbb6da5aab3d49d19b3b4af8040bc59cef97e91215840b5183d5e687ee9a
    reason: Only Contacts gains optional membership. Crew, vehicles, crew lists and resource imports retain their own folder requirements and all modules retain separate folder structures.
  - id: TERM-TAX-ID
    cause: BC-CONTACTS
    relationship: defined-in
    digest: sha256:063baf6ba034d6a8f1db9e5292d7a33a8093bee3713298b6020a848325f26836
    cause-digest: sha256:48d05be4499e68dc216bca722744448025ac982a2be5fc0d3726d6b289c97977
    reason: Directory, people and organization remain within Contacts; no issuer, registry, commercial-default or project-selection workflow is added or changed.
  - id: TERM-TRAVEL-ESTIMATE
    cause: BC-CONTACTS
    relationship: defined-in
    digest: sha256:d906eddf131432098775963b7f33cc099582a51c5ca8e2da06c31932eec77777
    cause-digest: sha256:48d05be4499e68dc216bca722744448025ac982a2be5fc0d3726d6b289c97977
    reason: Directory, people and organization remain within Contacts; no issuer, registry, commercial-default or project-selection workflow is added or changed.
  - id: TERM-VEHICLE
    cause: TERM-RECORD-FOLDER
    relationship: uses-terms
    digest: sha256:91b0ed455aa88293bd9c9c3d81d5c8b472b941c79e6f15777e9b6fb35de246b7
    cause-digest: sha256:84a8bbb6da5aab3d49d19b3b4af8040bc59cef97e91215840b5183d5e687ee9a
    reason: Only Contacts gains optional membership. Crew, vehicles, crew lists and resource imports retain their own folder requirements and all modules retain separate folder structures.
  - id: TERM-VISITING-ADDRESS
    cause: BC-CONTACTS
    relationship: defined-in
    digest: sha256:642668716bd32eb3598539dd4a9e1c2aeb798e745ab860ec4ddc0b16339a3705
    cause-digest: sha256:48d05be4499e68dc216bca722744448025ac982a2be5fc0d3726d6b289c97977
    reason: Directory, people and organization remain within Contacts; no issuer, registry, commercial-default or project-selection workflow is added or changed.
  - id: TERM-VISITING-ADDRESS
    cause: TERM-CONTACT
    relationship: uses-terms
    digest: sha256:642668716bd32eb3598539dd4a9e1c2aeb798e745ab860ec4ddc0b16339a3705
    cause-digest: sha256:765bb0dbb94cad61730e1e3b91034b2b45afded759fa10ab67cddec377e4d880
    reason: The contact remains the same stable selectable party; optional filing and directory maintenance do not change this consumer’s address, project, finance or reference semantics.
  - id: UC-CORRECT-ISSUED-INVOICE
    cause: BR-PARTY-SNAPSHOT
    relationship: governed-by
    digest: sha256:7eda3289003ca4f8b4a5c5afcd99fa5bae449d4a871438b7576deb49adb35980
    cause-digest: sha256:eec706ec68420337799bb8bcbeb1c690876f8c071a823de6e087387d8f3f6ac5
    reason: Only status is promoted; the complete later issuer-review and immutable-snapshot rule body is preserved, including the distinct-second-administrator decision.
  - id: UC-CREATE-PROJECT
    cause: TERM-CUSTOMER
    relationship: uses-terms
    digest: sha256:2416d85758de14ca9785a48a1aa604ba6fbba2b85b520092035d55a407e51a10
    cause-digest: sha256:3e5e2b709352e3374527d591cea5fb66178c7b1ed02a77d3f63a5a36ee530085
    reason: The customer remains a contact role usable on projects and invoices; legal readiness, address data and financial defaults retain their own rules.
  - id: UC-DEFINE-ISSUER-PROFILE
    cause: BC-CONTACTS
    relationship: bounded-context
    digest: sha256:fd058a02a9f6f90de16249ea291336c0193cfefce4236e0081a00f6da3223b34
    cause-digest: sha256:48d05be4499e68dc216bca722744448025ac982a2be5fc0d3726d6b289c97977
    reason: Directory, people and organization remain within Contacts; no issuer, registry, commercial-default or project-selection workflow is added or changed.
  - id: UC-DEFINE-ISSUER-PROFILE
    cause: BR-PARTY-SNAPSHOT
    relationship: governed-by
    digest: sha256:fd058a02a9f6f90de16249ea291336c0193cfefce4236e0081a00f6da3223b34
    cause-digest: sha256:eec706ec68420337799bb8bcbeb1c690876f8c071a823de6e087387d8f3f6ac5
    reason: Only status is promoted; the complete later issuer-review and immutable-snapshot rule body is preserved, including the distinct-second-administrator decision.
  - id: UC-EQP-IMPORT-EQUIPMENT
    cause: BR-PRJ-IMPORT-FOLDER-PLACEMENT
    relationship: governed-by
    digest: sha256:04045c947021522a7381e7289dc2ef128763188b8e4179969c4bcb4a7335324c
    cause-digest: sha256:5cebb8045cdd3bd977a07cde9450749d700eb9771ea62b39de6427a623aa0ffc
    reason: The exception is restricted to create-only Contacts import. Equipment and crew/resource import placement remains unchanged.
  - id: UC-EQP-IMPORT-EQUIPMENT
    cause: BR-PRJ-IMPORT-RESULT-REPORT
    relationship: governed-by
    digest: sha256:04045c947021522a7381e7289dc2ef128763188b8e4179969c4bcb4a7335324c
    cause-digest: sha256:982bf712ffcf4e8eb875a60c9a8cb3e8f6a1625ceedd9a706a6cd4e80b7d4f26
    reason: The exception is restricted to create-only Contacts import. Equipment, project-equipment and task imports retain their existing downloadable failed-row reports.
  - id: UC-EQP-IMPORT-EQUIPMENT
    cause: BR-PRJ-IMPORT-TAG
    relationship: governed-by
    digest: sha256:04045c947021522a7381e7289dc2ef128763188b8e4179969c4bcb4a7335324c
    cause-digest: sha256:28796997820643251297eb67b65908701248c1d69550dfd01ac3621450737479
    reason: The exception is restricted to create-only Contacts import. Equipment and task imports retain their tagging and existing undo/archive behavior.
  - id: UC-FIN-SET-CUSTOMER-FINANCIAL-DEFAULTS
    cause: TERM-CUSTOMER
    relationship: uses-terms
    digest: sha256:b3d4e44d150c5ea0715f02dca7a166993517015f6b66f5ee8ba7eca3b3a80383
    cause-digest: sha256:3e5e2b709352e3374527d591cea5fb66178c7b1ed02a77d3f63a5a36ee530085
    reason: The customer remains a contact role usable on projects and invoices; legal readiness, address data and financial defaults retain their own rules.
  - id: UC-INT-LOOK-UP-COMPANY-DETAILS
    cause: BC-CONTACTS
    relationship: bounded-context
    digest: sha256:4c2cbc0eddd217be0fb7930706a2cb2b58780efb8c11e3940468f789cc8d4758
    cause-digest: sha256:48d05be4499e68dc216bca722744448025ac982a2be5fc0d3726d6b289c97977
    reason: Directory, people and organization remain within Contacts; no issuer, registry, commercial-default or project-selection workflow is added or changed.
  - id: UC-ISSUE-INVOICE
    cause: BR-PARTY-SNAPSHOT
    relationship: governed-by
    digest: sha256:342ce2d34a022b8a621fd91efff10751c2dd841e6f42b28c5530dbc5ead7eb63
    cause-digest: sha256:eec706ec68420337799bb8bcbeb1c690876f8c071a823de6e087387d8f3f6ac5
    reason: Only status is promoted; the complete later issuer-review and immutable-snapshot rule body is preserved, including the distinct-second-administrator decision.
  - id: UC-PRJ-BULK-EDIT-CONTACTS
    cause: BC-CONTACTS
    relationship: bounded-context
    digest: sha256:9654763f344f91aaee9312ccab8a26439f3366cb6f7460f69d8f57816397ff22
    cause-digest: sha256:48d05be4499e68dc216bca722744448025ac982a2be5fc0d3726d6b289c97977
    reason: Directory, people and organization remain within Contacts; no issuer, registry, commercial-default or project-selection workflow is added or changed.
  - id: UC-PRJ-BULK-EDIT-CONTACTS
    cause: TERM-CONTACT
    relationship: uses-terms
    digest: sha256:9654763f344f91aaee9312ccab8a26439f3366cb6f7460f69d8f57816397ff22
    cause-digest: sha256:765bb0dbb94cad61730e1e3b91034b2b45afded759fa10ab67cddec377e4d880
    reason: The contact remains the same stable selectable party; optional filing and directory maintenance do not change this consumer’s address, project, finance or reference semantics.
  - id: UC-PRJ-EDIT-PROJECT-DETAILS
    cause: BR-PRJ-PARTIES-FROM-CONTACTS
    relationship: governed-by
    digest: sha256:3a850f131e9a6add0649f4c24c472b392dc2e4c7543e349dda3c3282ef9d09a0
    cause-digest: sha256:b539abeef519e679b5a5da2cebcbe60bc04796718262d3489e703f5f57100344
    reason: The same contact directory still supplies project parties; project editing and saved associations retain their existing semantics.
  - id: UC-PRJ-EDIT-PROJECT-DETAILS
    cause: TERM-CONTACT-PERSON
    relationship: uses-terms
    digest: sha256:3a850f131e9a6add0649f4c24c472b392dc2e4c7543e349dda3c3282ef9d09a0
    cause-digest: sha256:802b751544c3ca24816a0e7e26ea093b6bde5497fba603fbb55b8246d300914d
    reason: People remain owned by one contact; zero-or-one designations and atomic reassignment do not change later explicit project selection or invoice recipient policy.
  - id: UC-PRJ-EDIT-PROJECT-DETAILS
    cause: TERM-CUSTOMER
    relationship: uses-terms
    digest: sha256:3a850f131e9a6add0649f4c24c472b392dc2e4c7543e349dda3c3282ef9d09a0
    cause-digest: sha256:3e5e2b709352e3374527d591cea5fb66178c7b1ed02a77d3f63a5a36ee530085
    reason: The customer remains a contact role usable on projects and invoices; legal readiness, address data and financial defaults retain their own rules.
  - id: UC-PRJ-IMPORT-EQUIPMENT-LIST
    cause: BR-PRJ-IMPORT-RESULT-REPORT
    relationship: governed-by
    digest: sha256:39624ff024d5010540f7178a288d05f0edb296dc3ff7624819bad4156b7aba8e
    cause-digest: sha256:982bf712ffcf4e8eb875a60c9a8cb3e8f6a1625ceedd9a706a6cd4e80b7d4f26
    reason: The exception is restricted to create-only Contacts import. Equipment, project-equipment and task imports retain their existing downloadable failed-row reports.
  - id: UC-PRJ-IMPORT-TASKS
    cause: BR-PRJ-IMPORT-RESULT-REPORT
    relationship: governed-by
    digest: sha256:198cae90fe34ea977f1a6021a5990c59ed71d848bdfb183cea9fb085f8f13701
    cause-digest: sha256:982bf712ffcf4e8eb875a60c9a8cb3e8f6a1625ceedd9a706a6cd4e80b7d4f26
    reason: The exception is restricted to create-only Contacts import. Equipment, project-equipment and task imports retain their existing downloadable failed-row reports.
  - id: UC-PRJ-IMPORT-TASKS
    cause: BR-PRJ-IMPORT-TAG
    relationship: governed-by
    digest: sha256:198cae90fe34ea977f1a6021a5990c59ed71d848bdfb183cea9fb085f8f13701
    cause-digest: sha256:28796997820643251297eb67b65908701248c1d69550dfd01ac3621450737479
    reason: The exception is restricted to create-only Contacts import. Equipment and task imports retain their tagging and existing undo/archive behavior.
  - id: UC-PRJ-SET-CONTACT-COMMERCIAL-DEFAULTS
    cause: BC-CONTACTS
    relationship: bounded-context
    digest: sha256:e798a64b5810d27f4322c6f6e7240aa007a8797d0be79d22d25b791a17b8e7a3
    cause-digest: sha256:48d05be4499e68dc216bca722744448025ac982a2be5fc0d3726d6b289c97977
    reason: Directory, people and organization remain within Contacts; no issuer, registry, commercial-default or project-selection workflow is added or changed.
---

## Problem

The approved Contacts directory, contact-person, designation-cardinality, spreadsheet-import and organization changes were applied on a historical delivery branch rather than this accepted baseline. Current generic import and folder artifacts also conflict with the approved bounded Contacts profiles.

## Intended Product Outcome

The accepted graph expresses CHG-CONTACTS-DIRECTORY-001, CHG-CONTACT-PERSONS-299, CHG-CONTACT-PERSON-DESIGNATION-CARDINALITY-299, CHG-CONTACT-SPREADSHEET-IMPORT-301 and CHG-CONTACT-ORGANIZATION-302 coherently with the current v1alpha2 baseline. Existing IDs and later baseline decisions remain intact. Contacts directory maintenance, people and atomic designation changes, create-only import, optional folders and tag filters have explicit requirements and behaviour examples.

## Rationale

The owner approved the existing written intent and proposals in issue #437 and explicitly requested reconciliation and application on 2026-11-16. The historical proposals provide the intended semantics; this change reconciles their scope and relationship serialization rather than restoring an old model wholesale. Narrow creation/import entry points and the broader dedicated maintenance/profile cases retain their distinct scopes. General extended-import rules do not expand UC-CON-IMPORT-CONTACTS. Folder deletion safety does not imply mandatory contact filing. Organization isolation follows the existing separate-installation boundary.

## Affected Product Areas

Contacts directory, stable contact numbers, customers/suppliers/venues, contact people, designation cardinality, spreadsheet validation and privacy, contact tags/folders/filtering, and their relationships to project contact selection and immutable invoice snapshots.

## Open Questions

None. The existing owner-approved issue criteria and historical decisions resolve these product choices; no new owner approval is requested.

## Product Acceptance

- FR-CON-001 expresses role-aware directory maintenance, stable numbering, pagination and issued-snapshot preservation.
- FR-CON-002 and BR-CON-CONTACT-PERSON-DESIGNATIONS express zero-or-one of each designation, both flags on one person when selected, atomic reassignment, parent ownership and unchanged project/invoice history.
- FR-CON-003 and CON-CON-IMPORT-FILE-FORMAT express create-only CSV/single-worksheet XLSX import, at most 64 columns, a 10-row preview, a 10 MiB upload bound, digest recheck, validation before writes, row atomicity and privacy-preserving audit. Updates, contact persons, tags/folders/custom fields, undo and downloadable rejections remain outside that profile.
- FR-CON-004 and FR-CON-005 express Contacts-only tags, any/all filtering, optional single-folder membership, hierarchical maintenance and refusal to delete a folder with contacts or children.
- BR-PRJ-INVOICE-RECIPIENT remains unchanged; this change neither selects project contact people nor sends invoices. Existing party-snapshot and issuer-history decisions remain intact.
- Existing generic import and folder artifacts explicitly respect the bounded Contacts profiles; other record types retain their existing semantics. Organization isolation remains separate deployments/databases.

## Out of Scope

Application implementation, delivery acceptance evidence, new permission roles, tenant columns or client-selected organization IDs, contact-person removal, attachments/custom fields, project selection, recipient delivery, commercial defaults, configurable number-series policy, registry lookup, unrelated product changes, release, deployment and production data.
