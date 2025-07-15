import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ApiService } from '../../../core/api.service';
import { Router } from '@angular/router';
import { AuthService } from '../../../services/auth';

@Component({
  selector: 'app-complete-profile',
  standalone:true,
  imports: [CommonModule,FormsModule],
  templateUrl: './complete-profile.html',
  styleUrls: ['./complete-profile.scss']
})
export class EmployeeCompleteProfile {
  companyName = '';
  companyEmail = '';
  error = '';

  constructor(private api: ApiService, private router: Router, private authservice : AuthService) {}

  saveProfile() {
    const userId = this.authservice.getTokenPayload().userId;

    const payload = {
      userId: userId,
      companyName: this.companyName,
      contactEmail: this.companyEmail
    };

    this.api.post('/v1/employee', payload).subscribe({
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
