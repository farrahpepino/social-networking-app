import { Component } from '@angular/core';
import { ReactiveFormsModule, FormControl, FormGroup } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../../services/AuthService/auth.service';
import { UserService } from '../../../services/UserService/user.service';
@Component({
  selector: 'app-login',
  imports: [ReactiveFormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {

  constructor(private authService: AuthService, private userService: UserService){}

  loginForm = new FormGroup({
    email: new FormControl(''),
    password: new FormControl('')
  });

  onSubmit(){
    if (this.loginForm.valid){
      const {email, password} = this.loginForm.value;
      this.authService.loginUser(email!, password!)
      .subscribe({
        next: () => 
        {
          const token = localStorage.getItem('5a6f9c4c-1b88-4d9f-b62f-9fcb9e91db26');
          this.userService.initializeUser(token!);
        },
        error: (err) => 
        {
        console.error('Login failed.');
        }  
      })
    }
  }


}
