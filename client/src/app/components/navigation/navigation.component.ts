import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { AuthService } from '../../services/AuthService/auth.service';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { UserService } from '../../services/UserService/user.service';
import { UserSearchResponse } from '../../models/usersearchresponse';

@Component({
  selector: 'app-navigation',
  imports: [CommonModule, FormsModule],
  templateUrl: './navigation.component.html',
  styleUrls: ['./navigation.component.css'] 
})
export class NavigationComponent {

  constructor(private authService: AuthService, private router: Router, private userService: UserService) {}
  searchQuery: string = "";
  users: UserSearchResponse[] =  [];

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
    this.userService.searchUser(this.searchQuery).subscribe({
      next : (data) => {
        this.users = data;
      },
      error: (err) => console.error('Error searching users')
    });
  }

}
