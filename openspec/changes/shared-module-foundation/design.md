<!-- pdac-scope: cited -->

## Context

Work item: [#30](https://github.com/juangcarmona/raccord/issues/30). See [proposal.md](proposal.md) for purpose and scope. Implement this change only, never the entire `module-architecture/tasks.md`.

Observed baseline: `Raccord.Domain/Common`, `Parties` and `Pricing` contain shared pure types. `Application/Abstractions` owns clock/actor/UoW ports; `IAuditTrail` mixes append and read and accepts `Domain.Auditing.AuditEvent`. `Infrastructure/Persistence/UnitOfWork.cs` saves one context. Storage adapters are internal infrastructure classes. `ApiExceptionHandler.cs` treats `draft.not-open` as 409, invoice readiness as `draft.invalid` with a list of `{code,message}`, and integrity failure as 500 with critical logging.

## Goals / Non-Goals

Provide shared assemblies usable by subsequent module slices while existing layered consumers remain runnable. Every task has a small input surface and a concrete completion check. Application use cases continue to call the compatibility `SaveChangesAsync` seam.

No module/host move, schema/migration, production action, endpoint-to-slice conversion, permission implementation, event bus, outbox or pricing rewrite is included. SharedKernel pricing is the parent plan's explicit default. Invoice JSON/hash representation and PDF bytes are compatibility boundaries.

<!-- pdac:cite id="JRN-BILL-FINISHED-PROJECT" digest="sha256:b9530a2b0a9aac0f9cb3a5a9608494d08be1fc1190f395887fc7bb7fb30df28e" -->
<!-- pdac:cite id="UC-CREATE-INVOICE-DRAFT" digest="sha256:f7310fed1fddd6fc7c813db19db677f8aea9feff2f179549c9a9425607804d33" -->
<!-- pdac:cite id="UC-ISSUE-INVOICE" digest="sha256:08b2bb0bd93fc7fbb0b745ee1e6e7f3f70d064d44da287ed8c478a8da7761afc" -->
<!-- pdac:cite id="BR-MONEY-ROUNDING" digest="sha256:51ad0cc84544ce872e3a93e25fb8110b701a85ec9efe737bc1ec15b8b985b87f" -->
<!-- pdac:cite id="BR-LINE-PRICING-BOUNDS" digest="sha256:652792a573f685a43f131a08b467a4de67c176c48e81ea5ba38aa1367e7c599c" -->
<!-- pdac:cite id="BR-TAX-ID-NORMALIZATION" digest="sha256:aadb655c57c6486610d3a3346f45bdd19696a407dd954d4fc37cd9cefd3391ca" -->
<!-- pdac:cite id="BR-PARTY-SNAPSHOT" digest="sha256:24dccb3800fada7dd1ae45a9844795011eaf44233be34465b3f9cf6a2a4e95d4" -->
<!-- pdac:cite id="BR-GAPLESS-NUMBERING" digest="sha256:1d6404217e241d191188f8a20bdaedda2204e10548c344df7d46793d1c204adb" -->
<!-- pdac:cite id="BR-ISSUED-INVOICE-IMMUTABLE" digest="sha256:309c88f0d7855c684b8ba796c0896ead9f6e821a19edfb731369a176a75ff8b3" -->
<!-- pdac:cite id="BR-AUDITED-ACTIONS" digest="sha256:a29bab7607c529ef8b86f34e32758cf8557927cf7e71812bded9aac01c3ef469" -->
<!-- pdac:cite id="BR-COMPLIANCE-OBLIGATION" digest="sha256:dab51b5c3c98ca25c40abec4a8e477fdc172ac986788fd2233eb2560f14a4757" -->
<!-- pdac:cite id="BR-INTEGRITY-VERIFICATION" digest="sha256:19dff5afbeb5e4cd25a3baaa4f1395ee74b0c0722572021e496e654cb44aaf34" -->
<!-- pdac:cite id="FR-AUDIT-001" digest="sha256:7646b5398f69286e802b2a3175edcf8b18cfc3b742edc9d3c2a44476c6159417" -->
<!-- pdac:cite id="FR-COMPLIANCE-001" digest="sha256:a53549df92d28a28435890b7663e0a9c92ec126ff0988b6cb103c39d5aa050d8" -->
<!-- pdac:cite id="QR-DETERMINISM" digest="sha256:2186a4638c7ad311864a05314e15638df09365c839eaa932ceb57eddcb1559e9" -->
<!-- pdac:cite id="QR-CONCURRENCY-CONSISTENCY" digest="sha256:320945ebf06be10006380b4e183377f48569c75224692b903f3cf093887736a2" -->

## Decisions

### 1. File moves and exact dependency edges

Move, do not copy, the files in these three directories. Preserve implementations, public members, exception messages and calculations; update namespaces/usings and project references mechanically.

| Source | Destination | Namespace |
| --- | --- | --- |
| `src/Raccord.Domain/Common/*.cs` | `src/shared/Raccord.SharedKernel/Common/` | `Raccord.SharedKernel.Common` |
| `src/Raccord.Domain/Parties/*.cs` | `src/shared/Raccord.SharedKernel/Parties/` | `Raccord.SharedKernel.Parties` |
| `src/Raccord.Domain/Pricing/*.cs` | `src/shared/Raccord.SharedKernel/Pricing/` | `Raccord.SharedKernel.Pricing` |
| `Application/Abstractions/IClock.cs` | `SharedKernel/Abstractions/IClock.cs` | `Raccord.SharedKernel.Abstractions` |
| `Application/Abstractions/ICurrentActor.cs` | `SharedKernel/Abstractions/ICurrentUser.cs` | `Raccord.SharedKernel.Abstractions` |
| `Application/Documents/IDocumentStore.cs` | `BuildingBlocks/Storage/IDocumentStore.cs` | `Raccord.BuildingBlocks.Storage` |
| Both `Infrastructure/Documents/*DocumentStore.cs` adapters | `BuildingBlocks/Storage/` | `Raccord.BuildingBlocks.Storage` |
| `Infrastructure/StorageOptions.cs` | `BuildingBlocks/Storage/StorageOptions.cs` | `Raccord.BuildingBlocks.Storage` |

SharedKernel has zero project references and no runtime packages. BuildingBlocks references only SharedKernel and uses `Microsoft.AspNetCore.App` as a framework reference plus centrally managed ErrorOr 2.1.1, FluentValidation 12.1.1, Npgsql.EntityFrameworkCore.PostgreSQL 10.0.3 and Azure.Storage.Blobs at the version already resolved by Aspire.Azure.Storage.Blobs. Pin that existing Azure version centrally before its direct reference; do not upgrade the existing dependency graph. EF/Npgsql/DI APIs arrive through the existing pinned packages/framework.

Interim direct references: Domain adds SharedKernel; Application adds SharedKernel and BuildingBlocks for the moved document-storage port; Infrastructure adds SharedKernel and BuildingBlocks; Web adds SharedKernel and BuildingBlocks. Existing references remain. Contracts remain BCL-only. Application does not reference infrastructure implementations; the temporary direct BuildingBlocks reference is limited to the storage contract and is recorded in the architecture rules until module use cases replace the compatibility seam. Keep the existing Application UoW and concurrency exception as adapters. Add test project references explicitly where moved types are used, including the naming-test project's new test-assembly reference.

Update `DependencyRulesTests.ProjectFile` to locate the two new paths explicitly and fail if a project is missing. Its Domain BCL rule allows SharedKernel as the sole Raccord dependency; SharedKernel itself is independently BCL-only. Retain forbidden frameworks in Domain/Application and exact-reference checks. BuildingBlocks' Raccord references must equal SharedKernel only. This narrowly deviates from the old Domain reference list in `architecture-rules.md`, justified by accepted ADR-0018; document the interim graph without claiming all module boundaries already exist.

Alternatives rejected: duplicate types risk conflicting identities; type-forwarder compatibility packages increase work with no published external package consumer; adding BuildingBlocks to Application would pull framework infrastructure into the current core.

### 2. Clock, identity and audit append

`ICurrentUser` keeps `string Name { get; }`; rename consumers of `ICurrentActor` to that port. Keep `HttpCurrentActor` behavior exactly (authenticated name when present, otherwise `unauthenticated`) and keep `SystemClock` in Infrastructure, implementing the moved IClock.

Define `SharedKernel/Auditing/AuditEntry.cs` as a BCL-only immutable record with `OccurredAt`, `Actor`, `Action`, `EntityType`, `EntityId`, and `IReadOnlyDictionary<string,string> Details`. Define `IAuditAppender.Append(AuditEntry entry)` as a synchronous append-to-pending-work port. Copy Details when persisting.

Keep `Domain.Auditing.AuditEvent` in its current project. Have the existing `IAuditTrail` inherit the shared append port while retaining `Append(AuditEvent)` and `ListForEntityAsync`. AuditTrail implements the new overload by calling the existing `AuditEvent.Record` once and adding it to the same RaccordDbContext. Register `IAuditAppender` as a factory resolving the same scoped IAuditTrail instance. Existing use cases need no audit-flow rewrite. The Compliance slice later replaces this adapter.

Reject accepting AuditEvent in SharedKernel: that creates a forbidden dependency back to Domain. Reject moving audit persistence into this issue: Compliance owns that later work.

### 3. Explicit sender with bounded exception adapters

New files under `BuildingBlocks/Messaging/`: `IRequest<TResponse>`, `IRequestHandler<TRequest,TResponse>`, `ISender`, `Sender`, `ExpectedFailure`, `IExpectedExceptionMapper`, `SenderRegistration`. Use `Task<ErrorOr<TResponse>>` for both handler and sender; request is a marker, handler takes request plus CancellationToken. `SendAsync<TRequest,TResponse>` uses generic constraints tying the handler to the request and response.

Use these public signatures; implementations remain internal sealed. Keep the namespace folders listed above and XML documentation on public members to satisfy warnings-as-errors.

```csharp
public interface IRequest<TResponse> { }

public interface IRequestHandler<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    Task<ErrorOr<TResponse>> HandleAsync(TRequest request, CancellationToken cancellationToken);
}

public interface ISender
{
    Task<ErrorOr<TResponse>> SendAsync<TRequest, TResponse>(
        TRequest request, CancellationToken cancellationToken)
        where TRequest : IRequest<TResponse>;
}

public interface IExpectedExceptionMapper
{
    ExpectedFailure? TryMap(Exception exception);
}
```

ExpectedFailure is a sealed record containing `Error Error` and optional `IReadOnlyList<ProblemIssue>? Issues`. ProblemIssue lives in BuildingBlocks.Api. Register the built-in domain mapper once, followed by optional application mappers; first non-null mapping wins. Reserve metadata keys `raccord.issues` and `raccord.title` for the mapper's issue list and preserved problem title; ApiResults reads these keys rather than serializing the metadata dictionary. Null request/empty error list arguments fail with ArgumentNullException/ArgumentException before dispatch/response writing.

Register each request/handler/optional validator explicitly through a generic DI helper; no reflection, dynamic invocation or assembly scanning. Sender resolves the typed handler from the scoped provider and runs registered validators in registration order using `ValidateAsync`. Supply the same token to every step. Stop before the handler on validation failure. Convert validator failures to Error.Validation with stable configured ErrorCode and ErrorMessage; carry field names as internal metadata only, not a new HTTP field.

A narrow `IExpectedExceptionMapper.TryMap(Exception)` returns nullable `ExpectedFailure`, which holds the top-level Error and optional list of `ProblemIssue(Code,Message)`. BuildingBlocks contains a mapper for SharedKernel.BusinessRuleViolationException. Because concurrency and invoice-readiness exceptions currently belong to Application, add `Infrastructure/Messaging/ApplicationExceptionMapper.cs` implementing the shared mapper; this avoids BuildingBlocks → Application/Domain references. It recognizes only current `ConcurrencyConflictException` and `InvoiceValidationException`. Register it explicitly with the sender. Sender returns the mapped Error and attaches the optional issues as metadata for ApiResults. Keep readiness status/code/message consistent with the existing exception handler. Cancellation and unrecognized exceptions propagate.

The domain mapper must special-case `draft.not-open` to Error.Conflict and decline `issued-invoice.integrity-violation`; the existing global exception handler retains its critical log and 500 response. Generic rules use a documented custom rule discriminator (1000) and the original rule code. Log only safe request type/outcome information, never request payloads or validation values. Sender does not open a database transaction automatically; handlers choose the UoW seam.

Reject relocating invoice-specific exceptions into the shared kernel: their existing ownership need not change. Reject domain dependencies on ErrorOr/FluentValidation, as required by ADR-0022.

### 4. HTTP failures with explicit types

Add `BuildingBlocks/Api/ApiResults.cs` and BCL-shaped `ProblemIssue(Code,Message)` metadata. Expose `ToProblem(IReadOnlyList<Error>)` returning an ASP.NET IResult; successful response construction stays with each future endpoint, so avoid inventing a universal success/Created mapping.

| Error type | HTTP status |
| --- | --- |
| Validation | 422 |
| Custom rule discriminator 1000 | 422 |
| Conflict | 409 |
| NotFound | 404 |
| Forbidden | 403 |
| Unauthorized | 401 |
| Failure, Unexpected, unknown custom | 500 |

Use the first error's code for `type = urn:raccord:problem:<code>` and `code`. For input validation, add `issues` containing every error as `{code,message}` in stable validator order. For invoice-readiness errors, use the mapper's supplied issues and the existing top-level `draft.invalid` code. Preserve known titles/details from ApiExceptionHandler when bridging those failures. Unknown 500 failures use a generic safe detail, never raw exception content.

Keep existing Endpoints/ApiExceptionHandler active and unchanged in behavior. Demonstrate the mapper via a small test-only endpoint/HttpContext; do not route existing endpoints through the sender in #30. Module slice adoption comes later. This also preserves today's bare NotFound responses, rather than changing all missing-resource HTTP bodies during foundation work.

### 5. Shared transaction and legacy adapter

New `BuildingBlocks/Persistence/` files: `ParticipatingContexts`, `IUnitOfWork`, `SharedConnectionUnitOfWork`, `PersistenceConflictException`. Shared IUnitOfWork exposes `Task SaveChangesAsync(CancellationToken)` and `Task<T> ExecuteAsync<T>(Func<CancellationToken,Task<T>>, CancellationToken)`. ParticipatingContexts exposes `void Register(DbContext context)` and an ordered read-only participant list. Register contexts explicitly in a scoped registry, deduplicated by object identity and ordered by first registration.

Take a scoped NpgsqlConnection and registry in SharedConnectionUnitOfWork. Before opening a transaction verify every registered context uses that same connection object. Before each save/enlist step recheck participants, including contexts registered during the callback. Any other connection or active external transaction is rejected before persistence; silently running separate transactions is forbidden. Nested Execute calls are rejected explicitly.

Execute opens the connection only if needed, starts one transaction, enlists contexts via `Database.UseTransactionAsync`, runs the callback, saves each participating context once, and commits once. A successful callback value is returned only after commit. SaveChanges outside Execute uses the same transaction routine with a no-op callback; inside an active Execute it throws InvalidOperationException before saving. Execute alone owns the single final flush and commit; callbacks must not call SaveChangesAsync.

Always rollback on callback/enlist/save/commit failure or cancellation, using a cleanup token not already cancelled. Detach transactions in a finally block; clear tracked state after rollback and discard the request scope before retrying. Dispose owned transactions and close only a connection this operation opened. Do not dispose a connection owned by the scoped context/container. Map EF concurrency and PostgreSQL 23505 to shared PersistenceConflictException; preserve the original exception as InnerException. Other database failures propagate.

No retry strategy is enabled by this slice. Keep current default isolation and request-level retry behavior. Reject a participating context configured for automatic retries until a whole-callback execution strategy is implemented and tested; do not retry individual SaveChanges or replay object storage side effects.

Keep `Application.Abstractions.IUnitOfWork` and `ConcurrencyConflictException` where they are. Replace Infrastructure's UnitOfWork implementation with a thin adapter: resolve RaccordDbContext; register it; delegate to shared SaveChanges; translate shared PersistenceConflictException to the current ConcurrencyConflictException with the existing concurrency/unique-conflict messages. In Infrastructure registration, derive the shared connection from `RaccordDbContext.Database.GetDbConnection()` and reuse it; keep Aspire's current AddNpgsqlDbContext registration and design-time factory. Production still has one context. Tests use two small test-only contexts configured explicitly with a single external connection; no module schema is required.

Test failures in both registration orders; query committed rows through a fresh connection so change tracking cannot hide partial commits.

### 6. Storage ownership without rendering moves

Keep IDocumentStore signatures and Storage options names/defaults exactly. BuildingBlocks owns internal filesystem and Azure adapters and exposes `AddDocumentStorage(IServiceCollection services, StorageOptions options)` as an extension method. The Azure branch uses a singleton factory to resolve BlobServiceClient from DI; Infrastructure registers that client through Aspire first. Infrastructure retains resource/configuration selection and invokes the helper, registering one singleton store. Include InternalsVisibleTo for BuildingBlocks tests, and test provider selection through the public helper without expanding adapter visibility.

Reuse the existing filesystem path-resolution rules, atomic temporary-file replacement and Azure create-if-needed/overwrite/content-type/404 behavior. Keep PdfInvoiceRenderer, WinAnsi, document key generation and rendering interfaces in their current layers. No new configuration, document format, key prefix or lazy production request is needed to complete #30.

### 7. Test projects and the small-model execution contract

Add `tests/Raccord.BuildingBlocks.Tests` (inherits tests/Directory.Build.props) with references to both shared projects and existing Testcontainers.PostgreSql. Include it in Raccord.slnx and the Architecture test project's naming-test reference list. Keep existing domain test project/name for moved primitive/pricing tests; moving a test project's namespace does not require renaming its assembly.

Read this design, then execute one checkbox at a time in [tasks.md](tasks.md). A task is complete only after its named check passes. First write the named failing test, then implement the minimum. Preserve assertion meanings. Commit in small conventional commits after checkpoints; stop on a changed HTTP contract, EF model drift, dependency cycle, unexpected pricing change or red-line operation, and report concrete evidence. Use [test-plan.md](test-plan.md) as the fixed acceptance mapping.

## Risks / Trade-offs

- Ambient retries or independent connections can destroy atomicity → validate connection identity and transaction ownership; reject unsupported retry configuration.
- SharedKernel type moves change EF CLR metadata → regenerate no migration; pending-model check must report none. Compare configurations and the canonical representation tests before proceeding.
- Existing exceptions live above BuildingBlocks → keep one explicitly registered Infrastructure mapper, rather than reversing dependencies.
- Storage lies outside PostgreSQL transactions → preserve today's sequencing; this slice promises database atomicity, not distributed rollback of blob/file writes. Full finalization/storage behavior stays under the current workflow and later Invoicing migration.
- Sender/result mapping exists before route adoption → exercise the public seams in tests; later module slices migrate routes.
- Small models can mistake an epic plan for a work queue → only this change's tasks are executable for #30; parent tasks 1.3 onward are excluded.
- Old architectural documentation is transitional → publish the exact interim shared dependency graph during implementation and retain framework-free Domain/Application checks.

## Migration Plan

Only code/reference/registration updates are included. Database schemas, EF migrations, resource names and storage layout remain unchanged. Deployment is outside the stage; the implementer supplies a green image/Compose check as evidence. Rollback of an eventual release uses the previous application image against the unchanged database/storage, performed by the Owner.

## Open Questions

None required before implementation. Selected interfaces and temporary adapters above define this slice; future permission, module schema and event decisions belong to their separate issues.
