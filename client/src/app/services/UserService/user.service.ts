import { Injectable } from '@angular/core';
import { JwtService } from '../JwtService/jwt.service';
import { UserDto } from '../../dtos/UserDto';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private loggedInUserSubject = new BehaviorSubject<UserDto | null>(null);



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
    const user: UserDto = {
      id: decoded.sub,
      username: decoded.unique_name,
      email: decoded.email
    };
    this.setLoggedInUser(user);
  }

  setLoggedInUser(user: UserDto) {
    this.loggedInUserSubject.next(user);
    localStorage.setItem('loggedInUser', JSON.stringify(user));
  }

  getLoggedInUser(): UserDto | null {
    return this.loggedInUserSubject.value;
  }
}
