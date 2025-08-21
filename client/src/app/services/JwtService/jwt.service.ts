import { Injectable } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';

@Injectable({
  providedIn: 'root',
})
export class JwtService {
  constructor() {}
  getDecodedAccessToken(token: string): any {
    const helper = new JwtHelperService();
    const decodedToken = helper.decodeToken(token);
    return decodedToken;
  }
}
