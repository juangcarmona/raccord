import { useState, type SyntheticEvent } from "react";
import { useLocation, useNavigate } from "react-router";
import { useI18n } from "../../shared/i18n/I18nProvider";
import { ApiError } from "../../shared/api/client";
import { useAuth } from "./AuthProvider";

export function SignInPage() {
    const { t } = useI18n();
    const navigate = useNavigate();
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [rememberMe, setRememberMe] = useState(false);
    const [error, setError] = useState<"invalid" | "network" | null>(null);
    const [pending, setPending] = useState(false);
    const auth = useAuth();
    const location = useLocation();

    async function submit(event: SyntheticEvent<HTMLFormElement>) {
        event.preventDefault();
        setError(null);
        setPending(true);
        try {
            await auth.signIn(email, password, rememberMe);
            const from = (location.state as { from?: { pathname?: string } } | null)?.from?.pathname ?? "/projects";
            await navigate(from, { replace: true });
        } catch (exception) {
            setError(exception instanceof ApiError && exception.status === 401 ? "invalid" : "network");
        } finally {
            setPending(false);
        }
    }

    return (
        <main className="auth-page">
            <form
                className="auth-form"
                onSubmit={(event) => {
                    void submit(event);
                }}
            >
                <img className="auth-form__mark" src="/raccord-mark.svg" alt="Raccord" />
                <h1 data-page-heading tabIndex={-1}>
                    {t("auth.title")}
                </h1>
                <label>
                    {t("auth.email")}
                    <input
                        type="email"
                        value={email}
                        onChange={(event) => {
                            setEmail(event.target.value);
                        }}
                        required
                    />
                </label>
                <label>
                    {t("auth.password")}
                    <input
                        type="password"
                        value={password}
                        onChange={(event) => {
                            setPassword(event.target.value);
                        }}
                        required
                    />
                </label>
                <label>
                    <input
                        type="checkbox"
                        checked={rememberMe}
                        onChange={(event) => {
                            setRememberMe(event.target.checked);
                        }}
                    />
                    {t("auth.remember")}
                </label>
                {error && <p role="alert">{t(error === "invalid" ? "auth.invalid" : "auth.unavailable")}</p>}
                <button className="button button--primary" type="submit" disabled={pending}>
                    {pending ? t("auth.pending") : t("auth.submit")}
                </button>
            </form>
        </main>
    );
}
