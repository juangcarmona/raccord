import { screen } from "@testing-library/react";
import userEvent from "@testing-library/user-event";
import { describe, expect, it } from "vitest";
import { SignInPage } from "./SignInPage";
import { AuthProvider } from "./AuthProvider";
import { renderAt, stubApi, json } from "../../test/render";

describe("SignInPage", () => {
    it("uses the typed API and shows the pending and invalid states", async () => {
        const requests = stubApi({
            "GET /api/auth/me": () => json({}, 401),
            "POST /api/auth/sign-in": async () => {
                await new Promise((resolve) => setTimeout(resolve, 5));
                return json({}, 401);
            },
        });
        renderAt("/sign-in", [
            {
                path: "/sign-in",
                element: (
                    <AuthProvider>
                        <SignInPage />
                    </AuthProvider>
                ),
            },
        ]);
        await userEvent.type(screen.getByLabelText("Correo electrónico"), "person@example.test");
        await userEvent.type(screen.getByLabelText("Contraseña"), "bad");
        const submit = screen.getByRole("button", { name: "Iniciar sesión" });
        await userEvent.click(submit);
        expect(await screen.findByRole("alert")).toHaveTextContent("no son válidos");
        expect(requests.some((request) => request.method === "POST" && request.path === "/api/auth/sign-in")).toBe(
            true,
        );
    });
});
