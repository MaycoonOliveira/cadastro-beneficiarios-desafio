import { ComponentFixture, TestBed } from '@angular/core/testing';
import { PlanoList } from './plano-list';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { provideRouter } from '@angular/router';
import { NzMessageService } from 'ng-zorro-antd/message';
import { of, throwError } from 'rxjs';
import { PlanoService } from '../../../services/plano.service';

const listaPlanosMock = [
  { id: 1, nome: 'Plano A', codigo_registro_ans: '111' },
  { id: 2, nome: 'Plano B', codigo_registro_ans: '222' }
];

class MockPlanoService {
  getAll() { return of(listaPlanosMock); }
  delete(id: number) { return of({ success: true }); }
}

class MockNzMessageService {
  success(msg: string) {}
  error(msg: string) {}
}

describe('PlanoList', () => {
  let component: PlanoList;
  let fixture: ComponentFixture<PlanoList>;
  let planoService: PlanoService;
  let messageService: NzMessageService;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PlanoList, NoopAnimationsModule],
      providers: [
        provideRouter([]),
        provideHttpClient(),
        provideHttpClientTesting(),
        { provide: PlanoService, useClass: MockPlanoService },
        { provide: NzMessageService, useClass: MockNzMessageService }
      ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PlanoList);
    component = fixture.componentInstance;
    planoService = TestBed.inject(PlanoService);
    messageService = TestBed.inject(NzMessageService);

    fixture.detectChanges();
  });

  it('deve ser criado', () => {
    expect(component).toBeTruthy();
  });

  it('deve carregar a lista de planos ao iniciar', () => {
    expect(component.planos.length).toBe(2);
    expect(component.planos[0].nome).toBe('Plano A');
    expect(component.loading).toBeFalse();
  });

  it('deve abrir o modal ao clicar em excluir', () => {
    component.abrirModalExclusao(1);
    expect(component.isVisible).toBeTrue();
    expect(component.idParaExcluir).toBe(1);
  });

  it('deve excluir e atualizar a lista localmente ao confirmar', () => {
    const spyDelete = spyOn(planoService, 'delete').and.callThrough();
    const spyMessage = spyOn(messageService, 'success');

    component.abrirModalExclusao(1);
    component.confirmarExclusao();

    expect(spyDelete).toHaveBeenCalledWith(1);
    expect(spyMessage).toHaveBeenCalled();

    expect(component.planos.length).toBe(1);
    expect(component.planos[0].id).toBe(2);
    expect(component.isVisible).toBeFalse();
  });

  it('deve exibir mensagem de erro se a exclusão falhar', () => {
    spyOn(planoService, 'delete').and.returnValue(throwError(() => ({ error: { mensagem: 'Erro API' } })));
    const spyMessage = spyOn(messageService, 'error');

    component.abrirModalExclusao(99);
    component.confirmarExclusao();

    expect(spyMessage).toHaveBeenCalled();
  });
});
