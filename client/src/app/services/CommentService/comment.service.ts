import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { CommentModel } from '../../models/CommentModel';
import { HttpHeaders } from '@angular/common/http';
@Injectable({
  providedIn: 'root'
})

export class CommentService {

  constructor(private http: HttpClient) { }

  private getAuthHeaders(): HttpHeaders {
    const token = localStorage.getItem('token');
    return new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
  }
  getComments(postId: string): Observable<CommentModel>{
    return this.http.get<CommentModel>(`${environment.apiUrl}/comment/${postId}`, { headers: this.getAuthHeaders() });
  }

  createComment(authorId: string, content: string, postId: string): Observable<CommentModel>{
    return this.http.post<CommentModel>(`${environment.apiUrl}/comment`, {
      AuthorId: authorId,
      Content: content,
      PostId: postId
    }, { headers: this.getAuthHeaders() });
  }
}
