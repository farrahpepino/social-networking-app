import { Component } from '@angular/core';
import { NavigationComponent } from '../../navigation/navigation.component';
import { PostService } from '../../../services/PostService/post.service';
import { ViewChild, ElementRef } from '@angular/core';
@Component({
  selector: 'app-home',
  standalone: true, 
  imports: [NavigationComponent],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent {

  constructor(private postService: PostService) {}
  @ViewChild('postInput') postInput!: ElementRef<HTMLElement>;

  showPost = false;
  showForm = false;
  viewPost() { this.showPost = true; }
  hidePost() { this.showPost = false; }
  viewForm() { this.showForm = true; }
  hideForm() { this.showForm = false; }
  
  // authorId: string = ""; //change this once auth is created
  
  submitPost() {

    const content = this.postInput.nativeElement.innerText.trim();
    if (!content) return;

    this.postService.createPost( "admin", content).subscribe({
      next: (post) => {
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
