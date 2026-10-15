<!-- pdac-scope: cited -->

# Role permissions and role management

Implement the first usable authorization model for an installation. Roles carry explicit `module:action` permissions, the four product-defined roles are seeded, and administrators can create, edit, inspect, and assign roles.

Product sources:

<!-- pdac:cite id="UC-WSP-MANAGE-ROLE" digest="sha256:afdfe2af3c8762da5cbbecba231a9156769b0715ae10f67949a2bb4fd6ccb405" -->
<!-- pdac:cite id="UC-WSP-ASSIGN-ROLE" digest="sha256:7225266488941b418540e36431ed75cb596efdb9cddef5109aa52f65a9db9b37" -->
<!-- pdac:cite id="UC-WSP-REVIEW-ROLE-ASSIGNMENTS" digest="sha256:0d9d3f9d3d989cc2cee6ccd363bbf954117df26c16c75b8015d4d326aea5f0ed" -->
<!-- pdac:cite id="TERM-PREDEFINED-ROLE" digest="sha256:40c8223391cb84a39ccfa1047f4cec3e38063e401caf61b52a31a2d14d69e7c5" -->
<!-- pdac:cite id="TERM-USER-ROLE" digest="sha256:c190cd829253e0e4b2cc3965b8e7d3bd7e6048edec05cd5cf9ecec10faabdb4b" -->
<!-- pdac:cite id="TERM-ROLE-PERMISSION" digest="sha256:c4b1b094b93090f465182781150585161f1659ffef73093f03ae1ae2c610eceb" -->

The MVP deliberately limits declarations to the current endpoint surface. Future modules can add permissions without changing the authorization mechanism.
