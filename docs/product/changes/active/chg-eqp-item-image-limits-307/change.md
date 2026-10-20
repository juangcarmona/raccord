---
id: CHG-EQP-ITEM-IMAGE-LIMITS-307
type: product-change
title: Define safe Equipment image upload limits
status: proposed
base-revision: "5a92fa0cc2ad8841410e007f10391e469492915f"
operations:
    add:
        - CON-EQP-IMAGE-UPLOAD-LIMITS
        - FR-EQP-020
    modify:
        - UC-EQP-ADD-ITEM-IMAGE
    remove: []
---

## Problem

The accepted Equipment model lets a manager upload an item image but does not constrain accepted file types or sizes, nor define what happens to an invalid upload. Product requirements say Equipment images use the document store with configured type and size limits. Existing product definitions already cover technical item details, accessories and declared alternatives; this change addresses only the missing image-upload constraint.

## Intended Product Outcome

Equipment image bytes are stored through the existing document-store capability only after their file type and size pass the configured limits. An invalid upload is rejected with no stored file and no change to the item's current image. The existing first non-confidential image default rule remains unchanged.

## Rationale

Image uploads accept untrusted binary input and can consume storage or expose unsupported content if the product does not enforce its stated limits. The limits and rejection behavior need an explicit product contract before implementation.

## Affected Product Areas

- `UC-EQP-ADD-ITEM-IMAGE`: upload validation and refusal behavior.
- `CON-EQP-IMAGE-UPLOAD-LIMITS`: configured image format and size constraints.
- `FR-EQP-020`: verifiable upload acceptance and refusal behavior.
- `TERM-EQUIPMENT-IMAGE` and `BR-EQP-DEFAULT-IMAGE` were inspected; their definitions and default-image behavior remain unchanged.
- The durable storage path remains governed by ADR-0009. Proposed ADR-0031 records where Equipment-specific validation runs relative to the generic document-store port.

## Open Questions

- Which file formats and media types are accepted for an Equipment image?
- What maximum byte size applies per file, and is there a total storage or image-count limit per item?
- Are the allowed types and sizes configured per deployment, or may an Equipment manager configure them in the product?
- Do the same checks apply to images obtained from the existing web-image-search path and ordinary item-file attachments, or only to direct image uploads?
- On refusal, should the interface identify the failed type/size check while preserving the current image and all existing item data?

## Product Acceptance

- Upload limits specify accepted formats and maximum size in bytes, and identify who configures them.
- Invalid or oversized image files are refused before durable storage and do not replace a current image or affect other item data.
- Accepted images continue to be stored through the document-store capability and retain the existing confidentiality/default-image behavior.
- The product definition distinguishes direct uploads from any separately governed image-search or attachment path.
- All open questions have an Owner answer before approval.

## Out of Scope

- Product Change approval or apply, application code, API contracts, UI, document-store adapters, deployment configuration and E2E implementation.
- Changes to technical detail fields, accessory behavior, declared alternatives or the Warehouse shortage-resolution workflow; these are already described by `UC-EQP-CONFIGURE-ITEM-DETAILS`, `UC-EQP-CONFIGURE-ACCESSORIES` and `UC-EQP-SET-ALTERNATIVES`.
- Displaying item images in generated documents, bulk image import, image transformations or external image-search provider decisions.
