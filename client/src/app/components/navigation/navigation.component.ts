import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { AuthService } from '../../services/AuthService/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-navigation',
  imports: [CommonModule],
  templateUrl: './navigation.component.html',
  styleUrls: ['./navigation.component.css'] 
})
export class NavigationComponent {

  constructor(private authService: AuthService, private router: Router) {}

  logout() {
    this.authService.logout();
  }

  viewProfile() {
    this.router.navigate(['/profile']); 
  }

  navigateToHome(){
    this.router.navigate(['/home']); 
  }

}
