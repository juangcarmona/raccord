import { Navigate, Outlet } from "react-router";
import { useAuth } from "./AuthProvider";

export function RequireRoleManagement() {
    const auth = useAuth();
    if (auth.status === "loading") return <p>{"Loading…"}</p>;
    if (!auth.user?.canManageRoles) return <Navigate to="/projects" replace />;
    return <Outlet />;
}
