import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth';
// import { CommonModule } from '@angular/common;
import { FormsModule } from '@angular/forms';
import { ApiService } from '../../core/api.service';

@Component({
  selector: 'app-login',
  standalone : true,
  imports: [FormsModule],
  templateUrl: './login.html',
  styleUrls: ['./login.scss']
})
export class Login {
  username = '';
  password = '';
  error = '';
  
  constructor(private authService: AuthService, private router: Router , private api : ApiService) {}

  onLogin() {
    const credentials = {
      username: this.username,
      password: this.password
    };
  
    this.authService.login(credentials).subscribe({
      next: (response) => {

        if(response?.token){
        this.authService.saveToken(response.token);
        } else{
          console.error('Token missing in response', response);
          this.error = 'Login failed: Token missing in response.';
          return;
        }
  
        let tokenPayload;
        try {
          tokenPayload = this.authService.getTokenPayload();
        } catch (e) {
          console.error('Invalid token structure', e);
          this.error = 'Login failed: Invalid token.';
          return;
        }
        const userId = tokenPayload.userId;
        const role = tokenPayload.role;
  
        this.api.checkProfileCompletion(userId, role).subscribe({
          next: (profile) => {
            // If profile exists, navigate home
            this.router.navigate(['/']);
          },
          error: () => {
            // If profile does not exist, navigate to complete profile page
            if (role === 'Employer') {
              this.router.navigate(['/employee/completeprofile']);
            } else if (role === 'JobSeeker') {
              this.router.navigate(['/jobseeker/completeprofile']);
            } else {
              this.router.navigate(['/']); // fallback
            }
          }
        });
      },
      error: (err) => {
        console.error(err);
        this.error = 'Invalid username or password.';
      }
    });
  }
  
}
