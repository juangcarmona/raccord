import { createContext, useContext, useEffect, useMemo, useState, type ReactNode } from "react";
import { api, unwrap } from "../../shared/api/client";

export type CurrentUser = {
    id: string;
    email: string;
    displayName: string | null;
    preferredLanguage: string;
    canManageRoles: boolean;
};
type AuthState = {
    status: "loading" | "signed-out" | "signed-in";
    user: CurrentUser | null;
    setupRequired: boolean | null;
};
export type PasswordCheck = "clear" | "matched" | "unavailable";
type AuthContextValue = AuthState & {
    signIn: (email: string, password: string, rememberMe: boolean) => Promise<void>;
    signOut: () => Promise<void>;
    createFirstAdministrator: (email: string, password: string) => Promise<PasswordCheck>;
    updateProfile: (displayName: string, preferredLanguage: "es" | "en") => Promise<void>;
};

const AuthContext = createContext<AuthContextValue | null>(null);

export function AuthProvider({ children }: { children: ReactNode }) {
    const [state, setState] = useState<AuthState>({ status: "loading", user: null, setupRequired: null });

    useEffect(() => {
        let active = true;
        void Promise.allSettled([unwrap(api.GET("/api/auth/me")), unwrap(api.GET("/api/setup/status"))]).then(
            ([identity, setup]) => {
                if (!active) return;
                const setupRequired =
                    setup.status === "fulfilled" && typeof setup.value.required === "boolean"
                        ? setup.value.required
                        : null;
                setState(
                    identity.status === "fulfilled"
                        ? { status: "signed-in", user: identity.value, setupRequired }
                        : { status: "signed-out", user: null, setupRequired },
                );
            },
        );
        return () => {
            active = false;
        };
    }, []);

    const value = useMemo<AuthContextValue>(
        () => ({
            ...state,
            async signIn(email, password, rememberMe) {
                const user = await unwrap(api.POST("/api/auth/sign-in", { body: { email, password, rememberMe } }));
                setState({ status: "signed-in", user, setupRequired: false });
            },
            async signOut() {
                await unwrap(api.POST("/api/auth/sign-out"));
                setState({ status: "signed-out", user: null, setupRequired: false });
            },
            async createFirstAdministrator(email, password) {
                const { data: body, response } = await api.POST("/api/setup", { body: { email, password } });
                if (!response.ok) {
                    if (response.status === 409) throw new Error("completed");
                    throw new Error(response.status === 400 ? "invalid" : "unavailable");
                }
                setState((current) => ({ ...current, setupRequired: false }));
                return body?.passwordCheck === "matched" || body?.passwordCheck === "clear"
                    ? body.passwordCheck
                    : "unavailable";
            },
            async updateProfile(displayName, preferredLanguage) {
                const user = await unwrap(api.PUT("/api/auth/me", { body: { displayName, preferredLanguage } }));
                setState({ status: "signed-in", user, setupRequired: false });
            },
        }),
        [state],
    );

    return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
}

export function useAuth(): AuthContextValue {
    const auth = useContext(AuthContext);
    if (!auth) throw new Error("useAuth must be used within AuthProvider");
    return auth;
}
