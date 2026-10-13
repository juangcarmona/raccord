import { createContext, useContext, useEffect, useMemo, useState, type ReactNode } from "react";
import { api, unwrap } from "../../shared/api/client";

export type CurrentUser = { email: string };
type AuthState = { status: "loading" | "signed-out" | "signed-in"; user: CurrentUser | null };
type AuthContextValue = AuthState & {
    signIn: (email: string, password: string, rememberMe: boolean) => Promise<void>;
    signOut: () => Promise<void>;
};

const AuthContext = createContext<AuthContextValue | null>(null);

export function AuthProvider({ children }: { children: ReactNode }) {
    const [state, setState] = useState<AuthState>({ status: "loading", user: null });

    useEffect(() => {
        void unwrap(api.GET("/api/auth/me"))
            .then((user) => {
                setState({ status: "signed-in", user });
            })
            .catch(() => {
                setState({ status: "signed-out", user: null });
            });
    }, []);

    const value = useMemo<AuthContextValue>(
        () => ({
            ...state,
            async signIn(email, password, rememberMe) {
                const user = await unwrap(api.POST("/api/auth/sign-in", { body: { email, password, rememberMe } }));
                setState({ status: "signed-in", user });
            },
            async signOut() {
                await unwrap(api.POST("/api/auth/sign-out"));
                setState({ status: "signed-out", user: null });
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
