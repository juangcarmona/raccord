<!-- pdac-scope: cited -->
<!-- pdac:cite id="UC-WSP-SET-TIME-ZONE" digest="sha256:27ace6bc0430fe7e848b71443457ab2839c9af7b89d56dd638d0170a5c1c74c7" -->

# Design

Workspace preferences are a single-row Workspace persistence record with `en` and `UTC` defaults. The organization endpoints use the role permission catalog: reads require organization read, writes require organization edit. Existing issuer and invoice-series records are retained, so changing preferences cannot rewrite issued invoice numbers.
