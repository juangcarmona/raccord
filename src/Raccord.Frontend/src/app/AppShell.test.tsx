import { render, screen } from "@testing-library/react";
import userEvent from "@testing-library/user-event";
import { createMemoryRouter, RouterProvider } from "react-router";
import { describe, expect, it } from "vitest";
import { I18nProvider } from "../shared/i18n/I18nProvider";
import { AppShell } from "./AppShell";
import { AuthProvider } from "../features/auth/AuthProvider";
import { json, stubApi } from "../test/render";

describe("AppShell", () => {
    it("exposes the current section, account entry, and skip link", async () => {
        stubApi({
            "GET /api/auth/me": () =>
                json({
                    email: "person@example.com",
                    displayName: "Alex",
                    preferredLanguage: "en",
                    canManageRoles: true,
                }),
        });
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
        expect(await screen.findByRole("link", { name: "My account" })).toHaveAttribute("href", "/account");
        expect(screen.getByRole("navigation", { name: "Main navigation" })).toHaveTextContent("Roles and permissions");
        expect(document.querySelector(".brand img")).toHaveAttribute("src", "/raccord-mark.svg");
        expect(document.querySelector(".brand img")).toHaveAttribute("aria-hidden", "true");

        await userEvent.tab();
        expect(screen.getByRole("link", { name: "Skip to content" })).toHaveFocus();
    });
});
