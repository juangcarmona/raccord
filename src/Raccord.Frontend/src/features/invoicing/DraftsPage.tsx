import { Link } from 'react-router';
import { api, unwrap } from '../../shared/api/client';
import { useQuery } from '../../shared/hooks/useQuery';
import { useI18n } from '../../shared/i18n/I18nProvider';
import { PageHeader } from '../../shared/ui/PageHeader';
import { Async, Empty } from '../../shared/ui/States';

export function DraftsPage() {
  const { t, format } = useI18n();
  const query = useQuery('drafts', () => unwrap(api.GET('/api/invoice-drafts')));
  return (
    <>
      <PageHeader title={t('drafts.title')} />
      <Async query={query}>
        {(drafts) =>
          drafts.length === 0 ? (
            <Empty>{t('drafts.empty')}</Empty>
          ) : (
            <div className="table-scroll">
              <table>
                <caption className="visually-hidden">{t('drafts.title')}</caption>
                <thead>
                  <tr>
                    <th scope="col">{t('common.project')}</th>
                    <th scope="col">{t('common.customer')}</th>
                    <th scope="col" className="num">{t('projects.lines')}</th>
                    <th scope="col" className="num">{t('common.total')}</th>
                  </tr>
                </thead>
                <tbody>
                  {drafts.map((draft) => (
                    <tr key={draft.id}>
                      <th scope="row"><Link to={`/drafts/${draft.id}`}>{draft.projectName}</Link></th>
                      <td>{draft.customerName}</td>
                      <td className="num">{draft.lineCount}</td>
                      <td className="num">{format.money(draft.total, draft.currency)}</td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>
          )
        }
      </Async>
    </>
  );
}
