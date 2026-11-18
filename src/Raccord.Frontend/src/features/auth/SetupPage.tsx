import { useState, type SyntheticEvent } from "react";
import { Navigate } from "react-router";
import { useI18n } from "../../shared/i18n/I18nProvider";
import { useAuth, type PasswordCheck } from "./AuthProvider";

export function SetupPage() {
    const { t } = useI18n();
    const auth = useAuth();
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [error, setError] = useState<"invalid" | "completed" | "unavailable" | "sign-in" | null>(null);
    const [pending, setPending] = useState(false);
    const [created, setCreated] = useState(false);
    const [passwordCheck, setPasswordCheck] = useState<PasswordCheck>("unavailable");

    if (auth.status === "loading")
        return (
            <main className="auth-page">
                <p role="status">{t("setup.checking")}</p>
            </main>
        );
    if (auth.user) return <Navigate to="/projects" replace />;
    if (auth.setupRequired === false && !created) return <Navigate to="/sign-in" replace />;

    async function submit(event: SyntheticEvent<HTMLFormElement>) {
        event.preventDefault();
        if (pending || created || error === "completed") return;
        setError(null);
        setPending(true);
        try {
            const check = await auth.createFirstAdministrator(email, password);
            setCreated(true);
            setPasswordCheck(check);
        } catch (exception) {
            const message = exception instanceof Error ? exception.message : "unavailable";
            setError(message === "completed" || message === "invalid" ? message : "unavailable");
            if (message === "completed") setPassword("");
        } finally {
            setPending(false);
        }
    }

    async function continueToApplication() {
        if (pending) return;
        setPending(true);
        try {
            await auth.signIn(email, password, false);
            setPassword("");
        } catch {
            setPassword("");
            setError("sign-in");
        } finally {
            setPending(false);
        }
    }

    return (
        <main className="auth-page">
            <section className="auth-form" aria-labelledby="setup-heading">
                <img className="auth-form__mark" src="/raccord-mark.svg" alt="Raccord" />
                <h1 id="setup-heading" data-page-heading tabIndex={-1}>
                    {t("setup.title")}
                </h1>
                {created ? (
                    <>
                        <p role="status">{t("setup.created")}</p>
                        {passwordCheck !== "clear" && (
                            <p role="status">
                                {t(
                                    passwordCheck === "matched"
                                        ? "setup.password.matched"
                                        : "setup.password.unavailable",
                                )}
                            </p>
                        )}
                        {error === "sign-in" ? (
                            <p role="alert">{t("setup.error.sign-in")}</p>
                        ) : (
                            <button
                                className="button button--primary"
                                type="button"
                                disabled={pending}
                                onClick={() => {
                                    void continueToApplication();
                                }}
                            >
                                {t("setup.continue")}
                            </button>
                        )}
                        <a href="/sign-in">{t("setup.signIn")}</a>
                    </>
                ) : auth.setupRequired === null ? (
                    <>
                        <p role="alert">{t("setup.error.unavailable")}</p>
                        <button
                            type="button"
                            onClick={() => {
                                globalThis.location.reload();
                            }}
                        >
                            {t("state.retry")}
                        </button>
                        <a href="/sign-in">{t("setup.signIn")}</a>
                    </>
                ) : error === "completed" ? (
                    <>
                        <p role="alert">{t("setup.error.completed")}</p>
                        <a href="/sign-in">{t("setup.signIn")}</a>
                    </>
                ) : (
                    <form
                        onSubmit={(event) => {
                            void submit(event);
                        }}
                    >
                        <p>{t("setup.intro")}</p>
                        <label>
                            {t("auth.email")}
                            <input
                                type="email"
                                autoComplete="email"
                                value={email}
                                onChange={(event) => {
                                    setEmail(event.target.value);
                                }}
                                required
                                disabled={pending}
                            />
                        </label>
                        <label>
                            {t("auth.password")}
                            <input
                                type="password"
                                autoComplete="new-password"
                                minLength={12}
                                aria-describedby="setup-password-hint"
                                value={password}
                                onChange={(event) => {
                                    setPassword(event.target.value);
                                }}
                                required
                                disabled={pending}
                            />
                        </label>
                        <p id="setup-password-hint">{t("setup.password.hint")}</p>
                        {error && (
                            <p role="alert">
                                {t(error === "invalid" ? "setup.error.invalid" : "setup.error.unavailable")}
                            </p>
                        )}
                        <button className="button button--primary" type="submit" disabled={pending}>
                            {t(pending ? "setup.pending" : "setup.submit")}
                        </button>
                    </form>
                )}
            </section>
        </main>
    );
}
