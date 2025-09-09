import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { AuthService } from '../../services/AuthService/auth.service';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { UserService } from '../../services/UserService/user.service';
import { UserSearchResponse } from '../../models/usersearchresponse';
import { ViewChild, ElementRef } from '@angular/core';


@Component({
  selector: 'app-navigation',
  imports: [CommonModule, FormsModule],
  templateUrl: './navigation.component.html',
  styleUrls: ['./navigation.component.css'] 
})
export class NavigationComponent {
  @ViewChild('searchInput') searchInput!: ElementRef<HTMLElement>;

  constructor(private authService: AuthService, private router: Router, private userService: UserService) {}
  searchQuery: string = "";
  users: UserSearchResponse[] =  [];
  showMenu = false;

  toggleMenu(){
    if(this.showMenu==true){
      this.showMenu = false;
    }
    else{
      this.showMenu = true;

    }
  }

  logout() {
    this.authService.logout();
  }

  viewProfile() {
    this.router.navigate(['/profile']); 
  }

  navigateToHome(){
    this.router.navigate(['/home']); 
  }

  onSearch(){
    
    if(this.searchQuery === "") {
      this.users = [];
      if(this.searchInput) {
        this.searchInput.nativeElement.style.visibility = 'hidden';
      }
      return;
    }

    this.userService.searchUser(this.searchQuery).subscribe({
      next : (data) => {
        this.users = data;
      },
      error: (err) => console.error('Error searching users')
    });
  }

  //FORCED. FIX LATER
  visitProfile(username: string) {
    if (this.router.url === `/profile/${username}`) {
      this.router.navigateByUrl('/', { skipLocationChange: true }).then(() => {
        this.router.navigateByUrl(`/profile/${username}`);
      });
    } else {
      this.router.navigateByUrl(`/profile/${username}`);
    }
  }
  

}
