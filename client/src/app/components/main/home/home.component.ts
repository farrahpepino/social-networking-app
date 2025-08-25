import { Component } from '@angular/core';
import { NavigationComponent } from '../../navigation/navigation.component';
import { PostService } from '../../../services/PostService/post.service';
import { ViewChild, ElementRef, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { UserService } from '../../../services/UserService/user.service';
import { CommentService } from '../../../services/CommentService/comment.service';
import { PostModel } from '../../../models/PostModel';
import { CommonModule } from '@angular/common';
import { UserModel } from '../../../models/UserModel';
import { CommentModel } from '../../../models/CommentModel';
import { LikeModel } from '../../../models/LikeModel';
@Component({
  selector: 'app-home',
  standalone: true, 
  imports: [NavigationComponent, CommonModule],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css',
})

export class HomeComponent implements OnInit {

  constructor(private userService: UserService, private postService: PostService, private commentService: CommentService, private router: Router) {}
  @ViewChild('postInput') postInput!: ElementRef<HTMLElement>;
  @ViewChild('commentInput') commentInput!: ElementRef<HTMLElement>;

  loggedInUser: UserModel | null = null;
  posts: PostModel[] = [];
  likes: LikeModel[] = [];
  post: PostModel | null = null;
  comments: CommentModel[] = [];
  commentCount: number = 0;

  ngOnInit(): void {
    this.userService.loggedInUser$.subscribe(user => {
      this.loggedInUser = user;
      this.loadPosts();
    });
    
   
  }

  showPost = false;
  showForm = false;

  loadPosts() {
    this.postService.getPosts().subscribe({
      next: (data) => {
        this.posts = data;
        this.posts.forEach(post => {
          this.postService.getLikes(post.id).subscribe({
            next: (likes: LikeModel[]) => {
              post.likes = likes;
              post.likedByUser = likes.some(like => like.likerId === this.loggedInUser?.id);
            }
          });
          this.commentService.getComments(post.id).subscribe({
            next: (comments) => {
              this.comments = Array.isArray(comments) ? comments : [comments];
              this.commentCount = this.comments.length;
            }
          });
        });
      },
      error: (err) => console.error("Error fetching posts:", err)
    });
  }
  toggleLike(post: PostModel) {
    if (!this.loggedInUser) return;
    if (post.likedByUser) {
      this.postService.unlikePost(post.id, this.loggedInUser.id).subscribe({
        next:()=>{
          post.likedByUser = false;
        },
        error: (err) => {
          console.error('Failed to unlike post', err);
        }
      });
    } else {
      this.postService.likePost(post.id, this.loggedInUser.id).subscribe({
        next: (like) => {
          post.likedByUser = true;
        },
        error: (err) => {
          console.error('Failed to like post', err);
        }
      });
    }
    this.loadPosts();
  }
  
  

  viewPost(id: string) {
    this.showPost = true; 
    this.postService.getPost(id).subscribe({
      next: (data) => {
        this.post = data;
        this.postService.getLikes(data.id).subscribe({
          next: (likes: LikeModel[]) => {
            data.likes = likes;
            data.likedByUser = likes.some(like => like.likerId === this.loggedInUser?.id);
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
  hideForm() { this.showForm = false;
  }
 

  submitPost() {
    const authorId = this.loggedInUser!.id;
    
    const content = this.postInput.nativeElement.innerText.trim();
    if (!content) return;

    this.postService.createPost(authorId, content).subscribe({
      error: (err) => {
        console.error('Error creating post:', err);
      }
    });
  }

  submitComment(postId: string){
    const authorId = this.loggedInUser!.id;
    const content = this.commentInput.nativeElement.innerText.trim();
    if (!content) return;

    this.commentService.createComment(authorId, content, postId ).subscribe({
      next: (data) => {
        data.username = this.loggedInUser!.username; 
        this.commentCount = this.comments.length;
        this.comments.push(data); 
        this.commentInput.nativeElement.innerText = ''; 
      },
      error: (err)=> { console.error(err, "Error creating comment."); }
    });
    
  }

}
