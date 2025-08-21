import { Injectable } from '@angular/core';
import { JwtService } from '../JwtService/jwt.service';
import { UserModel } from '../../models/UserModel';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private loggedInUserSubject = new BehaviorSubject<UserModel | null>(
  JSON.parse(localStorage.getItem('loggedInUser') || 'null')
  );

  loggedInUser$ = this.loggedInUserSubject.asObservable();
  constructor(private jwtService: JwtService) { }

  initializeUser(token: string) {
    const decoded = this.jwtService.getDecodedAccessToken(token);
    const user: UserModel = {
      id: decoded.sub,
      username: decoded.unique_name,
      email: decoded.email
    };
    this.setLoggedInUser(user);
  }

  setLoggedInUser(user: UserModel) {
    this.loggedInUserSubject.next(user);
    localStorage.setItem('loggedInUser', JSON.stringify(user));
  }

  getLoggedInUser(): UserModel | null {
    return this.loggedInUserSubject.value;
  }
}
