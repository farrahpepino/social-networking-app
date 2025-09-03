import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpParams } from '@angular/common/http';
import { HttpHeaders } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { InterestResponse } from '../../models/interestresponse';

@Injectable({
  providedIn: 'root'
})
export class InterestsService {

  private getAuthHeaders(): HttpHeaders {
    const token = localStorage.getItem('5a6f9c4c-1b88-4d9f-b62f-9fcb9e91db26');
    return new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
  }
  constructor(private http: HttpClient) { }

  follow(userId1: string, userId2: string) {
    return this.http.post(
      `${environment.apiUrl}/interest`,
      { UserId1: userId1, UserId2: userId2 },  
      { headers: this.getAuthHeaders(), responseType: 'json' }  
    );
  }
  
  
  //fix later
  getInterests(userId: string): Observable<InterestResponse[]> {
    const params = new HttpParams().set('userId', userId);
    const options = {
      headers: this.getAuthHeaders(),
      params: params,
      responseType: 'json' as const 
    };
    return this.http.get<InterestResponse[]>(`${environment.apiUrl}/interest`, options);
  }

  deleteInterest(userId1: string, userId2: string) {
    return this.http.delete(`${environment.apiUrl}/interest`, {
      headers: this.getAuthHeaders(),
      body: { UserId1: userId1, UserId2: userId2 }, 
      responseType: 'json'
    });
  }
  
}
