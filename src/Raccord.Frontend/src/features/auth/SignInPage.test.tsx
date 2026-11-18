import { act, screen } from "@testing-library/react";
import userEvent from "@testing-library/user-event";
import { describe, expect, it } from "vitest";
import { SignInPage } from "./SignInPage";
import { AuthProvider } from "./AuthProvider";
import { renderAt, stubApi, json } from "../../test/render";

describe("SignInPage", () => {
    it("returns to the requested page after signing in", async () => {
        stubApi({
            "GET /api/auth/me": () => json({}, 401),
            "GET /api/setup/status": () => json({ required: false }),
            "POST /api/auth/sign-in": () => json({ id: "1", email: "person@example.test", preferredLanguage: "es" }),
        });
        const router = renderAt("/sign-in", [
            {
                path: "/sign-in",
                element: (
                    <AuthProvider>
                        <SignInPage />
                    </AuthProvider>
                ),
            },
            { path: "/account", element: <h1>My account</h1> },
            { path: "/projects", element: <h1>Projects</h1> },
        ]);
        await act(async () => {
            await router.navigate("/sign-in", { state: { from: { pathname: "/account" } } });
        });
        await userEvent.type(await screen.findByLabelText("Correo electrónico"), "person@example.test");
        await userEvent.type(screen.getByLabelText("Contraseña"), "DemoPassword123!");
        await userEvent.click(screen.getByRole("button", { name: "Iniciar sesión" }));
        expect(await screen.findByRole("heading", { name: "My account" })).toBeInTheDocument();
        expect(router.state.location.pathname).toBe("/account");
    });

    it("uses the typed API and shows the pending and invalid states", async () => {
        const requests = stubApi({
            "GET /api/auth/me": () => json({}, 401),
            "GET /api/setup/status": () => json({ required: false }),
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
        expect(await screen.findByRole("img", { name: "Raccord" })).toHaveAttribute("src", "/raccord-mark.svg");
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
