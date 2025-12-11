import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { Beneficiario, BeneficiarioCriacao } from '../models/beneficiario.model';
import { environment } from '../../environments/environment';
import { ApiService } from './api.service';

@Injectable({
  providedIn: 'root'
})
export class BeneficiarioService {
  private beneficiario = `Beneficiario`;
  private apiService = inject(ApiService);

  getAll(): Observable<Beneficiario[]> {
    return this.apiService.get<any>(this.beneficiario).pipe(
      map(response => response.dados)
    );
  }

  getById(id: number): Observable<Beneficiario> {
    return this.apiService.get<any>(`${this.beneficiario}/${id}`).pipe(
      map(response => response.dados)
    );
  }

  create(beneficiario: BeneficiarioCriacao): Observable<any> {
    return this.apiService.post<any>(this.beneficiario, beneficiario);
  }

  update(id: number, beneficiario: BeneficiarioCriacao): Observable<any> {
    return this.apiService.put<any>(`${this.beneficiario}/${id}`, beneficiario);
  }

  delete(id: number, prioridade: number = 3): Observable<any> {
    return this.apiService.delete<any>(`${this.beneficiario}/${id}?prioridade=${prioridade}`);
  }
}
