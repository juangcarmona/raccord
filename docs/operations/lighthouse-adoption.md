# Lighthouse adoption in Raccord

The pinned root tooling package runs Lighthouse verification for the existing .NET backend and React TypeScript frontend. `dotnet-backend`, `react-web-platform`, and `visual-theme` are selected; the React profile requires the visual profile. Raccord's current authentication uses local ASP.NET Core Identity and a same-origin session, so the external-provider-oriented `web-bff` profile is not selected.

The .NET profile describes LH1001 and LH1002, but initialization does not install those analyzers. Raccord keeps its RACC001–004 analyzer, RACC100 MSBuild rule, architecture tests, and Semgrep rules as the executable module-boundary contract. LH1002 overlaps the existing sibling-module checks; LH1001 targets Lighthouse CQRS interfaces that Raccord does not use. Do not add either analyzer without a separate reviewed change.

Lighthouse manages `.husky/commit-msg` and `.husky/pre-push`. Raccord's `.husky/pre-commit` remains project-owned. The CI lint job still runs Raccord's configured commitlint policy over PR commits, while the generated commit hook checks Lighthouse's conventional-commit baseline.

The default SDLC triage and refine workflows are installed from the pinned package and compiled with gh-aw v0.93.2. The router requires the repository variable `LIGHTHOUSE_AUTOMATION_ENABLED=true`; installing files does not set it. Runner provisioning, label review, and an explicit automation decision are separate steps.

The copied SDLC workflow sources retain the package's [license](../licenses/lighthouse-sdlc/LICENSE) and [notice](../licenses/lighthouse-sdlc/NOTICE).
