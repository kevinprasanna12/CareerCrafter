import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../services/auth';
import { RouterModule, Router } from '@angular/router';
import { TransitionService } from '../../services/TransitionService/transition-service';

@Component({
  selector: 'app-navbar',
  standalone:true,
  imports: [RouterModule,CommonModule],
  templateUrl: './navbar.html',
  styleUrls: ['./navbar.scss']
})
export class Navbar {
  isLoggedIn = false;

  constructor(
    private authService: AuthService,
    private router: Router,
    private transitionservice : TransitionService
    ) 
    {
    this.authService.loggedIn$.subscribe(status => {
      this.isLoggedIn = status;
      console.log('LoggedIn status:', status);
      
    });
  }

  logout() {
    this.authService.logout();
  }

  async navigateToDashboard() {
    const role = this.authService.getRole();
    console.log('Role detected in navbar:', role);
    if (role === 'Employer') {
      await this.transitionservice.navigateWithTransition(['/employee/dashboard']);
    } else if (role === 'JobSeeker') {
      await this.transitionservice.navigateWithTransition(['/jobseeker/dashboard']);
    } else {
      await this.transitionservice.navigateWithTransition(['/home']); // fallback if no valid role
    }
}
//for smooth animation and transition
async navigateWithTransition(path: string) {
  await this.transitionservice.navigateWithTransition([path]);
}



}
