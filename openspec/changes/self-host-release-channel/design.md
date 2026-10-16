<!-- pdac-scope: cited -->

## Decision

Use Docker Compose as the supported self-host path and publish tagged, versioned application images to GitHub Container Registry. Operators pin the exact image version. The upgrade flow backs up the database and documents, applies migrations with the selected image, and starts the web service only after migration succeeds.

## Recovery and support

A migration or post-upgrade health failure requires restoring the matching database and document backup before retrying. Do not roll back only the application image after schema changes. The latest stable release is the only supported release. Modified and source-built images remain possible but are outside the official release support promise. Aspire remains for development; Azure remains a separately documented deployment.

## Verification

The implementation follow-up must exercise a clean install, a prior-release upgrade, a migration failure, backup and restore, health checks, and preservation of issued invoice and audit records.

<!-- pdac:cite id="BR-ISSUED-INVOICE-IMMUTABLE" digest="sha256:309c88f0d7855c684b8ba796c0896ead9f6e821a19edfb731369a176a75ff8b3" -->
<!-- pdac:cite id="UC-REVIEW-AUDIT-TRAIL" digest="sha256:76998e018b1b1b591ab6278c52a376fb388b6c12665e1c48648332eab6495d9a" -->
