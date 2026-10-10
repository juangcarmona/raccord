import { screen, waitFor } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import { afterEach, describe, expect, it, vi } from 'vitest';
import type { Schemas } from '../../shared/api/client';
import { json, renderAt, stubApi } from '../../test/render';
import { DraftPage } from './DraftPage';

const draft: Schemas['InvoiceDraftResponse'] = {
  id: 'd1',
  status: 'open',
  projectId: 'p1',
  projectName: 'Rodaje campaña primavera',
  customerId: 'c1',
  customerName: 'Productora Ejemplo S.A.',
  seriesCode: 'F',
  operationDate: '2026-09-18',
  createdAt: '2026-10-06T10:00:00Z',
  lines: [
    { id: 'l1', position: 1, description: 'Kit de cámara', quantity: 2, unitPrice: 450, discountPercent: 0, vatRate: 21, netAmount: 900 },
  ],
  totals: { currency: 'EUR', taxableBase: 900, vatAmount: 189, total: 1089, vatBreakdown: [{ rate: 21, taxableBase: 900, vatAmount: 189 }] },
  validationIssues: [],
  issuedInvoiceId: null,
};

const routes = [
  { path: '/drafts/:id', element: <DraftPage /> },
  { path: '/invoices/:id', element: <p>Página de factura</p> },
];

describe('DraftPage', () => {
  afterEach(() => {
    vi.unstubAllGlobals();
  });

  it('shows server totals and explains, in the user language, why the draft cannot be issued', async () => {
    stubApi({
      'GET /api/invoice-drafts/d1': () =>
        json({ ...draft, validationIssues: [{ code: 'recipient.tax-id-missing', message: 'The recipient tax ID is missing.' }] }),
    });

    renderAt('/drafts/d1', routes);

    expect(await screen.findByRole('heading', { level: 1, name: /Rodaje campaña primavera/ })).toBeInTheDocument();
    expect(screen.getByText('Falta el NIF del cliente.')).toBeInTheDocument();
    expect(screen.getByText(/1089,00/)).toBeInTheDocument();
    expect(screen.getByRole('button', { name: 'Emitir factura' })).toBeDisabled();
  });

  it('issues the invoice with the chosen date and opens it', async () => {
    const requests = stubApi({
      'GET /api/invoice-drafts/d1': () => json(draft),
      'POST /api/invoice-drafts/d1/finalize': () => json({ id: 'i1', number: 'F2026-00001' }, 201),
    });
    const router = renderAt('/drafts/d1', routes);
    const user = userEvent.setup();

    const date = await screen.findByLabelText('Fecha de expedición');
    await user.clear(date);
    await user.type(date, '2026-10-06');
    await user.click(screen.getByRole('button', { name: 'Emitir factura' }));

    await waitFor(() => {
      expect(router.state.location.pathname).toBe('/invoices/i1');
    });
    expect(requests.find((request) => request.method === 'POST')?.body).toEqual({ issueDate: '2026-10-06' });
  });

  it('shows every issue returned when finalization is refused', async () => {
    stubApi({
      'GET /api/invoice-drafts/d1': () => json(draft),
      'POST /api/invoice-drafts/d1/finalize': () =>
        json({ code: 'draft.invalid', title: 'Invalid', issues: [{ code: 'numbering.issue-date-not-accepted', message: 'x' }] }, 422),
    });
    renderAt('/drafts/d1', routes);
    const user = userEvent.setup();

    await user.click(await screen.findByRole('button', { name: 'Emitir factura' }));

    expect(await screen.findByRole('alert')).toHaveTextContent('La fecha de expedición es anterior a la última factura de la serie');
  });
});
