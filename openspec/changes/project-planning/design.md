<!-- pdac-scope: cited -->
<!-- pdac:cite id="UC-PRJ-CHANGE-PROJECT-STATUS" digest="sha256:b653f51a2901fb9689b6b7f0357150880ee7658e322411274696709b93d1ce80" -->

# Design

Project owns a fixed lifecycle status persisted as a string. The API returns it in list/detail responses and exposes an explicit status transition operation; warehouse and invoicing effects remain owned by their respective slices.
