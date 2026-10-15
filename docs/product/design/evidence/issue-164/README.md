# Issue 164 evidence

The payment-reporting journey issues a token with invoices.read and payments.write scopes, reads the public invoice collection through X-Api-Key, and captures a screenshot of the verified integration state.

The CompanyRegistryJourney_Should_HandleUnavailableProvider journey verifies the safe no-match/manual-entry behavior when no registry provider is configured; a real provider is selected through Raccord:CompanyRegistry:BaseUrl.
