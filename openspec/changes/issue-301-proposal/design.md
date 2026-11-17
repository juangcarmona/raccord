---
pdac-scope: cited
---

# Design

## Context

See proposal and spec. The planning-time main revision lacked #443 runtime. Delivery began after its qualified merge `166fd329cc8f312137ecc0abb81052a1ee8793dc` and serial #437 activation. Historical source packets at `6486327b3db5dda4062a79ba77e4745cd497fea9` supplied reusable deltas; current implementation and verification are recorded in tasks and verification evidence.
<!-- pdac:cite id="UC-CON-IMPORT-CONTACTS" digest="sha256:9a964862aa671bf1c5f9648286ecf92c5ab60631ed7b30c02c3de8be38a3d587" -->

## Goals / Non-Goals

**Goals:** Stateless review, reliable row-level isolation and minimal audit using existing Contacts boundaries.

**Non-Goals:** Background jobs, retained uploads, a new database model, product row limit or new external parser dependency.

## Decisions

- Adapt historical platform CSV/XLSX readers and DTOs to the qualified Contacts base; merge only needed host, routes, localization and client deltas. Keep parsing outside the core domain, respect module ports and authenticated root host. Alternative: replace host files or restore historical model wholesale; rejected because that would overwrite newer runtime and product decisions. No architecture deviation is intended.
- Use inspect, preview and confirm multipart endpoints protected by contact-create permission. Preview returns digest, sample and all-row validation; confirmation resubmits file and mapping and recomputes digest and validation. UI invalidates preview whenever inputs change and removes ignored mappings. Alternative: retain server-side uploads; rejected to reduce sensitive-data retention, at the cost of reparsing.
- Route each valid row through existing Contacts creation with independent transaction and clean persistence context on an expected conflict; append start and outcome audit with import id, file type and counts. Prove rollback through the real API. Propagate cancellation and infrastructure failures rather than misreporting them as invalid rows. Alternative: one batch transaction would prevent accepted partial success.
- Validate archive and XML resource bounds, cell and row references; preserve sparse original row numbers without allocating blank gaps. No new product row-count limit. Avoid extracting files to disk or logging values.
<!-- pdac:cite id="CON-CON-IMPORT-FILE-FORMAT" digest="sha256:f33969383814d4ba04d00eadd559a0dd470c67958d33cfc42ff6a8aded62f9fb" -->
<!-- pdac:cite id="BR-CON-IMPORT-ROW-ATOMICITY" digest="sha256:f6c233e920161353d0bdac7a81156a1fc58dca933546a8da314593dc4b61b2b8" -->
<!-- pdac:cite id="BR-CON-IMPORT-AUDIT" digest="sha256:bfa968766a4f0c22c180c9c6bad1742062be10f37b375f15c6e5058aded4327f" -->

## Risks / Trade-offs

- Interrupted imports can leave already committed complete rows → expose failure and start audit; users inspect the directory before retrying.
- Parser expansion and malformed sparse references → bounded archive/XML processing and regression tests without gap allocation.
- A digest alone does not bind mapping → invalidate UI preview on mapping changes and revalidate mappings at confirmation; exercise forged requests at the API boundary.
- A transaction failure can poison a shared persistence context → verify independent rollback and subsequent success with a real database.

## Migration Plan

No schema migration is planned. Release UI and API together after separate authorization; application rollback removes the entry point but does not delete contacts or append-only audit records.
