<!-- pdac-scope: cited -->
<!-- pdac:cite id="TERM-ROLE-PERMISSION" digest="sha256:c4b1b094b93090f465182781150585161f1659ffef73093f03ae1ae2c610eceb" -->

# Design

Identity role claims store one permission per `raccord:permission` claim. A shared catalog defines the current endpoint permissions and the predefined role profiles. Endpoint groups attach a requirement to each operation.

The authorization handler resolves the current user roles and role claims from the database for every request. This makes assignment changes effective immediately, while updating the security stamp also invalidates stale application cookies through the normal Identity validation path.

The role API supports listing roles, creating and editing custom or predefined role permissions, and assigning exactly one role to a user. The SPA exposes a role list and custom-role editor. Existing module endpoints remain thin adapters; authorization is composed at the endpoint boundary.
