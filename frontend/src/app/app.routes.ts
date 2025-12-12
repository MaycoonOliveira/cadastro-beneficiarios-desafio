import { Routes } from '@angular/router';
import { PlanoForm } from './pages/plano/plano-form/plano-form';
import { PlanoList } from './pages/plano/plano-list/plano-list';
import { BeneficiarioList } from './pages/beneficiario/beneficiario-list/beneficiario-list';
import { BeneficiarioForm } from './pages/beneficiario/beneficiario-form/beneficiario-form';

export const routes: Routes = [
  { path: '', redirectTo: 'planos', pathMatch: 'full' },

  { path: 'planos', component: PlanoList },
  { path: 'planos/novo', component: PlanoForm },
  { path: 'planos/editar/:id', component: PlanoForm },

  { path: 'beneficiarios', component: BeneficiarioList },
  { path: 'beneficiarios/novo', component: BeneficiarioForm },
  { path: 'beneficiarios/editar/:id', component: BeneficiarioForm },
];
