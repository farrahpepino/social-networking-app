import { Injectable } from '@angular/core';
import { Observable, tap } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { AuthResponse } from '../../models/authresponse';
import { Router } from '@angular/router';
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
        localStorage.setItem('5a6f9c4c-1b88-4d9f-b62f-9fcb9e91db26', response.token);
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
        localStorage.setItem('5a6f9c4c-1b88-4d9f-b62f-9fcb9e91db26', response.token);
        this.router.navigate(['home']);
      })
    );
  }

  logout() {
    localStorage.removeItem('5a6f9c4c-1b88-4d9f-b62f-9fcb9e91db26');
    this.router.navigate([''],{ replaceUrl: true });
  }

  isLoggedIn(): boolean {
    if (typeof window === 'undefined') return false;
    return !!localStorage.getItem('5a6f9c4c-1b88-4d9f-b62f-9fcb9e91db26'); 
  }

}
