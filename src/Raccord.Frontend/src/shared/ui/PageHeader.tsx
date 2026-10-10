import type { ReactNode } from 'react';

/** Page title. The heading receives focus after navigation so screen-reader users hear the new page. */
export function PageHeader({ title, actions, children }: { title: string; actions?: ReactNode; children?: ReactNode }) {
  return (
    <header className="page-header">
      <div>
        <h1 tabIndex={-1} data-page-heading>
          {title}
        </h1>
        {children}
      </div>
      {actions && <div className="page-header__actions">{actions}</div>}
    </header>
  );
}
