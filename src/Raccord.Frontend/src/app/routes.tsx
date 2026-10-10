import { Navigate, type RouteObject } from 'react-router';
import { CustomersPage } from '../features/customers/CustomersPage';
import { NewCustomerPage } from '../features/customers/NewCustomerPage';
import { DraftPage } from '../features/invoicing/DraftPage';
import { DraftsPage } from '../features/invoicing/DraftsPage';
import { InvoicePage } from '../features/invoicing/InvoicePage';
import { InvoicesPage } from '../features/invoicing/InvoicesPage';
import { IssuerSettingsPage } from '../features/organization/IssuerSettingsPage';
import { NewProjectPage } from '../features/projects/NewProjectPage';
import { ProjectDetailPage } from '../features/projects/ProjectDetailPage';
import { ProjectsPage } from '../features/projects/ProjectsPage';
import { AppShell } from './AppShell';
import { NotFoundPage } from './NotFoundPage';

export const routes: RouteObject[] = [
  {
    element: <AppShell />,
    children: [
      { index: true, element: <Navigate to="/projects" replace /> },
      { path: 'projects', element: <ProjectsPage /> },
      { path: 'projects/new', element: <NewProjectPage /> },
      { path: 'projects/:id', element: <ProjectDetailPage /> },
      { path: 'customers', element: <CustomersPage /> },
      { path: 'customers/new', element: <NewCustomerPage /> },
      { path: 'drafts', element: <DraftsPage /> },
      { path: 'drafts/:id', element: <DraftPage /> },
      { path: 'invoices', element: <InvoicesPage /> },
      { path: 'invoices/:id', element: <InvoicePage /> },
      { path: 'settings/issuer', element: <IssuerSettingsPage /> },
      { path: '*', element: <NotFoundPage /> },
    ],
  },
];
