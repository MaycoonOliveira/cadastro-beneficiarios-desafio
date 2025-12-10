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
  private apiService = inject(ApiService);

  private apiUrl = `${environment.api}/Beneficiario`;

  getAll(): Observable<Beneficiario[]> {
    return this.apiService.get<any>(this.apiUrl).pipe(
      map(response => response.dados)
    );
  }

  getById(id: number): Observable<Beneficiario> {
    return this.apiService.get<any>(`${this.apiUrl}/${id}`).pipe(
      map(response => response.dados)
    );
  }

  create(beneficiario: BeneficiarioCriacao): Observable<any> {
    return this.apiService.post<any>(this.apiUrl, beneficiario);
  }

  update(id: number, beneficiario: BeneficiarioCriacao): Observable<any> {
    return this.apiService.put<any>(`${this.apiUrl}/${id}`, beneficiario);
  }

  delete(id: number): Observable<any> {
    return this.apiService.delete<any>(`${this.apiUrl}/${id}`);
  }
}
