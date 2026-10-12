## Purpose

Each implemented bounded context is a module assembly whose internals other modules cannot reach. A boundary violation fails the build or the architecture tests, not only review.

## ADDED Requirements

### Requirement: Modules are assemblies named after bounded contexts

Every module SHALL be an assembly `src/modules/<Module>/Raccord.Modules.<Module>` whose name matches a bounded context in `docs/product/model/domain/bounded-contexts/`. A module SHALL have a `Raccord.Modules.<Module>.Contracts` assembly only when another module references it.

#### Scenario: Initial module set

- **WHEN** the solution `Raccord.slnx` is built
- **THEN** it contains exactly the modules Workspace, Contacts, Projects, Invoicing and Compliance, and no `Raccord.Domain`, `Raccord.Application`, `Raccord.Contracts` or `Raccord.Infrastructure` project

### Requirement: Cross-module references go through contracts

A module SHALL reference another module only through that module's `.Contracts` assembly. A `ProjectReference` from a module to another module's implementation, or to a host, SHALL fail the build with a diagnostic naming both projects.

#### Scenario: Forbidden project reference

- **WHEN** `Raccord.Modules.Invoicing.csproj` gains a `ProjectReference` to `Raccord.Modules.Projects.csproj`
- **THEN** `dotnet build Raccord.slnx` fails with the reference-guard error naming both projects

#### Scenario: Contracts reference is allowed

- **WHEN** `Raccord.Modules.Invoicing.csproj` references `Raccord.Modules.Projects.Contracts.csproj`
- **THEN** the build succeeds

### Requirement: Module internals are not public

Inside a module assembly, commands, queries, persistence entities and handlers SHALL be `internal`. Handlers SHALL be `internal sealed`. Public members SHALL NOT expose domain or persistence types. MVC controllers SHALL NOT exist. Each violation SHALL be a Roslyn analyzer error from `src/shared/Raccord.ArchitectureAnalyzers`, with each rule covered by a test in that analyzer's test project.

#### Scenario: Public handler

- **WHEN** a handler in `Raccord.Modules.Contacts` is declared `public sealed class`
- **THEN** the build fails with the handler-visibility analyzer error at that declaration

#### Scenario: Analyzer rules are tested

- **WHEN** `dotnet test Raccord.slnx` runs
- **THEN** the analyzer test project runs, for every rule, at least one case that must report and one that must not

### Requirement: Domain code stays framework-free

Types in a module's `Domain` namespace and in `Raccord.SharedKernel` SHALL NOT depend on EF Core, ASP.NET Core, Npgsql, Azure SDKs, ErrorOr or FluentValidation. `Raccord.SharedKernel` SHALL reference no other Raccord assembly, and `Raccord.BuildingBlocks` SHALL reference only `Raccord.SharedKernel`.

#### Scenario: Framework type in domain

- **WHEN** a type in `Raccord.Modules.Invoicing.Domain` references `Microsoft.EntityFrameworkCore`
- **THEN** the architecture tests in `tests/Raccord.Architecture.Tests` fail, naming the type

### Requirement: Contract dependencies are acyclic

The graph of `.Contracts` references between modules SHALL be acyclic.

#### Scenario: Cycle introduced

- **WHEN** Projects references `Invoicing.Contracts` while Invoicing references `Projects.Contracts`
- **THEN** the architecture tests fail and report the cycle

### Requirement: The host registers modules explicitly

`src/root/Raccord.Web/Program.cs` SHALL register every module with an explicit `Add<Module>Module()` call and map its endpoints with `Map<Module>Endpoints()`. Modules SHALL NOT be discovered by assembly scanning.

#### Scenario: Module missing from host

- **WHEN** a module's `Add<Module>Module()` call is removed from `Program.cs`
- **THEN** the web tests that cover that module's endpoints fail
