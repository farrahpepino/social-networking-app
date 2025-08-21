import { Component } from '@angular/core';
import { NavigationComponent } from '../../navigation/navigation.component';
import { PostService } from '../../../services/PostService/post.service';
import { ViewChild, ElementRef, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { UserService } from '../../../services/UserService/user.service';
import { PostModel } from '../../../models/PostModel';
import { CommonModule } from '@angular/common';
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

  posts: PostModel[] = [];
  ngOnInit(): void {
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
  viewPost() { this.showPost = true; }
  hidePost() { this.showPost = false; }
  viewForm() { this.showForm = true; }
  hideForm() { this.showForm = false; }
  
 
  submitPost() {
    const token = localStorage.getItem('token');
    const user = this.userService.getCurrentUser(token!);
    if (!token) {
      alert('You must be logged in to post.');
      return;
    }
    const authorId = user.id;
    
    const content = this.postInput.nativeElement.innerText.trim();
    if (!content) return;

    this.postService.createPost(authorId, content).subscribe({
      next: () => {
        alert('Post created.');
        this.postInput.nativeElement.innerText = ''; 
      },
      error: (err) => {
        console.error('Error creating post:', err);
        alert('Failed to create post.');
      }
    });
  }


  


}
