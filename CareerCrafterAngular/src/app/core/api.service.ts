// src/app/core/api.service.ts

import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { Observable } from 'rxjs';
import { throwError } from 'rxjs';

@Injectable({
  providedIn: 'root'
})

export class ApiService {
  private baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  get<T>(endpoint: string): Observable<T> {
    return this.http.get<T>(`${this.baseUrl}/${endpoint}`);
  }

  getWithOptions(endpoint: string, options: any): Observable<any> {
    const url = endpoint.startsWith('http')
        ? endpoint
        : `${this.baseUrl}/${endpoint}`;
    return this.http.get(url, options);
    }


  post<T>(endpoint: string, data: any): Observable<T> {
    return this.http.post<T>(`${this.baseUrl}/${endpoint}`, data);
  }

  put<T>(endpoint: string, data: any): Observable<T> {
    return this.http.put<T>(`${this.baseUrl}/${endpoint}`, data);
  }

  delete<T>(endpoint: string): Observable<T> {
    return this.http.delete<T>(`${this.baseUrl}/${endpoint}`);
  }

  //ensure employee or jobseeker complete their profile 
  checkProfileCompletion(userId: number, role: string) {
    if (role === 'Employer') {
      return this.http.get(`${this.baseUrl}/api/v1/employer/${userId}`);
    } else if (role === 'JobSeeker') {
      return this.http.get(`${this.baseUrl}/api/v1/jobseeker/${userId}`);
    } else {
      return throwError(() => new Error('Unknown role'));
    }
  }
}
