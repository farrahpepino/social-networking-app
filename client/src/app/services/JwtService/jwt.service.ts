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

  getUserId(token: string){
    const decoded = this.getDecodedAccessToken(token);
    console.log("delete this from jwt service", decoded);
    return decoded.sub;
  }

  getUserData<UserModel>(token: string){
    const decoded = this.getDecodedAccessToken(token);
    return ({
      id: decoded.sub,
      username: decoded.unique_name,
      email: decoded.email
    })
  }
}
