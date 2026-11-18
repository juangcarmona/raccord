import { render, screen } from "@testing-library/react";
import userEvent from "@testing-library/user-event";
import { createMemoryRouter, RouterProvider } from "react-router";
import { describe, expect, it, vi } from "vitest";
import { I18nProvider } from "../shared/i18n/I18nProvider";
import { AppShell } from "./AppShell";
import { AuthProvider } from "../features/auth/AuthProvider";
import { RequireAuth } from "../features/auth/RequireAuth";
import { SignInPage } from "../features/auth/SignInPage";
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
    it("redirects once after signing out without returning to protected content", async () => {
        const requests = stubApi({
            "GET /api/auth/me": () =>
                json({ id: "1", email: "person@example.test", preferredLanguage: "en", canManageRoles: true }),
            "GET /api/setup/status": () => json({ required: false }),
            "POST /api/auth/sign-out": () => new Response(null, { status: 200 }),
        });
        const loadSignIn = vi.fn(async () => {
            await new Promise((resolve) => setTimeout(resolve, 10));
            return null;
        });
        const router = createMemoryRouter(
            [
                {
                    element: <RequireAuth />,
                    children: [
                        {
                            element: <AppShell />,
                            children: [{ path: "/projects", element: <h1>Protected projects</h1> }],
                        },
                    ],
                },
                { path: "/sign-in", loader: loadSignIn, element: <SignInPage /> },
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
        await userEvent.click(await screen.findByRole("button", { name: "Sign out" }));
        expect(await screen.findByRole("heading", { name: "Sign in" })).toBeInTheDocument();
        expect(screen.queryByRole("heading", { name: "Protected projects" })).not.toBeInTheDocument();
        expect(router.state.location.pathname).toBe("/sign-in");
        expect(loadSignIn).toHaveBeenCalledTimes(1);
        expect(requests.filter((request) => request.path === "/api/auth/sign-out")).toHaveLength(1);
    });
});
