import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ApiService {

  constructor(private http: HttpClient) {
  }

  public post<T>(url: string, data: any, headers?: HttpHeaders, params?: HttpParams): Observable<T> {
    return this.http.post<T>(`${environment.api}/${url}`, data, { headers, params })
  }

  public postCustomApi<T>(url: string, data: any, headers?: HttpHeaders, params?: HttpParams) {
    return this.http.post<T>(url, data, { headers, params })
  }

  public get<T>(url: string, params?: HttpParams) {
    if (params)
      return this.http.get<T>(`${environment.api}/${url}`, { params: params });
    else
      return this.http.get<T>(`${environment.api}/${url}`);
  }

  public getCustomApi<T>(url: string, params?: HttpParams) {
    if (params)
      return this.http.get<T>(url, { params: params });
    else
      return this.http.get<T>(url);
  }

  public put<T>(url: string, data?: any) {
    return this.http.put<T>(`${environment.api}/${url}`, data)
  }

  public putCustomApi<T>(url: string, data?: any) {
    return this.http.put<T>(url, data)
  }

  public delete<T>(url: string, data?: any) {
    return this.http.request('DELETE', `${environment.api}/${url}`, {
      body: data
    });
  }

  public deleteCustomApi<T>(url: string, data?: any) {
    return this.http.request('DELETE', url, {
      body: data
    });
  }

  public download(url: string, data?: any) {
    return this.http.post(`${environment.api}/${url}`, data, { responseType: "blob", observe: 'response' })
  }

  public downloadPUT(url: string, data?: any) {
    return this.http.put(`${environment.api}/${url}`, data, { responseType: "blob", observe: 'response' })
  }

  public downloadCustomApi(url: string, data?: any) {
    return this.http.post(url, data, { responseType: "blob", observe: 'response' })
  }

  public downloadGet(url: string) {
    return this.http.get(`${environment.api}/${url}`, { responseType: "blob", observe: 'response' })
  }

  public downloadGetCustomApi(url: string) {
    return this.http.get(url, { responseType: "blob", observe: 'response' })
  }

  public convertObjectToParams(obj: any): HttpParams {
    let params = new HttpParams()
    if (obj === null || obj === undefined)
      return params;

    Object.keys(obj).filter(x => obj[x] != undefined && obj[x] != null && obj[x] != 'null' && obj[x] != '').forEach((x) => {
      params = params.set(x.toString(), obj[x]);
    })

    return params;
  }

  public removeNullPropertys<T>(obj: any): T {
    Object.keys(obj).filter(x => obj[x] == undefined || obj[x] == null || obj[x] == 'null' || obj[x] == '').forEach((x) => {
      delete obj[x];
    })
    return <T>obj;
  }

  private removeEmpty<T>(obj: any): T {
    if (!obj)
      return obj;

    return <T>Object.entries(obj)
      .filter(([_, v]) => v != null && v != undefined && (<any>v)?.toString().trim() != '')
      .reduce(
        (acc, [k, v]) => ({ ...acc, [k]: v === Object(v) ? this.removeEmpty(v) : v }),
        {}
      );
  }
}
