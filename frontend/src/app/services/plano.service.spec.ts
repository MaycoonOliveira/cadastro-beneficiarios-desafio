import { TestBed } from '@angular/core/testing';
import { PlanoService } from './plano.service';
import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { PlanoCriacao } from '../models/plano.model';
import { environment } from '../../environments/environment';

describe('PlanoService', () => {
  let service: PlanoService;
  let httpMock: HttpTestingController;

  const API_URL = `${environment.api}/Plano`;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        PlanoService,
        provideHttpClient(),
        provideHttpClientTesting()
      ]
    });
    service = TestBed.inject(PlanoService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('deve ser criado', () => {
    expect(service).toBeTruthy();
  });

  it('deve enviar um POST ao criar um plano', () => {
    const novoPlano: PlanoCriacao = { nome: 'Plano Teste', codigo_registro_ans: '123' };

    service.create(novoPlano).subscribe(resposta => {
      expect(resposta).toBeTruthy();
    });

    const req = httpMock.expectOne(API_URL);

    expect(req.request.method).toBe('POST');
    expect(req.request.body).toEqual(novoPlano);

    req.flush({ dados: { id: 1, ...novoPlano }, mensagem: 'Sucesso' });
  });

  it('deve enviar um DELETE ao excluir um plano', () => {
    const idParaDeletar = 5;

    service.delete(idParaDeletar).subscribe();

    const req = httpMock.expectOne(`${API_URL}/${idParaDeletar}`);
    expect(req.request.method).toBe('DELETE');

    req.flush({ mensagem: 'Removido' });
  });
});
