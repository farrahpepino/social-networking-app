import { Component } from '@angular/core';
import { NavigationComponent } from '../../navigation/navigation.component';
import { PostService } from '../../../services/PostService/post.service';
import { ViewChild, ElementRef, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { UserService } from '../../../services/UserService/user.service';
import { CommentService } from '../../../services/CommentService/comment.service';
import { Post } from '../../../models/post';
import { CommonModule } from '@angular/common';
import { User } from '../../../models/user';
import { Comment } from '../../../models/comment';
import { Like } from '../../../models/like';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-home',
  standalone: true, 
  imports: [NavigationComponent, CommonModule],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css',
})

export class HomeComponent implements OnInit {

  constructor(private userService: UserService, private postService: PostService, private commentService: CommentService, private router: Router, private http: HttpClient) {}
  
  @ViewChild('postInput') postInput!: ElementRef<HTMLElement>;
  @ViewChild('commentInput') commentInput!: ElementRef<HTMLElement>;

  loggedInUser: User | null = null;
  posts: Post[] = [];
  post: Post | null = null;
  comments: Comment[] = [];
  likes: Like[] = []
  commentCount: number = 0;
  likedPosts: { [postId: string]: boolean } = {};
  showPost = false;
  showForm = false;
  isClicked = false;
  selectedFile: File | null = null;
  previewUrl: string | null = null;
  s3Url: string | null = null;

  ngOnInit(): void {
    this.userService.loggedInUser$.subscribe(user => {
      this.loggedInUser = user;
      this.loadPosts();
    });
  }

  onFileSelected(event: Event) {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      this.selectedFile = input.files[0];
      const reader = new FileReader();
      reader.onload = () => {
        this.previewUrl = reader.result as string;
      };
      reader.readAsDataURL(this.selectedFile);
    }
  }

  unselectFile() {
    this.selectedFile = null;
    this.previewUrl = null;
  }

  loadPosts() {
    this.postService.getPosts().subscribe({
      next: (data) => {
        this.posts = data;
        this.posts.forEach(post => {
          this.postService.getLikes(post.id).subscribe({
            next:(data)=>{
              post.likes = data;
            },
            error: (err) => {
             console.error("Error fetching likes:", err)
            }
          });
          this.postService.isLiked(post.id, this.loggedInUser!.id).subscribe({
            next: (data) => {
              this.likedPosts[post.id] = data;
            },
            error: (err) => console.error('Error checking post...', err)
          });
          this.commentService.getComments(post.id).subscribe({
            error: (err) => {
             console.error("Error fetching comments:", err)
            }
          });
        });
      },
      error: (err) => console.error("Error fetching posts:", err)
    });
  }

  deletePost(id: string){
    this.postService.deletePost(id).subscribe({
      next: () => this.loadPosts(), 
      error: (err) => console.error("Error deleting post:", err)
    });
  }

  onClick() {
    this.isClicked = true;
  }

  isLiked(postId: string){
    return !!this.likedPosts[postId];
  }

  toggleLike(post: Post) {
    const postId = post.id;
    const userId = this.loggedInUser!.id;
  
    if (this.likedPosts[postId]) {
      this.postService.unlikePost(postId, userId).subscribe({
        next: () => {
          this.likedPosts[postId] = false;
          post.likes = post.likes?.filter(like => like.likerId !== userId) || [];
          this.loadPosts();
        },
        error: (err) => console.error('Error unliking post', err)
      });
    } else {
      this.postService.likePost(postId, userId).subscribe({
        next: (like) => {
          this.likedPosts[postId] = true;
          post.likes = [...(post.likes || []), {
            ...like,
            username: this.loggedInUser!.username
          }];
          this.loadPosts();
        },
        error: (err) => console.error('Error liking post', err)
      });
    }
  }
  
  viewPost(id: string) {
    this.showPost = true; 
    this.postService.getPost(id).subscribe({
      next: (data) => {
        this.post = data;
        this.postService.getLikes(id).subscribe({
          next:(data)=>{
            this.post!.likes = data;
          },
          error: (err) => {
           console.error("Error fetching likes:", err)
          }
        });
        this.commentService.getComments(data.id).subscribe({
          next: (comments) => {
            this.comments = Array.isArray(comments) ? comments : [comments];
            this.commentCount = this.comments.length;
          }
        });
      },
      error: (err) => {
        console.error('Error fetching post: ', err);
      }
    });
    }

  hidePost() { 
    this.post = null;
    this.showPost = false; 
    this.loadPosts();
  }

  viewForm() { this.showForm = true; }
  hideForm() { this.showForm = false; }

  submitPost() {
    const authorId = this.loggedInUser!.id;
    const content = this.postInput.nativeElement.innerText.trim() || '';
    
    if (this.selectedFile!=null) {
      const formData = new FormData();
      formData.append("File", this.selectedFile);
      formData.append("UserId",  this.loggedInUser!.id);

      this.postService.uploadImage(formData)
    .subscribe(
        res => { 
         this.postService.createPost(authorId, content, res.url).subscribe({
          next:()=>{this.selectedFile = null;
            this.previewUrl = null;
            this.postInput.nativeElement.innerText = '';
            this.hideForm();
            this.loadPosts();
          },
           error: (err)=>{console.error("Error", err);}
         });
        },
        err => { console.error("Error", err);}
      );
    } 
    else {
      this.postService.createPost(authorId, content, null).subscribe({
        next: (post) => {
          this.postInput.nativeElement.innerText = '';
          this.hideForm();
          this.loadPosts(); 
        },
        error: (err) => console.error("Error creating post:", err)
      });
    }  
  }
  
  submitComment(postId: string){
    const authorId = this.loggedInUser!.id;
    const content = this.commentInput.nativeElement.innerText.trim();
    if (!content) return;

    this.commentService.createComment(authorId, content, postId ).subscribe({
      next: (data) => {
        data.username = this.loggedInUser!.username; 
        this.commentCount = this.comments.length+1;
        this.comments.push(data); 
        this.commentInput.nativeElement.innerText = ''; 
      },
      error: (err)=> { console.error(err, "Error creating comment."); }
    });
    
  }
}
