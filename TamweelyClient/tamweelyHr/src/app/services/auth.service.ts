import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { tap } from 'rxjs/operators';
import { LoginDto, LoginResponseDto, AuthState } from '../models/auth.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = 'https://localhost:7097/api';
  private authStateSubject = new BehaviorSubject<AuthState>({
    token: this.getToken(),
    user: this.getUser(),
    isAuthenticated: !!this.getToken(),
    roles: this.getRoles()
  });

  public authState$ = this.authStateSubject.asObservable();

  constructor(private http: HttpClient) {}

  login(credentials: LoginDto): Observable<LoginResponseDto> {
    return this.http.post<LoginResponseDto>(`${this.apiUrl}/Auth/login`, credentials).pipe(
      tap(response => {
        localStorage.setItem('token', response.token);
        localStorage.setItem('user', JSON.stringify({
          userName: response.userName,
          fullName: response.fullName,
          roles: response.roles
        }));
        this.updateAuthState();
      })
    );
  }

  logout(): void {
    localStorage.removeItem('token');
    localStorage.removeItem('user');
    this.updateAuthState();
  }

  getToken(): string | null {
    return localStorage.getItem('token');
  }

  getUser(): LoginResponseDto | null {
    const userStr = localStorage.getItem('user');
    return userStr ? JSON.parse(userStr) : null;
  }

  getRoles(): string[] {
    const user = this.getUser();
    return user?.roles || [];
  }

  isAdmin(): boolean {
    return this.getRoles().includes('Admin');
  }

  isAuthenticated(): boolean {
    return !!this.getToken();
  }

  private updateAuthState(): void {
    this.authStateSubject.next({
      token: this.getToken(),
      user: this.getUser(),
      isAuthenticated: !!this.getToken(),
      roles: this.getRoles()
    });
  }
}
