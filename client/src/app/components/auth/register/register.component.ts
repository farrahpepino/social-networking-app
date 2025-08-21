import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormGroup, FormControl, AbstractControl, ValidationErrors } from '@angular/forms';
import { AuthService } from '../../../services/AuthService/auth.service';
import { UserService } from '../../../services/UserService/user.service';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent{

  constructor (private authService: AuthService, private userService: UserService){}

  registerForm = new FormGroup(
    {
      email: new FormControl(''),
      username: new FormControl(''),
      password: new FormControl(''),
      confirmPassword: new FormControl('')
    },
    {
      validators: (control: AbstractControl): ValidationErrors | null => {
        return control.get('password')?.value === control.get('confirmPassword')?.value ? null : { passwordsMismatch: true };
      }
    }
  );
  
  onSubmit(){
    if(this.registerForm.valid){
      const {username, password, email} = this.registerForm.value;
      this.authService.registerUser(username!, password!, email!)
      .subscribe({
        next: ()=>{
          const token = localStorage.getItem('token');
          this.userService.initializeUser(token!);
        },
        error: (err) => 
        {
        console.error('Registration failed. ');
        }
      });
    }
  }
}
