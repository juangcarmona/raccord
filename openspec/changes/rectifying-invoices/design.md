<!-- pdac-scope: cited -->
<!-- pdac:cite id="BR-ISSUED-INVOICE-IMMUTABLE" digest="sha256:309c88f0d7855c684b8ba796c0896ead9f6e821a19edfb731369a176a75ff8b3" -->
<!-- pdac:cite id="BR-GAPLESS-NUMBERING" digest="sha256:1d6404217e241d191188f8a20bdaedda2204e10548c344df7d46793d1c204adb" -->

# Design

`InvoiceDraft.CreateRectification` snapshots the issued invoice's lines into a new draft. Substitution preserves the values; differences create signed adjustment lines. The draft stores the original ID, method and reason, and finalization copies those values into the immutable issued representation.

The endpoint `POST /api/invoices/{id}/rectifications` creates the draft. Existing finalization persists the resulting invoice, document, audit event and compliance record in the same unit of work.

The source invoice is loaded through the append-only repository and is never attached for update. Number allocation remains delegated to the existing per-series sequence, which keeps series R gapless.
