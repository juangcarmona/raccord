<!-- pdac-scope: cited -->
<!-- pdac:cite id="UC-PRJ-RECEIVE-PROJECT-REQUEST" digest="sha256:dd19f461f00bac47b39603582fa87cdd8f3ad836ca659c0fce729ca27b1e7050" -->
<!-- pdac:cite id="UC-PRJ-CONVERT-PROJECT-REQUEST" digest="sha256:7efe604979d08b83572e4bf7e9441cef0e69f20f7e51645a99419d611c473a50" -->

# Design

Project requests, project types and teams are durable project-context records. The coordination service creates a request, validates an optional existing customer, creates a project from the request fields and marks the original request converted in one unit of work. Decline is an explicit terminal state requiring a reason. Existing project creation remains compatible while accepting optional type and team identifiers.
