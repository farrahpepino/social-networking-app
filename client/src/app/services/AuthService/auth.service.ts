import { Injectable } from '@angular/core';
import { Observable, tap } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { AuthResponse } from '../../models/AuthResponse';
import { Router } from '@angular/router';
import { response } from 'express';
@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private http: HttpClient, private router:Router) { }

  registerUser(username: string, password: string, email: string): Observable<AuthResponse>{
    return this.http.post<AuthResponse>(`${environment.apiUrl}/auth/register`, {
      Username: username,
      HashedPassword: password,
      Email: email
    })
    .pipe(
      tap(response => {
        localStorage.setItem('token', response.token);
        this.router.navigate(['home']);
      })
    );
  }

  loginUser(email: string, password: string): Observable<AuthResponse>{
    return this.http.post<AuthResponse>(`${environment.apiUrl}/auth/login`,{
      Email: email,
      Password: password
    })
    .pipe(
      tap(response=>{
        localStorage.setItem('token', response.token);
        this.router.navigate(['home']);
      })
    );
  }

  logout() {
    localStorage.removeItem('token');
    this.router.navigate([''],{ replaceUrl: true });
  }

  isLoggedIn(): boolean {
    if (typeof window === 'undefined') return false;
    return !!localStorage.getItem('token'); 
  }

}
