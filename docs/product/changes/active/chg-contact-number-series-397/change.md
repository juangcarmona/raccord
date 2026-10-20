---
id: CHG-CONTACT-NUMBER-SERIES-397
type: product-change
title: Configure contact-number assignment in Workspace
status: proposed
base-revision: "769edf87e11dec97b23de000741c2f745545256b"
operations:
  add:
    - UC-WSP-CONFIGURE-CONTACT-NUMBER-SERIES
    - FR-WSP-030
    - BR-WSP-CONTACT-NUMBER-SERIES-CONFIGURATION
    - SB-WSP-CONTACT-NUMBER-SERIES-ALLOCATION
  modify:
    - BR-PRJ-CONTACT-NUMBER-ASSIGNMENT
  remove: []
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

- Confirm the supported format: optional static text prefix plus an unpadded decimal sequence, incremented by one; no date tokens or zero-padding. Recommendation: accept this simple format for #397 and extend it later only through another Product Change.
- Confirm defaults for a new workspace and an existing workspace. Recommendation: new workspace uses empty prefix/current number zero; existing workspace initializes from its highest numeric contact number without renumbering any existing contact.
- Confirm that custom supplied contact numbers remain allowed and that auto-allocation skips values already present. Recommendation: preserve this behavior and enforce uniqueness across both custom and generated numbers.

## Product Acceptance

- The Product Owner confirms the supported series format and the defaults for new and existing workspaces.
- The Product Owner confirms that configuration affects future allocations only and that contact numbers remain unique under concurrent creation.
- The accepted Product Definition agrees with #397's requirement to preserve all existing contact numbers.

## Out of Scope

Project, quote, contract and invoice series; changing existing contact numbers; bulk renumbering; custom numeric padding, date tokens or multiple contact series; implementation, technical design, release or deployment.
