import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

export interface AuthResponse { message: string; token: string; user: { idUser: number; userName: string; role?: string }; }
export interface AuthenticatedResponse { message: string; status: string; authenticatedUser?: { idUser: string; userName: string; role?: string }; }


@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly apiBaseUrl = 'http://localhost:5001';
  constructor(private readonly http: HttpClient) {}

  login(userName: string, password: string): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.apiBaseUrl}/api/auth/login`, { userName, password: this.encodePassword(password) });
  }

  register(userName: string, password: string, confirmPassword: string): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.apiBaseUrl}/api/auth/register`, {
      userName, password: this.encodePassword(password), confirmPassword: this.encodePassword(confirmPassword)
    });
  }

  checkAuth(): Observable<AuthenticatedResponse> {
    return this.http.get<AuthenticatedResponse>(`${this.apiBaseUrl}/auth`, { headers: new HttpHeaders({ Authorization: `Bearer ${this.token}` }) });
  }

  saveSession(response: AuthResponse): void {
    localStorage.setItem('jwt_token', response.token);
    localStorage.setItem('user_info', JSON.stringify(response.user));
  }

  logout(): void { localStorage.removeItem('jwt_token'); localStorage.removeItem('user_info'); }
  get isAuthenticated(): boolean { return Boolean(this.token); }
  private get token(): string { return localStorage.getItem('jwt_token') ?? ''; }
  private encodePassword(password: string): string { return btoa(unescape(encodeURIComponent(password))); }
}
