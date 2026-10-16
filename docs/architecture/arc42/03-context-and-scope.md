---
title: Context and scope
arc42-section: "03"
description: Describes Raccord's external actors, systems, interfaces, and architectural boundary.
---
<!-- markdownlint-disable MD025 -->

# 3. Context and scope

## Business context

```mermaid
flowchart LR
    accTitle: Business context and external boundaries
    user["Authenticated user<br/>browser application"] -->|same-origin UI and JSON API| raccord["Raccord<br/>self-hosted operations and invoicing"]
    api["Scoped API caller"] -->|HTTP API or calendar feed| raccord
    raccord -->|download| files["PDF and structured exports"]
    future["Tax network submission<br/>future seam, not connected"]
    classDef current fill:#e6f4ea,stroke:#1b5e36,color:#163b25,stroke-width:1.5px
    classDef boundary fill:#eaf2f8,stroke:#245679,color:#123045,stroke-width:1.5px
    classDef future fill:#fff4d6,stroke:#805500,color:#3c2900,stroke-width:1.5px,stroke-dasharray:5 4
    class raccord current
    class user,api,files boundary
    class future future
```

The browser UI and protected JSON API are served from the same web host. Scoped API callers use explicit integration credentials; the application also provides a personal calendar feed. Files are produced for download or received through the API. The tax-submission seam remains local and has no network sender (see [compliance seam](../compliance-seam.md)).

## Technical context

```mermaid
flowchart LR
    accTitle: Runtime context and persistence boundaries
    browser["Browser<br/>React application"] --> web["Raccord.Web<br/>API and static UI"]
    web -->|EF Core| database[("PostgreSQL")]
    web -->|document storage adapter| documents[("Configured document storage")]
    migrate["One-shot migration command"] -->|schema updates| database
    web -. optional telemetry .-> collector["OTLP collector"]
    classDef current fill:#e6f4ea,stroke:#1b5e36,color:#163b25,stroke-width:1.5px
    classDef boundary fill:#eaf2f8,stroke:#245679,color:#123045,stroke-width:1.5px
    classDef optional fill:#fff4d6,stroke:#805500,color:#3c2900,stroke-width:1.5px,stroke-dasharray:5 4
    class web,database,documents current
    class browser,migrate boundary
    class collector optional
```

## Scope

Raccord currently provides authenticated project, crew and contact workflows; invoicing and related documents; scoped integration endpoints; audit and compliance records; and self-hosted or Azure deployment paths. The diagrams show the implemented system boundary and mark external tax submission as a future seam rather than a running integration.

No outbound tax-network or B2B-platform exchange is implemented. Email delivery is not part of the current boundary.
