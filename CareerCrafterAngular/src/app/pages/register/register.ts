import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../services/auth';
import { Router } from '@angular/router';



@Component({
  selector: 'app-register',
  standalone: true,
  imports: [FormsModule,CommonModule],
  templateUrl: './register.html',
  styleUrls: ['./register.scss']
})
export class Register {
  username: string = '';
  password: string = '';
  email : string = '';
  confirmPassword: string = '';
  role: string = '';
  error ='';

  constructor(private authService:AuthService, private router : Router){}

  onSignup() {
    if (this.password !== this.confirmPassword) {
      this.error = 'Passwords do not match.';
      return;
    }

    const userData = {
      username: this.username,
      password: this.password,
      email:this.email,
      role : this.role
    };

    this.authService.signup(userData).subscribe({
      next: () => {
        this.router.navigate(['/login']);
      },
      error: (err) => {
        console.error(err);
        this.error = err.error?.message || 'Registration failed.';
      }
    });
  }
}
