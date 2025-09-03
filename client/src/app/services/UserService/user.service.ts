import { Injectable } from '@angular/core';
import { JwtService } from '../JwtService/jwt.service';
import { User } from '../../models/user';
import { BehaviorSubject, Observable } from 'rxjs';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { UserSearchResponse } from '../../models/usersearchresponse';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private loggedInUserSubject = new BehaviorSubject<User | null>(null);
  loggedInUser$ = this.loggedInUserSubject.asObservable();

  constructor(private jwtService: JwtService, private http: HttpClient) {
    if (typeof window !== 'undefined') {
      const storedUser = localStorage.getItem('loggedInUser');
      if (storedUser) {
        this.loggedInUserSubject.next(JSON.parse(storedUser));
      }
    }
  }

   private getAuthHeaders(): HttpHeaders {
    const token = localStorage.getItem('token');
    return new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
  }

  initializeUser(token: string) {
    const decoded = this.jwtService.getDecodedAccessToken(token);
    const user: User = {
      id: decoded.sub,
      username: decoded.unique_name,
      email: decoded.email
    };
    this.setLoggedInUser(user);
  }

  setLoggedInUser(user: User) {
    this.loggedInUserSubject.next(user);
    localStorage.setItem('loggedInUser', JSON.stringify(user));
  }

  getLoggedInUser(): User | null {
    return this.loggedInUserSubject.value;
  }

  searchUser(query: string): Observable<UserSearchResponse[]> {
    return this.http.get<UserSearchResponse[]>(`${environment.apiUrl}/user/search`, {params: { query }});
  }

  getUserInformation(userId: string): Observable<User> {
    return this.http.get<User>(`${environment.apiUrl}/user`, {params: { userId }});
  }
  
}
