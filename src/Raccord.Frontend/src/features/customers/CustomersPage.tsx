import { Link } from 'react-router';
import { api, unwrap } from '../../shared/api/client';
import { useQuery } from '../../shared/hooks/useQuery';
import { useI18n } from '../../shared/i18n/I18nProvider';
import { PageHeader } from '../../shared/ui/PageHeader';
import { Async, Empty } from '../../shared/ui/States';

export function CustomersPage() {
  const { t } = useI18n();
  const query = useQuery('customers', () => unwrap(api.GET('/api/customers')));
  return (
    <>
      <PageHeader title={t('customers.title')} actions={<Link className="button button--primary" to="/customers/new">{t('customers.new')}</Link>} />
      <Async query={query}>
        {(customers) =>
          customers.length === 0 ? (
            <Empty>{t('customers.empty')}</Empty>
          ) : (
            <div className="table-scroll">
              <table>
                <caption className="visually-hidden">{t('customers.title')}</caption>
                <thead>
                  <tr>
                    <th scope="col">{t('common.legalName')}</th>
                    <th scope="col">{t('common.taxId')}</th>
                    <th scope="col">{t('common.city')}</th>
                    <th scope="col">{t('common.email')}</th>
                    <th scope="col"><span className="visually-hidden">{t('common.actions')}</span></th>
                  </tr>
                </thead>
                <tbody>
                  {customers.map((customer) => (
                    <tr key={customer.id}>
                      <th scope="row">{customer.legalName}</th>
                      <td>{customer.taxId ?? '—'}</td>
                      <td>{customer.address?.city ?? '—'}</td>
                      <td>{customer.email ?? '—'}</td>
                      <td>
                        <span className={`badge ${customer.isInvoiceable ? 'badge--invoiced' : 'badge--warning'}`}>
                          {t(customer.isInvoiceable ? 'customers.invoiceable' : 'customers.incomplete')}
                        </span>
                      </td>
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
