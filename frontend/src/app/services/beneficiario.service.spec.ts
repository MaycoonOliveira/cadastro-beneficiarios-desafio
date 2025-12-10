import { TestBed } from '@angular/core/testing';
import { BeneficiarioService } from './beneficiario.service';
import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { BeneficiarioCriacao } from '../models/beneficiario.model';
import { environment } from '../../environments/environment';

describe('BeneficiarioService', () => {
  let service: BeneficiarioService;
  let httpMock: HttpTestingController;

  const API_URL = `${environment.api}/Beneficiario`;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        BeneficiarioService,
        provideHttpClient(),
        provideHttpClientTesting()
      ]
    });
    service = TestBed.inject(BeneficiarioService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('deve enviar um POST ao criar um beneficiário', () => {
    const novoBeneficiario: BeneficiarioCriacao = {
      nomeCompleto: 'João Teste',
      cpf: '12345678900',
      dataNascimento: '1990-01-01',
      email: 'joao@teste.com',
      planoId: 1
    };

    service.create(novoBeneficiario).subscribe(res => {
      expect(res).toBeTruthy();
    });

    const req = httpMock.expectOne(API_URL);
    expect(req.request.method).toBe('POST');
    expect(req.request.body).toEqual(novoBeneficiario);

    req.flush({ mensagem: 'Sucesso' });
  });

  it('deve buscar lista de beneficiários (GET)', () => {
    service.getAll().subscribe();

    const req = httpMock.expectOne(API_URL);
    expect(req.request.method).toBe('GET');
    req.flush({ dados: [] });
  });
});
