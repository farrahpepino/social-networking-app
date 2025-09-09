import { Component } from '@angular/core';
import { ViewChild, ElementRef, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { UserService } from '../../../services/UserService/user.service';
import { PostService } from '../../../services/PostService/post.service';
import { CommentService } from '../../../services/CommentService/comment.service';
import { User } from '../../../models/user';
import { Post } from '../../../models/post';
import { Like } from '../../../models/like';
import { Comment } from '../../../models/comment';
import { NavigationComponent } from '../../navigation/navigation.component';
import { CommonModule } from '@angular/common';
import { InterestsService } from '../../../services/InterestService/interests.service';
import { InterestResponse } from '../../../models/interestresponse';
@Component({
  selector: 'app-profile',
  imports: [NavigationComponent, CommonModule],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.css'
})
export class ProfileComponent implements OnInit {

  constructor(private interestService: InterestsService, private userService: UserService, private postService: PostService, private commentService: CommentService, private router: Router) {}
  @ViewChild('postInput') postInput!: ElementRef<HTMLElement>;
  @ViewChild('commentInput') commentInput!: ElementRef<HTMLElement>;

  sessionUser: User | null = null;
  posts: Post[] = [];
  post: Post | null = null;
  comments: Comment[] = [];
  likes: Like[] = []
  commentCount: number = 0;
  likedPosts: { [postId: string]: boolean } = {};
  showPost = false;
  showForm = false;
  outerDropdown = false;
  innerDropdown = false;
  showInterest = false;
  isClicked = false;
  selectedFile: File | null = null;
  previewUrl: string | null = null;
  interests: InterestResponse[] = [];


  ngOnInit(): void {
    this.userService.sessionUser$.subscribe(user => {
      if (user) {   
        this.sessionUser = user;
        this.loadPosts();
        this.interestService.getInterests(user.id).subscribe(
          {
            next: (i: InterestResponse[])=>{
              this.interests = i;
            }
          }
        );
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
  

  loadPosts() {
    this.postService.getPostsByUserId(this.sessionUser!.id).subscribe({
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

          this.postService.isLiked(post.id, this.sessionUser!.id).subscribe({
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


  deletePost(id: string, key: string){
    if(key!==null){
      this.postService.deleteImage(key, this.sessionUser!.id).subscribe({
        next: () => {
          this.postService.deletePost(id).subscribe({
            next: () => {
              this.loadPosts();
            },
            error: (err) => console.error("Error deleting post:", err)});
        },
        error: (err) => console.error("Error deleting image:", err)
      })
    }
    else{
      this.postService.deletePost(id).subscribe({
        next: () => {
          this.loadPosts();
        },
        error: (err) => console.error("Error deleting post:", err)
      });
    }
  }

  onClick() {
    this.isClicked = true;
  }

  isLiked(postId: string){
    return !!this.likedPosts[postId];
  }

  toggleLike(post: Post) {
    const postId = post.id;
    const userId = this.sessionUser!.id;
  
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
            username: this.sessionUser!.username
          }];
          this.loadPosts();

        },
        error: (err) => console.error('Error liking post', err)
      });
    }
  }
  
  viewPost(id: string) {
    this.showPost = true; 
    this.outerDropdown = false;
    this.showForm = false; 
    this.showInterest = false;
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
    this.outerDropdown = false;
    this.innerDropdown = false;
    this.loadPosts();
  }

  viewForm() {  
    this.outerDropdown=false;
    this.showForm = true;
    this.showPost = false; 
    this.showInterest = false;
  }
  hideForm() { this.showForm = false; }
  viewInterest() {  
    this.outerDropdown=false;
    this.showInterest = true;  
  }
  hideInterest(){ this.showInterest = false;  }
  toggleDropdown(i: number){
    if(i==1){
      if(this.outerDropdown == true){
        this.outerDropdown = false;
      }
      else{
        this.outerDropdown = true;
      }
    }
    else{
      if(this.innerDropdown == true){
        this.innerDropdown = false;
      }
      else{
        this.innerDropdown = true;
      }
    }
    
  }
 
  submitPost() {
    const authorId = this.sessionUser!.id;
    const content = this.postInput.nativeElement.innerText.trim() || '';
    
    if (this.selectedFile!=null) {
      const formData = new FormData();
      formData.append("File", this.selectedFile);
      formData.append("UserId",  this.sessionUser!.id);

      this.postService.uploadImage(formData)
    .subscribe(
        res => { 
         this.postService.createPost(authorId, content, res.url, res.key).subscribe({
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
      this.postService.createPost(authorId, content, null, null).subscribe({
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
    const authorId = this.sessionUser!.id;
    const content = this.commentInput.nativeElement.innerText.trim();
    if (!content) return;

    this.commentService.createComment(authorId, content, postId ).subscribe({
      next: (data) => {
        data.username = this.sessionUser!.username; 
        this.commentCount = this.comments.length+1;
        this.comments.push(data); 
        this.commentInput.nativeElement.innerText = ''; 
      },
      error: (err)=> { console.error(err, "Error creating comment."); }
    });  
  }
}
