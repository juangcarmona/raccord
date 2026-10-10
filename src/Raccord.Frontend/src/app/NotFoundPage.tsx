import { Link } from 'react-router';
import { useI18n } from '../shared/i18n/I18nProvider';
import { PageHeader } from '../shared/ui/PageHeader';

export function NotFoundPage() {
  const { t } = useI18n();
  return (
    <>
      <PageHeader title={t('state.notFound')} />
      <p>
        <Link to="/projects">{t('nav.projects')}</Link>
      </p>
    </>
  );
}
