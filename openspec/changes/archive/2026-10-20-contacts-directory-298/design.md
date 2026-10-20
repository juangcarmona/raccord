<!-- pdac-scope: cited -->

## Context

`src/modules/Contacts/` currently models `Customer` only. `CustomerDirectory` creates, lists and reads `PartySnapshot` values; `CustomerRepository` persists `CustomerRow` in `contacts.customers`; `/api/customers` exposes create/list/read; `CustomersPage` has an unpaged list and `NewCustomerPage` only creates. Projects and invoicing depend on the customer ID, while `BR-PARTY-SNAPSHOT` and the issued invoice representation already provide the historical snapshot seam.

The issue requires adding multiple contact roles and company/person type without breaking customer selection or existing database relationships. No Contacts OpenSpec capability currently exists, so `contacts-directory` is introduced as the implementation contract for the approved Product Change.

<!-- pdac:cite id="BC-CONTACTS" digest="sha256:b63537573b968a16a585370ae0e4a98ec688de6e31e42e8b7e2dad5c89d3f474" -->
<!-- pdac:cite id="UC-CON-MANAGE-CONTACT-DIRECTORY" digest="sha256:58989f1d3dfd59bb6021eb503753e4f9bccd6bd57f4dc43697dddf535c057901" -->
<!-- pdac:cite id="BR-PARTY-SNAPSHOT" digest="sha256:5ac10d9c4ab5f68acd5a18213fe4c9c0d50786549a1d94c760409214b13355f7" -->

## Goals / Non-Goals

**Goals:** Shared Contact ownership in the Contacts module; company/person types; customer/supplier/venue role tags; uniquely assigned contact numbers; paginated and editable directory; compatibility for project and invoicing callers; preservation of existing IDs and issued invoice data; tests that drive browser-visible behavior.

**Non-Goals:** Contact persons, designations, commercial defaults, imports, generic tag/folder management, registry lookup, bulk edits, configurable Workspace number-series screens, invoice issuance changes, or changing the accepted Product Definition.

## Decisions

- Evolve the Contacts domain around one Contact identity with explicit type and role tags. Keep role tags as the three accepted role values; do not add user-defined tags or folders in this issue.
- Preserve the existing `contacts.customers` table and its primary key to avoid destructive renames and retain project foreign keys. Extend it with the accepted type, role and contact-number data; use a normalized role relation if the EF model requires it. Existing rows migrate as company/customer contacts with their original IDs.
- Keep the existing `/api/customers` contract as a compatibility projection of contacts carrying the customer role. Add a paginated `/api/contacts` contract for the directory, with detail and edit operations; project and invoice consumers continue to resolve the same stable IDs.
- Make the directory order deterministic by contact number and ID, and validate page/page-size bounds at the API boundary. Page size is a server default with a bounded requested size; no Workspace settings UI is added.
- Allocate contact numbers through a database-backed sequence in the same unit of work as contact creation. Database uniqueness remains the final concurrency guard. Existing rows receive deterministic unique numbers during migration, and the sequence starts after the backfilled range.
- Keep invoice issuance reading a current contact through `IPartyDirectory`; continue snapshotting recipient details at issue time. Editing a Contact must not update `IssuedInvoiceRow` or its immutable representation.
- Implement the UI as a Contacts feature slice and retain `/customers` navigation compatibility where current route links or journeys rely on it. Update API schema artifacts through the repository's generated-contract workflow.
- No production dependency or ADR is needed: the existing modular boundaries, EF Core persistence, React/Vite feature slices and Playwright strategy cover the change.

## Risks / Trade-offs

The existing customer ID is referenced by projects and invoice source records, so replacing it with a new identity would be unnecessarily risky; preserving IDs and the table boundary limits migration impact. Backfilling contact numbers is a one-time data transformation and needs a migration preservation test that checks both old identities and sequence continuation. The compatibility customer API risks accidental exposure of non-customer contacts, so every projection and selector must filter by the customer role. Invoice snapshot tests must inspect persisted issued representation after editing the contact, not infer immutability from the invoice's display badge alone.

## Product and architecture references

The ProductShape impact graph reaches `UC-ISSUE-INVOICE`, `UC-PRJ-ADD-CONTACT`, audit behavior, contact persons and commercial defaults. Invoice/project selection and audit compatibility are included in verification. Contact persons, commercial defaults, imports, user-defined tags/folders and configurable series are excluded because they exceed #298 and have separate product/backlog scope.

<!-- pdac:cite id="FR-CON-001" digest="sha256:9b09ae74057045edaba5e51c4d14f287a22093be7537c8867a4fee8e07ccaa5f" -->
<!-- pdac:cite id="UC-REGISTER-CUSTOMER" digest="sha256:1cbbec6400a8cf4165c0a7c82742d6e71e9e434e267e6cc3a2af8c575164670f" -->
<!-- pdac:cite id="BR-PRJ-PARTIES-FROM-CONTACTS" digest="sha256:2fe8ce59516db4c08c3582e1c3dd6341fa3b658e8386d10f725671b633b2dcfe" -->
<!-- pdac:cite id="BR-PRJ-CONTACT-NUMBER-ASSIGNMENT" digest="sha256:aa32e12b334cd585d5d946fb961a46d5ddfda39bc4b16320a9c5f0b19d7ce8d7" -->
<!-- pdac:cite id="BR-PRJ-CONTACT-ROLES-BY-TAG" digest="sha256:d7110eb5318168938b259d3f44fb022f04666454b5112e97d17539861b8604d9" -->
