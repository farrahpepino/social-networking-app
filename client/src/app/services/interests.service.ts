import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class InterestsService {

  constructor(private http: HttpClient) { }

  follow(userId1: string, userId2: string){
    return this.http.post(`${environment.apiUrl}/interest`, {UserId1: userId1, UserId2: userId2});
  }
}
