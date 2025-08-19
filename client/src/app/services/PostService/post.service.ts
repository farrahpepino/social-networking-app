import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { PostModel } from '../../models/PostModel';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class PostService {

  constructor(private http: HttpClient) {}

  createPost(authorId: string, content: string): Observable<PostModel> {
    return this.http.post<PostModel>(`${environment.apiUrl}/post`, {
      AuthorId: authorId, 
      Content: content    
    });
  }
  
  
  deletePost(id: string): Observable<void> {
    return this.http.delete<void>(`${environment.apiUrl}/post/${id}`);
  }

  getPost(id: string): Observable<PostModel>{
    return this.http.get<PostModel>(`${environment.apiUrl}/post/${id}`);
  }
  
  
}
