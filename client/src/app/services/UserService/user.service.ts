import { Injectable } from '@angular/core';
import { JwtService } from '../JwtService/jwt.service';
import { User } from '../../models/User';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private loggedInUserSubject = new BehaviorSubject<User | null>(null);
  loggedInUser$ = this.loggedInUserSubject.asObservable();

  constructor(private jwtService: JwtService) {
    if (typeof window !== 'undefined') {
      const storedUser = localStorage.getItem('loggedInUser');
      if (storedUser) {
        this.loggedInUserSubject.next(JSON.parse(storedUser));
      }
    }
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
}
