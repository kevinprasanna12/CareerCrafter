import { Injectable } from '@angular/core';
import { ApiService } from '../core/api.service';
import { Observable, BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private isLoggedInSubject = new BehaviorSubject<boolean>(this.hasToken());
  loggedIn$ = this.isLoggedInSubject.asObservable();

  constructor(private api: ApiService) {}

  login(credentials: any): Observable<any> {
    return this.api.post<any>('v1/auth/login', credentials);
  }

  signup(userData: any): Observable<any> {
    return this.api.post<any>('v1/auth/register', userData);
  }

  saveToken(token: string): void {
    sessionStorage.setItem('token', token);
    this.isLoggedInSubject.next(true);
  }

  getToken(): string | null {
    return sessionStorage.getItem('token');
  }

  logout(): void {
    sessionStorage.removeItem('token');
    this.isLoggedInSubject.next(false);
  }

  private hasToken(): boolean {
    const token = sessionStorage.getItem('token');
    if (!token) return false;
    try {
        const payload = JSON.parse(atob(token.split('.')[1]));
        const expiry = payload.exp;
        const now = Math.floor(Date.now() / 1000);
        if (expiry && expiry > now) {
            return true; // valid and not expired
        } else {
            // expired token
            sessionStorage.removeItem('token');
            return false;
        }
    } catch (e) {
        console.error('Invalid token format:', e);
        sessionStorage.removeItem('token');
        return false;
    }
  }

  getTokenPayload() {
    const token = this.getToken();
    if (!token) return {};
    const payload = token.split('.')[1];
    return JSON.parse(atob(payload));
  }

  get isLoggedInSync(): boolean {
    return this.isLoggedInSubject.value;
  }

  getRole(): string | null {
    const payload = this.getTokenPayload();
    return (
      payload?.role ??
      payload?.Role ??
      payload?.["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"] ??
      null
    );
  }

  isEmployer(): boolean {
    return this.getRole() === 'Employer';
  }

  isJobSeeker(): boolean {
    return this.getRole() === 'JobSeeker';
  }
}
