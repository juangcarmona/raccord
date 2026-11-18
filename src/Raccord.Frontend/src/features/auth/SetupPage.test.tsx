import { screen } from "@testing-library/react";
import userEvent from "@testing-library/user-event";
import { describe, expect, it, vi } from "vitest";
import { AuthProvider } from "./AuthProvider";
import { SetupPage } from "./SetupPage";
import { SignInPage } from "./SignInPage";
import { RequireAuth } from "./RequireAuth";
import { json, renderAt, stubApi } from "../../test/render";

const identity = {
    id: "1",
    email: "first@example.test",
    displayName: null,
    preferredLanguage: "en",
    canManageRoles: true,
};
function renderSetup(path = "/setup") {
    const loadProjects = vi.fn(async () => {
        await new Promise((resolve) => setTimeout(resolve, 10));
        return null;
    });
    return renderAt(path, [
        {
            path: "/protected",
            element: (
                <AuthProvider>
                    <RequireAuth />
                </AuthProvider>
            ),
        },
        {
            path: "/setup",
            element: (
                <AuthProvider>
                    <SetupPage />
                </AuthProvider>
            ),
        },
        {
            path: "/sign-in",
            element: (
                <AuthProvider>
                    <SignInPage />
                </AuthProvider>
            ),
        },
        { path: "/projects", loader: loadProjects, element: <h1>Projects</h1> },
    ]);
}
async function submit() {
    await userEvent.type(await screen.findByLabelText("Correo electrónico"), "first@example.test");
    await userEvent.type(screen.getByLabelText("Contraseña"), "SyntheticPassword123!");
    await userEvent.click(screen.getByRole("button", { name: "Crear cuenta de administrador" }));
}

describe("first-run setup", () => {
    it.each(["/protected", "/sign-in", "/setup"])("routes fresh visitors from %s", async (path) => {
        stubApi({ "GET /api/auth/me": () => json({}, 401), "GET /api/setup/status": () => json({ required: true }) });
        renderSetup(path);
        expect(await screen.findByRole("heading", { name: "Crear el primer administrador" })).toBeInTheDocument();
    });
    it("creates once, retains the warning, then signs in without remembering the session", async () => {
        const requests = stubApi({
            "GET /api/auth/me": () => json({}, 401),
            "GET /api/setup/status": () => json({ required: true }),
            "POST /api/setup": () => json({ passwordCheck: "matched" }),
            "POST /api/auth/sign-in": () => json(identity),
        });
        const router = renderSetup();
        await submit();
        expect(await screen.findByText(/Esta contraseña aparece en filtraciones/)).toBeInTheDocument();
        expect(screen.queryByRole("button", { name: "Crear cuenta de administrador" })).not.toBeInTheDocument();
        expect(requests.filter((r) => r.path === "/api/auth/sign-in")).toHaveLength(0);
        await userEvent.click(screen.getByRole("button", { name: "Continuar a la aplicación" }));
        expect(await screen.findByRole("heading", { name: "Projects" })).toBeInTheDocument();
        expect(router.routes.find((route) => route.path === "/projects")?.loader).toHaveBeenCalledTimes(1);
        expect(requests.filter((r) => r.path === "/api/setup")).toHaveLength(1);
        expect(requests.find((r) => r.path === "/api/auth/sign-in")?.body).toEqual({
            email: "first@example.test",
            password: "SyntheticPassword123!",
            rememberMe: false,
        });
    });
    it.each([false, null])("does not enable setup for status %s", async (required) => {
        stubApi({
            "GET /api/auth/me": () => json({}, 401),
            "GET /api/setup/status": () => (required === null ? json({}, 503) : json({ required })),
        });
        renderSetup();
        if (required === null) expect(await screen.findByRole("alert")).toHaveTextContent("No se puede comprobar");
        else expect(await screen.findByRole("heading", { name: "Iniciar sesión" })).toBeInTheDocument();
        expect(screen.queryByRole("button", { name: "Crear cuenta de administrador" })).not.toBeInTheDocument();
    });
    it("routes signed-in users away even with unknown setup status", async () => {
        stubApi({ "GET /api/auth/me": () => json(identity), "GET /api/setup/status": () => json({}, 503) });
        renderSetup();
        expect(await screen.findByRole("heading", { name: "Projects" })).toBeInTheDocument();
    });
    it("removes stale setup after conflict", async () => {
        const requests = stubApi({
            "GET /api/auth/me": () => json({}, 401),
            "GET /api/setup/status": () => json({ required: true }),
            "POST /api/setup": () => json({ code: "setup-completed" }, 409),
        });
        renderSetup();
        await submit();
        expect(await screen.findByRole("alert")).toHaveTextContent("Ya existe una cuenta");
        expect(screen.getByRole("link", { name: "Ir a iniciar sesión" })).toHaveAttribute("href", "/sign-in");
        expect(screen.queryByLabelText("Contraseña")).not.toBeInTheDocument();
        expect(requests.filter((r) => r.path === "/api/setup")).toHaveLength(1);
    });
    it("keeps validation recoverable", async () => {
        stubApi({
            "GET /api/auth/me": () => json({}, 401),
            "GET /api/setup/status": () => json({ required: true }),
            "POST /api/setup": () => json({}, 400),
        });
        renderSetup();
        await submit();
        expect(await screen.findByRole("alert")).toHaveTextContent("Comprueba el correo");
        expect(screen.getByLabelText("Correo electrónico")).toHaveValue("first@example.test");
    });
    it("distinguishes unavailable checks and never repeats setup after sign-in failure", async () => {
        const requests = stubApi({
            "GET /api/auth/me": () => json({}, 401),
            "GET /api/setup/status": () => json({ required: true }),
            "POST /api/setup": () => json({ passwordCheck: "unavailable" }),
            "POST /api/auth/sign-in": () => json({}, 503),
        });
        renderSetup();
        await submit();
        expect(await screen.findByText(/No se pudo comprobar si la contraseña/)).toBeInTheDocument();
        await userEvent.click(screen.getByRole("button", { name: "Continuar a la aplicación" }));
        expect(await screen.findByRole("alert")).toHaveTextContent("La cuenta se creó");
        expect(screen.queryByRole("button")).not.toBeInTheDocument();
        expect(requests.filter((r) => r.path === "/api/setup")).toHaveLength(1);
    });
});
