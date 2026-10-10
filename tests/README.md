# Tests

| Project | Seam | Needs Docker |
|---|---|---|
| `Raccord.Domain.Tests` | Domain model (money, VAT, numbering, drafts, finalization, representation, parties) | No |
| `Raccord.Architecture.Tests` | Dependency rules | No |
| `Raccord.Infrastructure.Tests` | PDF renderer, file document store | No |
| `Raccord.Web.Tests` | HTTP API against real PostgreSQL (Testcontainers, one database per test) | Yes |

Frontend tests live next to the code in `src/Raccord.Frontend` (`npm test`).

## TDD rule

For every vertical slice:

1. Name the public seam and the user-visible behavior.
2. Write one failing test against that seam and check it fails for the right reason.
3. Implement the smallest behavior that makes it pass.
4. Repeat for the next behavior.
5. Refactor only after the slice is green.

Do not write speculative tests for imagined APIs. Money, taxes, numbering, rounding, finalization and audit never change without a test. Strategy: ADR-0014.
