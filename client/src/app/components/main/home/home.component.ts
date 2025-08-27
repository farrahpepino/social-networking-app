import { Component } from '@angular/core';
import { NavigationComponent } from '../../navigation/navigation.component';
import { PostService } from '../../../services/PostService/post.service';
import { ViewChild, ElementRef, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { UserService } from '../../../services/UserService/user.service';
import { CommentService } from '../../../services/CommentService/comment.service';
import { PostDto } from '../../../dtos/PostDto';
import { CommonModule } from '@angular/common';
import { UserDto } from '../../../dtos/UserDto';
import { CommentDto } from '../../../dtos/CommentDto';
import { LikeDto } from '../../../dtos/LikeDto';
import { UploadimageComponent } from '../../uploadimage/uploadimage.component';
@Component({
  selector: 'app-home',
  standalone: true, 
  imports: [NavigationComponent, CommonModule, UploadimageComponent],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css',
})

export class HomeComponent implements OnInit {

  constructor(private userService: UserService, private postService: PostService, private commentService: CommentService, private router: Router) {}
  @ViewChild('postInput') postInput!: ElementRef<HTMLElement>;
  @ViewChild('commentInput') commentInput!: ElementRef<HTMLElement>;

  loggedInUser: UserDto | null = null;
  posts: PostDto[] = [];
  post: PostDto | null = null;
  comments: CommentDto[] = [];
  likes: LikeDto[] = []
  commentCount: number = 0;
  likedPosts: { [postId: string]: boolean } = {};

  ngOnInit(): void {
    this.userService.loggedInUser$.subscribe(user => {
      this.loggedInUser = user;
      this.loadPosts();
    });
  }

  showPost = false;
  showForm = false;

  isLiked(postId: string){
    return !!this.likedPosts[postId];
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


  
  toggleLike(post: PostDto) {
    const postId = post.id;
    const userId = this.loggedInUser!.id;
  
    if (this.likedPosts[postId]) {
      this.postService.unlikePost(postId, userId).subscribe({
        next: () => {
          this.likedPosts[postId] = false;
          post.likes = post.likes?.filter(like => like.likerId !== userId) || [];
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
        },
        error: (err) => console.error('Error liking post', err)
      });
    }
    this.loadPosts();
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
        this.commentCount = this.comments.length+1;
        this.comments.push(data); 
        this.commentInput.nativeElement.innerText = ''; 
      },
      error: (err)=> { console.error(err, "Error creating comment."); }
    });
    
  }
}
