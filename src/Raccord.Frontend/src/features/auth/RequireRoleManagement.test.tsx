import { render, screen } from "@testing-library/react";
import { createMemoryRouter, Outlet, RouterProvider } from "react-router";
import { describe, expect, it } from "vitest";
import { json, stubApi } from "../../test/render";
import { AuthProvider } from "./AuthProvider";
import { RequireRoleManagement } from "./RequireRoleManagement";

describe("RequireRoleManagement", () => {
    it("redirects an authenticated user without role permission away from a direct visit", async () => {
        stubApi({
            "GET /api/auth/me": () =>
                json({
                    email: "crew@example.com",
                    displayName: "Crew",
                    preferredLanguage: "es",
                    canManageRoles: false,
                }),
        });
        const router = createMemoryRouter(
            [
                {
                    element: (
                        <AuthProvider>
                            <Outlet />
                        </AuthProvider>
                    ),
                    children: [
                        {
                            element: <RequireRoleManagement />,
                            children: [{ path: "/settings/roles", element: <h1>Role administration</h1> }],
                        },
                        { path: "/projects", element: <h1>Projects</h1> },
                    ],
                },
            ],
            { initialEntries: ["/settings/roles"] },
        );
        render(<RouterProvider router={router} />);

        expect(await screen.findByRole("heading", { name: "Projects" })).toBeInTheDocument();
        expect(screen.queryByRole("heading", { name: "Role administration" })).not.toBeInTheDocument();
    });
});
