import { Component } from '@angular/core';
import { LoginComponent } from '../../auth/login/login.component';
import { RegisterComponent } from '../../auth/register/register.component';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-hero',
  imports: [LoginComponent, RegisterComponent, CommonModule],
  templateUrl: './hero.component.html',
  styleUrl: './hero.component.css'
})
export class HeroComponent {
  showLogin = true;
  
  toggleForm(){
    this.showLogin = !this.showLogin;
  }
}
