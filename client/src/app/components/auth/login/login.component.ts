import { Component } from '@angular/core';
import { ReactiveFormsModule, FormControl, FormGroup } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../../services/AuthService/auth.service';
@Component({
  selector: 'app-login',
  imports: [ReactiveFormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {

  constructor(private authService: AuthService){}

  loginForm = new FormGroup({
    email: new FormControl(''),
    password: new FormControl('')
  });

  onSubmit(){
    
    if (this.loginForm.valid){
      const {email, password} = this.loginForm.value;
      this.authService.loginUser(email!, password!)
      .subscribe({
        next: (response) => 
        {
          console.log('User logged in.');
        },
        error: (err) => 
        {
        console.error('Login failed.');
        }  
      })
    }

  }


}
