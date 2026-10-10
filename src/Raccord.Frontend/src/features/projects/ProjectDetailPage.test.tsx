import { screen, waitFor } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import { afterEach, describe, expect, it, vi } from 'vitest';
import type { Schemas } from '../../shared/api/client';
import { json, renderAt, stubApi } from '../../test/render';
import { ProjectDetailPage } from './ProjectDetailPage';

const line = (id: string, description: string, billingState: string): Schemas['BillableLineResponse'] => ({
  id,
  kind: 'equipment',
  description,
  quantity: 1,
  unitPrice: 100,
  discountPercent: 0,
  vatRate: 21,
  netAmount: 100,
  billingState,
  draftId: null,
  invoiceId: null,
});

const project = (billableLines: Schemas['BillableLineResponse'][]): Schemas['ProjectResponse'] => ({
  id: 'p1',
  name: 'Rodaje campaña primavera',
  customerId: 'c1',
  customerName: 'Productora Ejemplo S.A.',
  startDate: '2026-09-14',
  endDate: '2026-09-18',
  currency: 'EUR',
  billableLines,
});

const routes = [
  { path: '/projects/:id', element: <ProjectDetailPage /> },
  { path: '/drafts/:id', element: <p>Página de borrador</p> },
];

describe('ProjectDetailPage', () => {
  afterEach(() => {
    vi.unstubAllGlobals();
  });

  it('only lets unbilled lines be selected for a draft', async () => {
    stubApi({
      'GET /api/projects/p1': () => json(project([line('a', 'Cámara', 'unbilled'), line('b', 'Óptica', 'invoiced')])),
    });

    renderAt('/projects/p1', routes);

    expect(await screen.findByRole('checkbox', { name: 'Seleccionar «Cámara»' })).toBeEnabled();
    expect(screen.getByRole('checkbox', { name: 'Seleccionar «Óptica»' })).toBeDisabled();
    expect(screen.getByText('Facturada')).toBeInTheDocument();
    expect(screen.getByRole('button', { name: 'Crear borrador con 0 línea(s)' })).toBeDisabled();
  });

  it('creates a draft from the selected lines and opens it', async () => {
    const requests = stubApi({
      'GET /api/projects/p1': () => json(project([line('a', 'Cámara', 'unbilled'), line('b', 'Gaffer', 'unbilled')])),
      'POST /api/invoice-drafts': () => json({ id: 'd9' }, 201),
    });
    const router = renderAt('/projects/p1', routes);
    const user = userEvent.setup();

    await user.click(await screen.findByRole('checkbox', { name: 'Seleccionar «Gaffer»' }));
    await user.click(screen.getByRole('button', { name: 'Crear borrador con 1 línea(s)' }));

    await waitFor(() => {
      expect(router.state.location.pathname).toBe('/drafts/d9');
    });
    expect(requests.find((request) => request.method === 'POST')?.body).toEqual({ projectId: 'p1', billableLineIds: ['b'], seriesCode: null });
  });

  it('says when a project has nothing to bill', async () => {
    stubApi({ 'GET /api/projects/p1': () => json(project([])) });

    renderAt('/projects/p1', routes);

    expect(await screen.findByText('Este proyecto no tiene líneas facturables.')).toBeInTheDocument();
  });

  it('reports a missing project instead of an empty page', async () => {
    stubApi({});

    renderAt('/projects/p1', routes);

    expect(await screen.findByRole('alert')).toHaveTextContent('No existe o ya no está disponible.');
  });
});
