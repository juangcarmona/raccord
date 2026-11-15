<!-- pdac-scope: none reason="Owner-requested repository delivery automation and queue bookkeeping only; no application product semantics change." -->

# Delivery bootstrap design

Use the installed SDLC 0.24.2 workflow generator in package mode with gh-aw 0.93.2. Retain generated sources and compiled locks together. Configure adopter-owned checks through `.lighthouse/implement.json` and declared CI identities through `.lighthouse/github.json`; do not invent a scheduler or weaken the existing native contracts.

Keep an ordered issue inventory with original body digests and the owner authorization URL. The inventory records operator intent; authenticated Lighthouse results and fresh GitHub state still decide eligibility. `future` holds prevent broad fanout while #699 blocks all source evidence preparation.

The optional epic controller remains disabled until its protected evidence/profile and native execution prerequisites are qualified. Product approval is recorded once for the authored intent; provider validation/application and actual review-key/head bindings remain explicit. This change delivers the bootstrap PR and a blocked queue, not proof that every epic can already advance unattended.
