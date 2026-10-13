import { render, screen } from "@testing-library/react";
import userEvent from "@testing-library/user-event";
import { createMemoryRouter, RouterProvider } from "react-router";
import { describe, expect, it } from "vitest";
import { I18nProvider } from "../shared/i18n/I18nProvider";
import { AppShell } from "./AppShell";
import { AuthProvider } from "../features/auth/AuthProvider";

describe("AppShell", () => {
    it("exposes the current section, skip link, and named locale selector", async () => {
        const router = createMemoryRouter(
            [
                {
                    element: <AppShell />,
                    children: [{ path: "/projects", element: <h1 data-page-heading>Projects</h1> }],
                },
            ],
            { initialEntries: ["/projects"] },
        );

        render(
            <I18nProvider locale="en">
                <AuthProvider>
                    <RouterProvider router={router} />
                </AuthProvider>
            </I18nProvider>,
        );

        expect(screen.getByRole("link", { name: "Skip to content" })).toHaveAttribute("href", "#main");
        expect(screen.getByRole("link", { name: "Projects" })).toHaveAttribute("aria-current", "page");
        expect(screen.getByRole("navigation", { name: "Main navigation" })).toBeInTheDocument();
        expect(screen.getByRole("combobox", { name: "Language" })).toHaveValue("en");

        await userEvent.tab();
        expect(screen.getByRole("link", { name: "Skip to content" })).toHaveFocus();
    });
});
