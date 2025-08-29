import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Post } from '../../models/Post';
import { environment } from '../../../environments/environment';
import { Like } from '../../models/Like';
import { S3Response } from '../../models/S3Response';
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

  createPost(authorId: string, content: string, imageUrl?: string | null): Observable<Post> {
    return this.http.post<Post>(`${environment.apiUrl}/post`, {
      AuthorId: authorId, 
      Content: content,
      ImageUrl: imageUrl
    },
    { headers: this.getAuthHeaders() }
    );
  }
  
  uploadImage(formData: FormData): Observable<S3Response> {
    return this.http.post<S3Response>(`${environment.apiUrl}/image`, formData);
  }
  
  deletePost(id: string): Observable<void> {
    return this.http.delete<void>(`${environment.apiUrl}/post/${id}`, { headers: this.getAuthHeaders() }
    );
  }

  getPost(id: string): Observable<Post>{
    return this.http.get<Post>(`${environment.apiUrl}/post/${id}`, { headers: this.getAuthHeaders() }
    );
  }

  getPosts(): Observable<Post[]>{
    return this.http.get<Post[]>(`${environment.apiUrl}/post`, { headers: this.getAuthHeaders() }
    );
  }

  likePost(postId: string, likerId: string): Observable<Like>{
    return this.http.post<Like>(`${environment.apiUrl}/post/like-post`,
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
  
  getLikes(postId: string): Observable<Like[]>{
    return this.http.get<Like[]>(`${environment.apiUrl}/post/${postId}/likes`,  { headers: this.getAuthHeaders() });
  }

  getPostsByUserId(userId: string): Observable<Post[]>{
    return this.http.get<Post[]>(`${environment.apiUrl}/post/${userId}/posts`,  { headers: this.getAuthHeaders() });
  }

  isLiked(postId: string, likerId: string): Observable<boolean>{
    return this.http.get<boolean>(`${environment.apiUrl}/post/${postId}/liked-by/${likerId}`,  { headers: this.getAuthHeaders() });
  }
}