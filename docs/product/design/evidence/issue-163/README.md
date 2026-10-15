# Issue 163 evidence

The `CalendarFeedJourney_Should_PublishPrivateIcs` journey issues a private `all-projects` subscription, reads its one-way iCalendar response, verifies the seeded project event, and captures a screenshot at the feed URL.

Hosted CI uploads the screenshot and Playwright trace as the `playwright-artifacts` artifact for the pull request.
