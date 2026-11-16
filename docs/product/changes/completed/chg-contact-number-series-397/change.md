---
id: CHG-CONTACT-NUMBER-SERIES-397
type: product-change
title: Configure contact-number assignment in Workspace
status: applied
base-revision: "b9fec25c8cf85a4ab00326d7be59303e0b3cee48"
operations:
  add:
    - UC-WSP-CONFIGURE-CONTACT-NUMBER-SERIES
    - FR-WSP-030
    - BR-WSP-CONTACT-NUMBER-SERIES-CONFIGURATION
    - SB-WSP-CONTACT-NUMBER-SERIES-ALLOCATION
  modify:
    - BR-PRJ-CONTACT-NUMBER-ASSIGNMENT
  remove: []
unaffected:
  - id: FR-CON-001
    cause: BR-PRJ-CONTACT-NUMBER-ASSIGNMENT
    relationship: derived-from
    digest: sha256:a7e73bb8c5c496226d8074b5fda85a5f974703596c57ee0f7b10c49eba3e040e
    cause-digest: sha256:684271aa27bb6ff4d2511637e71da034b5a1868e9317325dc894f807574a4a1e
    reason: Unique automatic numbering and immutable invoice snapshots remain required. This requirement delegates number allocation to the same rule; configurable future formatting does not change directory roles, pagination or saved invoice parties.
  - id: FR-PRJ-032
    cause: BR-PRJ-CONTACT-NUMBER-ASSIGNMENT
    relationship: derived-from
    digest: sha256:02cbb88a4887b9be9dbed989535f4d813d77037c1f59f5df40ad73c4783d6cd8
    cause-digest: sha256:684271aa27bb6ff4d2511637e71da034b5a1868e9317325dc894f807574a4a1e
    reason: Contacts keep their existing number, addresses, persons and roles. Future series configuration changes no saved reference, designation or invoice recipient selection; allocation remains governed by the existing rule.
  - id: UC-CON-MANAGE-CONTACT-DIRECTORY
    cause: BR-PRJ-CONTACT-NUMBER-ASSIGNMENT
    relationship: governed-by
    digest: sha256:74f3312e87cd726c7f1ec573a707571e7f3ad7367e20d59f4281da5ae04c3581
    cause-digest: sha256:684271aa27bb6ff4d2511637e71da034b5a1868e9317325dc894f807574a4a1e
    reason: The same authorized create flow assigns the next unique contact number when omitted and rejects duplicates. Settings remain in Workspace; browsing, editing and invoice-snapshot preservation are unchanged.
  - id: UC-PRJ-ADD-CONTACT
    cause: BR-PRJ-CONTACT-NUMBER-ASSIGNMENT
    relationship: governed-by
    digest: sha256:0f4847d4b221fde734392577cbb664d6b3da941fe13c01902063654466e95f84
    cause-digest: sha256:684271aa27bb6ff4d2511637e71da034b5a1868e9317325dc894f807574a4a1e
    reason: Optional supplied numbers, automatic next-free allocation and duplicate rejection remain the same observable creation contract. Configurable formatting adds no step, required folder or other project/commercial behavior.
---

## Problem

The accepted contact-directory behavior assigns unique numbers from a contact-number series, but the product model does not define how Workspace administrators configure that series. Contact numbers are currently allocated by a fixed sequence, so teams cannot align new references with their existing numbering policy.

## Intended Product Outcome

Workspace administrators can configure the contact-number series used for future contacts. The proposed settings are an optional static prefix and the current number; new workspaces default to an empty prefix and current number zero, so the first automatically assigned number is 1. Allocation advances by one, remains unique under concurrent creation, and skips values already in use. Changing the settings never changes existing contact numbers. A caller may still supply a custom contact number, subject to uniqueness.

## Rationale

A configurable contact series lets teams continue their established references while retaining stable contact numbers for documents and historical records.

## Affected Product Areas

Workspace numbering settings, contact creation, contact-number uniqueness and continuity.

## Open Questions

None. The owner approved the existing recommendations and written scope through #437 on 2026-11-15.

## Owner Decisions and Recovery

- Format: one optional static prefix plus an unpadded decimal sequence, advancing by one; no date tokens or custom padding.
- Defaults: empty prefix and current number zero for new installations; existing installations initialize the current number from their highest numeric contact number without rewriting any existing number.
- Custom supplied values remain allowed when unique; automatic allocation skips occupied values and concurrent creation cannot produce duplicates.
- Concurrent settings edits reject the stale save and require reload/retry, as the original use case already states.
- This is the same CHG-CONTACT-NUMBER-SERIES-397 proposed in PR #399 at dfc162a7f697caa03d9cd6a542bb982a32fa016c. Reconciliation preserves the current v1alpha2 relationship typing and later Contacts/invoice decisions.
- Native preparation43818486954 published a blocked assessment because its host report serializer exceeded a byte limit. This operator recovery uses the complete installed ProductShape provider graph directly; it does not claim a native passed result or current-head native approval binding.
- Approval authorizes explicit provider application on this working branch. Acceptance into main, runtime implementation and merge remain separate.

## Product Acceptance

- The Product Owner confirms the supported series format and the defaults for new and existing workspaces.
- The Product Owner confirms that configuration affects future allocations only and that contact numbers remain unique under concurrent creation.
- The accepted Product Definition agrees with #397's requirement to preserve all existing contact numbers.

## Out of Scope

Project, quote, contract and invoice series; changing existing contact numbers; bulk renumbering; custom numeric padding, date tokens or multiple contact series; implementation, technical design, release or deployment.
