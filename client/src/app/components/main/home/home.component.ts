import { Component } from '@angular/core';
import { NavigationComponent } from '../../navigation/navigation.component';
import { PostService } from '../../../services/PostService/post.service';
import { ViewChild, ElementRef, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { UserService } from '../../../services/UserService/user.service';
import { PostModel } from '../../../models/PostModel';
import { CommonModule } from '@angular/common';
import { UserModel } from '../../../models/UserModel';
@Component({
  selector: 'app-home',
  standalone: true, 
  imports: [NavigationComponent, CommonModule],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css',
})

export class HomeComponent implements OnInit {

  constructor(private userService: UserService, private postService: PostService, private router: Router) {}
  @ViewChild('postInput') postInput!: ElementRef<HTMLElement>;
  loggedInUser: UserModel | null = null;
  posts: PostModel[] = [];
  post: PostModel | null = null;

  ngOnInit(): void {
    this.userService.loggedInUser$.subscribe(user => {
      this.loggedInUser = user;
    });
    this.postService.getPosts().subscribe(
      {
        next: (data)=>{
          this.posts = data;
        },
        error: (err)=>{
          console.error("Error fetching posts: ", err);
        }
      }
    );
  }

  showPost = false;
  showForm = false;
  viewPost(id: string) {
    this.showPost = true; 
    this.postService.getPost(id).subscribe({
      next: (data) => {
        this.post = data;
      },
      error: (err) => {
        console.error('Error fetching post: ', err);
      }
    });
    }

  hidePost() { 
    this.post = null;
    this.showPost = false; 
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

}
