import { describe, expect, it } from "vitest";
import path from "node:path";
import { findBoundaryViolations } from "./check-frontend-boundaries.mjs";

const file = (name, content) => ({ path: path.resolve("src", name), content });

describe("frontend dependency boundaries", () => {
    it.each([
        ["features/orders/OrderPage.tsx", 'import { routes } from "../../app/routes";'],
        ["features/orders/OrderPage.tsx", 'import { CustomersPage } from "../customers/CustomersPage";'],
        ["shared/ui/Button.tsx", 'import { OrderPage } from "../../features/orders/OrderPage";'],
    ])("rejects %s", (source, content) => {
        expect(findBoundaryViolations([file(source, content)])).toHaveLength(1);
    });

    it("allows app composition, feature-to-shared imports and shared-local imports", () => {
        expect(
            findBoundaryViolations([
                file("app/routes.tsx", 'import { OrderPage } from "../features/orders/OrderPage";'),
                file("features/orders/OrderPage.tsx", 'import { Button } from "../../shared/ui/Button";'),
                file("shared/ui/Button.tsx", 'import { useI18n } from "../i18n/I18nProvider";'),
            ]),
        ).toEqual([]);
    });
});
