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
  private plano = `Plano`;
  private apiService = inject(ApiService);

  getAll(): Observable<Plano[]> {
    return this.apiService.get<any>(this.plano).pipe(
      map(response => response.dados)
    );
  }

  getById(id: number): Observable<Plano> {
    return this.apiService.get<any>(`${this.plano}/${id}`).pipe(
      map(response => response.dados)
    );
  }

  create(plano: PlanoCriacao): Observable<any> {
    return this.apiService.post<any>(this.plano, plano);
  }

  update(id: number, plano: PlanoCriacao): Observable<any> {
    return this.apiService.put<any>(`${this.plano}/${id}`, { id, ...plano });
  }

  delete(id: number): Observable<any> {
    return this.apiService.delete<any>(`${this.plano}/${id}`);
  }
}
