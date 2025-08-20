import { Injectable } from '@angular/core';
import { JwtService } from '../JwtService/jwt.service';
import { UserModel } from '../../models/UserModel';
@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private jwtService: JwtService) { }

  getCurrentUser(token: string): UserModel{
    const decoded = this.jwtService.getDecodedAccessToken(token);
    return ({
      id: decoded.sub,
      username: decoded.unique_name,
      email: decoded.email
    })
  }
}
