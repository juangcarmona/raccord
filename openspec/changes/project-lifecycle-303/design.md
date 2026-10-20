# Design

<!-- pdac-scope: cited -->

Persist lifecycle, project details and named periods with the Projects bounded context. The Projects API owns validation and mutation of project records; the web detail screen calls that API and never writes across module tables. Add `IProjectScheduleStore` and `IProjectSchedulePropagation` in `Raccord.Modules.Projects.Contracts`, with Infrastructure adapters for period persistence and linked-record propagation. Contacts are resolved through `Raccord.Modules.Contacts.Contracts`; project-specific commercial overrides remain in Projects because this repository has no separate Commercial module contract. Extend the current `CreateProjectFunctionRequest` and `CrewPlanningCatalog` boundary with period links and one atomic batch operation for generated functions. Keep `DocumentGenerationCatalog.GenerateProjectAsync` as the project-document boundary and apply each period's internal/external display flags there. Keep issued invoice snapshots immutable; cancellation never rewrites invoice records or billed lines.

Project schedules are persisted as project-owned periods. Date changes propagate only through explicit period links in equipment and function records; unlinked records retain their dates. `IProjectSchedulePropagation` coordinates these updates under one database transaction. A propagation failure aborts the operation and leaves the period and linked records consistent. Selected-period function generation calls the `CrewPlanningCatalog` batch operation, which creates one Crew and one Transport function per selected period and stores each period link in the same transaction. Project status is distinct from equipment status. Complete blocks operational edits and new planning until an explicit reopen. Cancellation action discovery is read-only until confirmation; the selected actions, cancellation state and audit record are committed together, while unselected applicable actions remain unchanged. Equipment reservation release stays behind the Warehouse boundary and its integrated result is covered in #384.

The API, domain and browser evidence covers project detail validation, period ordering and date propagation, Complete/reopen behavior, status-dependent area and document visibility, schedule validation, status changes, selected cancellation actions, visible warnings, and invoice history after cancellation. Browser selection must include synthetic data and `Area=projects` in each PR's E2E manifest. Additional Warehouse release evidence remains owned by #384.

<!-- pdac:cite id="UC-PRJ-EDIT-PROJECT-DETAILS" digest="sha256:cc87eb90988fdbd32fb2e626a02aa7154783ffc7b161702a3c2bf4ace9c2bdb7" -->
<!-- pdac:cite id="UC-PRJ-MANAGE-TIME-SCHEDULE" digest="sha256:63f5e75776fe129ece805663868a322547da608fd04023944189c071b9c04bae" -->
<!-- pdac:cite id="UC-PRJ-CHANGE-PROJECT-STATUS" digest="sha256:1a5c43dfd663a08a833301fa25ccad7dc02057bfbc8c444f4fb18b3347905acc" -->
<!-- pdac:cite id="UC-PRJ-CANCEL-PROJECT" digest="sha256:80e15e9d11dba906cb7d753fd78b7ce8b29c30651bfe9cdb9ac1d7cdc3118d96" -->
<!-- pdac:cite id="BR-PRJ-CANCEL-PRESERVES-INVOICED-WORK" digest="sha256:383aadaf0babcb79c58e36c9c9559532ceb4589ad3b8bc92f409874c012ae258" -->
<!-- pdac:cite id="BR-PRJ-DEFAULT-PERIOD-DESIGNATION" digest="sha256:d58ad77fac1f84439e20b7fe8103c4a4918ce1e8a31485a20a0e3e93ccb1d06d" -->
<!-- pdac:cite id="BR-PRJ-PERIOD-DOCUMENT-DISPLAY" digest="sha256:aec83cc960f978d7a6e59f2105e5c15f9e75d0c855f7a5d44fa543756c54c907" -->
<!-- pdac:cite id="BR-PRJ-FUNCTIONS-FROM-PERIODS" digest="sha256:335f48cbaf338557f9f01e6f1b49e6278b02d4a9d5c73328729650d909e330ac" -->
<!-- pdac:cite id="TERM-PROJECT-STATUS" digest="sha256:ea57a30386b9806577b0ed5376630ed6b9048a5afc882cc20320958ed8382eef" -->
