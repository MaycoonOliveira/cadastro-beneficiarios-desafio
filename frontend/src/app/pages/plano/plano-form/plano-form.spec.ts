import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PlanoForm } from './plano-form';
import { ReactiveFormsModule } from '@angular/forms';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { provideRouter, ActivatedRoute } from '@angular/router';
import { NzMessageService } from 'ng-zorro-antd/message';
import { of } from 'rxjs';
import { PlanoService } from '../../../services/plano.service';

class MockPlanoService {
  create(plano: any) { return of({ success: true }); }
  update(id: number, plano: any) { return of({ success: true }); }
  getById(id: number) { return of({ id: 1, nome: 'Teste', codigo_registro_ans: '123' }); }
}

class MockNzMessageService {
  success(msg: string) { }
  error(msg: string) { }
}

describe('PlanoForm', () => {
  let component: PlanoForm;
  let fixture: ComponentFixture<PlanoForm>;
  let planoService: PlanoService;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        PlanoForm,
        ReactiveFormsModule,
      ],
      providers: [
        provideRouter([]),
        { provide: PlanoService, useClass: MockPlanoService },
        { provide: NzMessageService, useClass: MockNzMessageService },
        {
          provide: ActivatedRoute,
          useValue: { snapshot: { paramMap: { get: () => null } } }
        }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(PlanoForm);
    component = fixture.componentInstance;
    planoService = TestBed.inject(PlanoService);
    fixture.detectChanges();
  });

  it('deve ser criado', () => {
    expect(component).toBeTruthy();
  });

  it('o formulário deve ser inválido quando vazio', () => {
    expect(component.form.valid).toBeFalse();
  });

  it('o formulário deve ser válido quando preenchido corretamente', () => {
    component.form.controls['nome'].setValue('Plano Ouro');
    component.form.controls['codigo_registro_ans'].setValue('123456');

    expect(component.form.valid).toBeTrue();
  });

  it('deve chamar o método create do serviço ao salvar um novo plano', () => {
    const spyCreate = spyOn(planoService, 'create').and.callThrough();

    component.form.controls['nome'].setValue('Novo Plano');
    component.form.controls['codigo_registro_ans'].setValue('999');
    component.salvar();

    expect(spyCreate).toHaveBeenCalled();
  });
});
