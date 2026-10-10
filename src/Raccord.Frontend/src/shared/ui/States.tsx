import type { ReactNode } from 'react';
import { ApiError } from '../api/client';
import type { QueryState } from '../hooks/useQuery';
import { useI18n } from '../i18n/I18nProvider';

export function Loading() {
  const { t } = useI18n();
  return (
    <p className="state" role="status" aria-live="polite">
      {t('state.loading')}
    </p>
  );
}

export function ErrorState({ error, onRetry }: { error: unknown; onRetry?: () => void }) {
  const { t, describe } = useI18n();
  const message =
    error instanceof ApiError
      ? error.isNotFound
        ? t('state.notFound')
        : describe('error', error.code, error.message)
      : t('state.error');
  return (
    <div className="state state--error" role="alert">
      <p>{message}</p>
      {onRetry && !(error instanceof ApiError && error.isNotFound) && (
        <button type="button" className="button" onClick={onRetry}>
          {t('state.retry')}
        </button>
      )}
    </div>
  );
}

export function Empty({ children }: { children: ReactNode }) {
  return <p className="state state--empty">{children}</p>;
}

/** Renders loading and error states, and the children once data is available. */
export function Async<T>({ query, children }: { query: QueryState<T> & { reload: () => void }; children: (data: T) => ReactNode }) {
  switch (query.status) {
    case 'loading':
      return <Loading />;
    case 'error':
      return <ErrorState error={query.error} onRetry={query.reload} />;
    case 'success':
      return <>{children(query.data)}</>;
  }
}
