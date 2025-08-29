import { Component } from '@angular/core';
import { ViewChild, ElementRef, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { UserService } from '../../../services/UserService/user.service';
import { PostService } from '../../../services/PostService/post.service';
import { CommentService } from '../../../services/CommentService/comment.service';
import { User } from '../../../models/User';
import { Post } from '../../../models/Post';
import { Like } from '../../../models/Like';
import { Comment } from '../../../models/Comment';
import { NavigationComponent } from '../../navigation/navigation.component';
import { CommonModule } from '@angular/common';
@Component({
  selector: 'app-profile',
  imports: [NavigationComponent, CommonModule],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.css'
})
export class ProfileComponent implements OnInit {

  constructor(private userService: UserService, private postService: PostService, private commentService: CommentService, private router: Router) {}
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

  ngOnInit(): void {
    this.userService.loggedInUser$.subscribe(user => {
      if (user) {   
        this.loggedInUser = user;
        this.loadPosts();
      }
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
  
  upload() {
    if (!this.selectedFile) {
      console.error("No file selected");
      return;
    }

    const formData = new FormData();
    formData.append("file", this.selectedFile);

  }

  loadPosts() {
    this.postService.getPostsByUserId(this.loggedInUser!.id).subscribe({
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
