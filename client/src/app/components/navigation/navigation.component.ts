import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { AuthService } from '../../services/AuthService/auth.service';

@Component({
  selector: 'app-navigation',
  imports: [CommonModule],
  templateUrl: './navigation.component.html',
  styleUrls: ['./navigation.component.css'] 
})
export class NavigationComponent {

  constructor(private authService: AuthService) {}

  logout() {
    this.authService.logout();
    alert("logging out");
  }

  viewProfile() {
    alert("viewing profile");
  }

}
