import { Navigate, Outlet, useLocation } from "react-router";
import { useAuth } from "./AuthProvider";

export function RequireAuth() {
    const auth = useAuth();
    const location = useLocation();
    if (auth.status === "loading") return <p>{"Loading…"}</p>;
    if (!auth.user) return <Navigate to="/sign-in" replace state={{ from: location }} />;
    return <Outlet />;
}
