<!-- pdac-scope: cited -->

## Decisions

- The setup status is anonymous and reports whether the users table is empty.
- The setup command is anonymous but succeeds only while no user exists; a completed installation returns conflict.
- Role creation, user creation and Admin assignment share the Identity DbContext transaction.
- The setup page creates the account and then sends the administrator to sign-in; it does not create a hidden session.

<!-- pdac:cite id="UC-WSP-CREATE-WORKSPACE" digest="sha256:e5ee7da50f5c081b1c257461484a9e6eecfbce0f108a5abd2d388c14c8e4f16a" -->
