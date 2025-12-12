import { ComponentFixture, TestBed } from '@angular/core/testing';
import { BeneficiarioForm } from './beneficiario-form';
import { ReactiveFormsModule } from '@angular/forms';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { provideRouter, ActivatedRoute } from '@angular/router';
import { NzMessageService } from 'ng-zorro-antd/message';
import { of } from 'rxjs';
import { BeneficiarioService } from '../../../services/beneficiario.service';
import { PlanoService } from '../../../services/plano.service';
import { NZ_I18N, en_US } from 'ng-zorro-antd/i18n';

class MockBeneficiarioService {
  create(dados: any) { return of({ success: true }); }
  update(id: number, dados: any) { return of({ success: true }); }
  getById(id: number) { return of({ id: 1, nomeCompleto: 'Teste', planoId: 10 }); }
}

class MockPlanoService {
  getAll() {
    return of([
      { id: 10, nome: 'Plano Mock A', codigo_registro_ans: '111' },
      { id: 20, nome: 'Plano Mock B', codigo_registro_ans: '222' }
    ]);
  }
}

class MockNzMessageService {
  success(msg: string) { }
  error(msg: string) { }
}

describe('BeneficiarioForm', () => {
  let component: BeneficiarioForm;
  let fixture: ComponentFixture<BeneficiarioForm>;
  let beneficiarioService: BeneficiarioService;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        BeneficiarioForm,
        ReactiveFormsModule,
        NoopAnimationsModule
      ],
      providers: [
        provideRouter([]),
        { provide: BeneficiarioService, useClass: MockBeneficiarioService },
        { provide: PlanoService, useClass: MockPlanoService },
        { provide: NzMessageService, useClass: MockNzMessageService },
        {
          provide: ActivatedRoute,
          useValue: { snapshot: { paramMap: { get: () => null } } }
        },
        { provide: NZ_I18N, useValue: en_US }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(BeneficiarioForm);
    component = fixture.componentInstance;
    beneficiarioService = TestBed.inject(BeneficiarioService);
    fixture.detectChanges();
  });

  it('deve carregar a lista de planos para o select ao iniciar', () => {
    expect(component.listaPlanos.length).toBe(2);
    expect(component.listaPlanos[0].nome).toBe('Plano Mock A');
  });

  it('formulário deve ser inválido se o Plano não for selecionado', () => {
    component.form.controls['nomeCompleto'].setValue('Fulano');
    component.form.controls['cpf'].setValue('123');
    component.form.controls['dataNascimento'].setValue(new Date());
    component.form.controls['planoId'].setValue(null);

    expect(component.form.valid).toBeFalse();
  });

  it('deve chamar create ao salvar com formulário válido', () => {
    const spyCreate = spyOn(beneficiarioService, 'create').and.callThrough();

    component.form.patchValue({
      nomeCompleto: 'Fulano Certo',
      cpf: '000.000.000-00',
      dataNascimento: new Date(),
      planoId: 10
    });

    component.salvar();

    expect(spyCreate).toHaveBeenCalled();
  });
});
