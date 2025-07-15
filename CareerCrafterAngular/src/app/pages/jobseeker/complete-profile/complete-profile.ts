import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ApiService } from '../../../core/api.service';
import { Router } from '@angular/router';
import { AuthService } from '../../../services/auth';

@Component({
  selector: 'app-jobseeker-complete-profile',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './complete-profile.html',
  styleUrls: ['./complete-profile.scss']
})
export class JobseekerCompleteProfile {
  fullName = '';
  email = '';
  error = '';

  constructor(
    private api: ApiService,
    private router: Router,
    private authService: AuthService
  ) {}

  saveProfile() {
    const userId = this.authService.getTokenPayload().userId;

    const payload = {
      userId: userId,
      fullName: this.fullName,
      email: this.email
    };

    this.api.post('/v1/jobseeker', payload).subscribe({
      next: () => {
        this.router.navigate(['/']); // navigate home after completion
      },
      error: (err) => {
        console.error(err);
        this.error = err.error?.message || 'Profile completion failed.';
      }
    });
  }
}
