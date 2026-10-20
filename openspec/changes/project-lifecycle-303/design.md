# Design

<!-- pdac-scope: cited -->

Persist the lifecycle and project detail state with the Projects bounded context and expose it through the existing project API and screens. Keep issued invoice snapshots immutable; project cancellation must never rewrite invoice records or billed lines. Keep equipment reservation release behind the existing Warehouse boundary and cover its integrated result in #384.

Project schedules are persisted as project-owned periods. Date propagation must update only records linked to the changed period, using existing module interfaces rather than introducing cross-module table access. Manual project status is distinct from equipment status. Cancellation action discovery is read-only until explicit confirmation; the confirmed mutation and audit record are saved together.

The browser evidence covers project edit, schedule validation, status change, cancellation confirmation, visible warning, and invoice history after cancellation. Additional Warehouse release evidence remains owned by #384.

<!-- pdac:cite id="UC-PRJ-EDIT-PROJECT-DETAILS" digest="sha256:cc87eb90988fdbd32fb2e626a02aa7154783ffc7b161702a3c2bf4ace9c2bdb7" -->
<!-- pdac:cite id="UC-PRJ-MANAGE-TIME-SCHEDULE" digest="sha256:63f5e75776fe129ece805663868a322547da608fd04023944189c071b9c04bae" -->
<!-- pdac:cite id="UC-PRJ-CHANGE-PROJECT-STATUS" digest="sha256:1a5c43dfd663a08a833301fa25ccad7dc02057bfbc8c444f4fb18b3347905acc" -->
<!-- pdac:cite id="UC-PRJ-CANCEL-PROJECT" digest="sha256:80e15e9d11dba906cb7d753fd78b7ce8b29c30651bfe9cdb9ac1d7cdc3118d96" -->
<!-- pdac:cite id="BR-PRJ-CANCEL-PRESERVES-INVOICED-WORK" digest="sha256:383aadaf0babcb79c58e36c9c9559532ceb4589ad3b8bc92f409874c012ae258" -->
