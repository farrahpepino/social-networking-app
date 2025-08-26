import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { PostDto } from '../../dtos/PostDto';
import { environment } from '../../../environments/environment';
import { LikeDto } from '../../dtos/LikeDto';
@Injectable({
  providedIn: 'root'
})
export class PostService {

  constructor(private http: HttpClient) {}
  
  private getAuthHeaders(): HttpHeaders {
    const token = localStorage.getItem('token');
    return new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
  }

  createPost(authorId: string, content: string): Observable<PostDto> {
    return this.http.post<PostDto>(`${environment.apiUrl}/post`, {
      AuthorId: authorId, 
      Content: content    
    },
    { headers: this.getAuthHeaders() }
    );
  }
  
  deletePost(id: string): Observable<void> {
    return this.http.delete<void>(`${environment.apiUrl}/post/${id}`, { headers: this.getAuthHeaders() }
    );
  }

  getPost(id: string): Observable<PostDto>{
    return this.http.get<PostDto>(`${environment.apiUrl}/post/${id}`, { headers: this.getAuthHeaders() }
    );
  }

  getPosts(): Observable<PostDto[]>{
    return this.http.get<PostDto[]>(`${environment.apiUrl}/post`, { headers: this.getAuthHeaders() }
    );
  }

  likePost(postId: string, likerId: string): Observable<LikeDto>{
    return this.http.post<LikeDto>(`${environment.apiUrl}/post/like-post`,
    {
      PostId: postId,
      LikerId: likerId
    },
    { headers: this.getAuthHeaders() });

  }

  unlikePost(PostId: string, LikerId: string): Observable<void> {
    return this.http.delete<void>(`${environment.apiUrl}/post/${PostId}/unlike-post/${LikerId}`, {
      headers: this.getAuthHeaders()});

  }
  
  getLikes(postId: string): Observable<LikeDto[]>{
    return this.http.get<LikeDto[]>(`${environment.apiUrl}/post/${postId}/likes`,  { headers: this.getAuthHeaders() });
  }

  getPostsByUserId(userId: string): Observable<PostDto[]>{
    return this.http.get<PostDto[]>(`${environment.apiUrl}/post/${userId}/posts`,  { headers: this.getAuthHeaders() });
  }

  isLiked(postId: string, likerId: string): Observable<boolean>{
    return this.http.get<boolean>(`${environment.apiUrl}/post/${postId}/liked-by/${likerId}`,  { headers: this.getAuthHeaders() });
  }
  
}