<!-- pdac-scope: cited -->

## Why

Contacts users need reusable tags and a folder hierarchy to classify records and narrow the directory within their workspace. Issue #302 calls for contact-only organization and filters, which are now defined by applied Product Change `CHG-CONTACT-ORGANIZATION-302`. Under ADR-0012, the current product serves one company per deployment, so this change uses the deployment database as the isolation boundary.

## What Changes

- Add contact tags, tag assignment/removal and directory filters supporting any/all matching in the active deployment workspace.
- Add a Contacts-only folder tree with create, rename, reorder, move, assignment and safe deletion of empty folders.
- Add authenticated API and UI flows, persist organization data inside the existing database-per-deployment boundary, and cover the complete synthetic workflow in browser E2E. Do not add a tenant/workspace column, multi-company tenancy or client-selected workspace id.
- No API or database contracts outside Contacts change; no new production dependency.

## Capabilities

### New Capabilities

- `contacts-tags-and-filters`: Manage contact tags and filter the active deployment's Contacts directory by one or more tags using any/all matching.
- `contacts-folders`: Manage the active deployment's folder hierarchy and optionally place each contact in one folder.

### Modified Capabilities

None. Existing OpenSpec specifications do not define Contacts organization behavior.

## Impact

Affected code includes the Contacts module, EF persistence and migrations, Contacts API, generated OpenAPI client, Contacts directory UI, E2E selection and architecture/process documentation. The deployment database remains the company isolation boundary; the APIs do not accept a client-selected workspace id. No breaking behavior is intended.

## Product definition impact

The complete relevant dependency set is the applied Product Change `CHG-CONTACT-ORGANIZATION-302`: `UC-CON-TAG-CONTACTS`, `UC-CON-MANAGE-CONTACT-FOLDERS`, `UC-CON-FILTER-CONTACT-DIRECTORY`, `FR-CON-004`, `FR-CON-005`, `BR-CON-CONTACT-TAG-SCOPE`, `BR-CON-TAG-FILTER-MODES`, `BR-CON-FOLDER-HIERARCHY`, `BR-CON-FOLDER-DELETE-NONEMPTY`, `TERM-CONTACT-TAG`, `TERM-CONTACT-FOLDER`, `SB-CON-TAG-FILTER-ANY-AND-ALL`, `SB-CON-CONTACTS-ISOLATED-BY-WORKSPACE` and `SB-CON-ORGANIZE-AND-FILTER-CONTACTS`. The connected Contacts bounded context, actor and `TERM-CONTACT` were checked as neighbours; they remain unchanged.

<!-- pdac:cite id="UC-CON-TAG-CONTACTS" digest="sha256:c19ff9af883fc97b09e564a63540df53b6235343c791288bfdd1d8cae856f912" -->
<!-- pdac:cite id="UC-CON-MANAGE-CONTACT-FOLDERS" digest="sha256:64bf01e23b42803074aa40b1c666ecfc1c2a1fbd0b453c4d15c2c11c4572c082" -->
<!-- pdac:cite id="UC-CON-FILTER-CONTACT-DIRECTORY" digest="sha256:8c11b6106c10adc68b9e495e57ed5bdd3e1763b98b1033c7cc4c9614770413a9" -->
<!-- pdac:cite id="FR-CON-004" digest="sha256:a492f73718d6ae6fffae47c5aaa562c092ab8a86e167c5a1f167b667ad55cfe6" -->
<!-- pdac:cite id="FR-CON-005" digest="sha256:351710f9bb90a6e303b5eb4a6a6a2d09df952aeb3b6d69d620030a507ac18bff" -->
<!-- pdac:cite id="BR-CON-CONTACT-TAG-SCOPE" digest="sha256:08680cad6deb2e72201f7a7d3585a8197131d694897c9b0c56e824b0b909f739" -->
<!-- pdac:cite id="BR-CON-TAG-FILTER-MODES" digest="sha256:fbdc5c466fa3c20260000a51e7105b01cab9002409efcae7bd795d2ba83f1966" -->
<!-- pdac:cite id="BR-CON-FOLDER-HIERARCHY" digest="sha256:9607b972d82930e5ca57988dc52ec26241b8b9104bc7cd3a6eeada76251dd0c7" -->
<!-- pdac:cite id="BR-CON-FOLDER-DELETE-NONEMPTY" digest="sha256:3a1f1955d9faa33b90388cd7c56418adf2c2c5dd468b44878edf731d5ff074bd" -->
<!-- pdac:cite id="TERM-CONTACT-TAG" digest="sha256:b25c8659937bc2f31987b618cbeb1384929165a9abf3db03f388587b06ace7ab" -->
<!-- pdac:cite id="TERM-CONTACT-FOLDER" digest="sha256:3fa98fa8813b1ddd9815bbd8536cb301286a943008f5454e04983ad9b7c7c54f" -->
<!-- pdac:cite id="SB-CON-TAG-FILTER-ANY-AND-ALL" digest="sha256:ee82a9793807a2a39857f77f49c6fb2ee0c21de3831a710d782d2db96c6e940f" -->
<!-- pdac:cite id="SB-CON-CONTACTS-ISOLATED-BY-WORKSPACE" digest="sha256:8f28e980e4bafd4fa79db9083f468bdd258d208c6d30d4d091a8886c65e95960" -->
<!-- pdac:cite id="SB-CON-ORGANIZE-AND-FILTER-CONTACTS" digest="sha256:a23f45220d361110b70224d3fe20f656fa4e46591f3840da6ff8ddeb955fa003" -->
