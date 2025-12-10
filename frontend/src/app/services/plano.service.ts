import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { Plano, PlanoCriacao } from '../models/plano.model';
import { environment } from '../../environments/environment';
import { ApiService } from './api.service';

@Injectable({
  providedIn: 'root'
})
export class PlanoService {
  private apiUrl = `${environment.api}/Plano`;
  private apiService = inject(ApiService);

  getAll(): Observable<Plano[]> {
    return this.apiService.get<any>(this.apiUrl).pipe(
      map(response => response.dados)
    );
  }

  getById(id: number): Observable<Plano> {
    return this.apiService.get<any>(`${this.apiUrl}/${id}`).pipe(
      map(response => response.dados)
    );
  }

  create(plano: PlanoCriacao): Observable<any> {
    return this.apiService.post<any>(this.apiUrl, plano);
  }

  update(id: number, plano: PlanoCriacao): Observable<any> {
    return this.apiService.put<any>(`${this.apiUrl}/${id}`, { id, ...plano });
  }

  delete(id: number): Observable<any> {
    return this.apiService.delete<any>(`${this.apiUrl}/${id}`);
  }
}
