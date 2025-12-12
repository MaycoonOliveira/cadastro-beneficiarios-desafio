import { ComponentFixture, TestBed } from '@angular/core/testing';
import { BeneficiarioList } from './beneficiario-list';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { provideRouter } from '@angular/router';
import { NzMessageService } from 'ng-zorro-antd/message';
import { of } from 'rxjs';
import { BeneficiarioService } from '../../../services/beneficiario.service';

const listaMock = [
  { id: 1, nomeCompleto: 'Maria', cpf: '111', plano: { nome: 'Plano Ouro' } },
  { id: 2, nomeCompleto: 'José', cpf: '222', plano: { nome: 'Plano Prata' } }
];

class MockBeneficiarioService {
  getAll() { return of(listaMock); }
  delete(id: number, prioridade: number) { return of({ success: true }); }
}
class MockNzMessageService {
  success(msg: string) { }
  error(msg: string) { }
}

describe('BeneficiarioList', () => {
  let component: BeneficiarioList;
  let fixture: ComponentFixture<BeneficiarioList>;
  let service: BeneficiarioService;
  let messageService: NzMessageService;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [BeneficiarioList, NoopAnimationsModule],
      providers: [
        provideRouter([]),
        provideHttpClient(),
        provideHttpClientTesting(),
        { provide: BeneficiarioService, useClass: MockBeneficiarioService },
        { provide: NzMessageService, useClass: MockNzMessageService }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(BeneficiarioList);
    component = fixture.componentInstance;
    service = TestBed.inject(BeneficiarioService);
    messageService = TestBed.inject(NzMessageService);

    fixture.detectChanges();
  });

  it('deve carregar a lista ao iniciar', () => {
    expect(component.beneficiarios.length).toBe(2);
    expect(component.beneficiarios[0].nomeCompleto).toBe('Maria');
  });

  it('deve abrir o modal de exclusão corretamente', () => {
    component.abrirModalExclusao(1);

    expect(component.isVisible).toBeTrue();
    expect(component.idParaExcluir).toBe(1);
    expect(component.prioridadeSelecionada).toBe(3);
  });

  it('deve confirmar a exclusão, chamar o service com prioridade e remover da lista local', () => {
    const spyDelete = spyOn(service, 'delete').and.callThrough();
    const spyMessage = spyOn(messageService, 'success');

    component.abrirModalExclusao(1);
    component.confirmarExclusao();


    expect(spyDelete).toHaveBeenCalledWith(1, 3);
    expect(component.beneficiarios.length).toBe(1);
    expect(component.beneficiarios[0].id).toBe(2);

    expect(spyMessage).toHaveBeenCalled();
  });
});
