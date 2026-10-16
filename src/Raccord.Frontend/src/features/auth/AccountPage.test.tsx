import { screen, waitFor } from "@testing-library/react";
import userEvent from "@testing-library/user-event";
import { describe, expect, it } from "vitest";
import { renderAt, stubApi, json } from "../../test/render";
import { AccountPage } from "./AccountPage";
import { AuthProvider } from "./AuthProvider";

const user = {
    email: "admin@example.com",
    displayName: "Admin",
    preferredLanguage: "es",
    canManageRoles: true,
};

describe("AccountPage", () => {
    it("saves profile preferences and shows the role link for an authorized user", async () => {
        const requests = stubApi({
            "GET /api/auth/me": () => json(user),
            "PUT /api/auth/me": () => json({ ...user, displayName: "Ana", preferredLanguage: "en" }),
        });
        renderAt("/account", [
            {
                path: "/account",
                element: (
                    <AuthProvider>
                        <AccountPage />
                    </AuthProvider>
                ),
            },
        ]);

        await screen.findByDisplayValue("Admin");
        expect(screen.getByLabelText(/Correo de inicio de sesión/)).toHaveAttribute("readonly");
        expect(screen.getByRole("link", { name: "Administrar roles y permisos" })).toHaveAttribute(
            "href",
            "/settings/roles",
        );
        await userEvent.clear(screen.getByLabelText("Nombre para mostrar"));
        await userEvent.type(screen.getByLabelText("Nombre para mostrar"), "Ana");
        await userEvent.selectOptions(screen.getByLabelText("Idioma preferido"), "en");
        await userEvent.click(screen.getByRole("button", { name: "Guardar" }));

        await screen.findByText("Preferencias guardadas.");
        await waitFor(() => {
            expect(requests.find((request) => request.method === "PUT")?.body).toEqual({
                displayName: "Ana",
                preferredLanguage: "en",
            });
        });
    });

    it("hides role administration when the current user has no role permission", async () => {
        stubApi({ "GET /api/auth/me": () => json({ ...user, canManageRoles: false }) });
        renderAt("/account", [
            {
                path: "/account",
                element: (
                    <AuthProvider>
                        <AccountPage />
                    </AuthProvider>
                ),
            },
        ]);

        await screen.findByDisplayValue("Admin");
        expect(screen.queryByRole("link", { name: "Administrar roles y permisos" })).not.toBeInTheDocument();
    });
});
