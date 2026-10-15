# Issue 152 evidence: equipment lifecycle

The Playwright journey creates rental equipment, sells one unit into a sale item, archives the remaining rental stock, verifies the resulting stock movement history, and captures the lifecycle state.

Product references:

- [UC-EQP-SELL-RENTAL-EQUIPMENT](../../../model/use-cases/uc-eqp-sell-rental-equipment.md)
- [UC-EQP-ARCHIVE-ITEM](../../../model/use-cases/uc-eqp-archive-item.md)
- [UC-EQP-VIEW-ITEM-INSIGHTS](../../../model/use-cases/uc-eqp-view-item-insights.md)
- [UC-EQP-VIEW-EQUIPMENT-PROJECT-HISTORY](../../../model/use-cases/uc-eqp-view-equipment-project-history.md)

The lifecycle API and persistence were already present on the current feature tip; this increment closes the missing end-to-end evidence gap for sale, retirement and trace history.

The hosted CI artifact contains the screenshot and trace from the successful run.
