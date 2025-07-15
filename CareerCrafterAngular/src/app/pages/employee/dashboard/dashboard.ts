import { Component , OnInit} from '@angular/core';
import { FormsModule } from '@angular/forms';
import { JobListingReadDto } from '../../../models/JoblistingReaddto';
import { ApplicationReadDto } from '../../../models/ApplicationReaddto';
import { Employeeservice } from '../../../services/EmployeeService/employeeservice';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { AuthService } from '../../../services/auth';
import { TransitionService } from '../../../services/TransitionService/transition-service';

@Component({
  selector: 'app-dashboard',
  standalone:true,
  imports: [CommonModule,FormsModule],
  templateUrl: './dashboard.html',
  styleUrls: ['./dashboard.scss']
})
export class EmployeeDashboard implements OnInit {
    
    jobListings: JobListingReadDto[] = [];
    jobCount: number = 0;
    appliedCount: number = 0;
    applications: ApplicationReadDto[] = [];
    showAddForm: boolean = false;

    newJobListing = {
        title: '',
        description: '',
        location: '',
        qualifications: '',
        salary: null
    };
  
    constructor(
          private employeeService:Employeeservice,
          private router : Router,
          private authservice : AuthService,
          private transitionservice : TransitionService
          ){}

    ngOnInit(): void {
      this.loadDashboardData();
    }

    loading: boolean = false; // optional for spinner usage

    loadDashboardData() {
      this.loading = true;
      this.loadJobListings();
      this.loadJobCount();
      this.loadAppliedCount();
      this.loadApplications();
      this.loading = false;
    }

    toggleAddJobListing() {
      this.showAddForm = !this.showAddForm;
  }
  
  submitJobListing() {
      if (!this.newJobListing.title || !this.newJobListing.description || !this.newJobListing.location || !this.newJobListing.qualifications) {
          alert('Please fill in all required fields.');
          return;
      }
  
      this.employeeService.createJobListing(this.newJobListing).subscribe({
          next: () => {
              this.loadJobListings(); // reload listings
              this.toggleAddJobListing(); // hide form
              this.resetNewJobListing(); // clear form
          },
          error: (err) => console.error(err)
      });
  }
  
  resetNewJobListing() {
      this.newJobListing = {
          title: '',
          description: '',
          location: '',
          qualifications: '',
          salary: null
      };
  }
  




    loadJobListings() {
      this.employeeService.getMyJobListings().subscribe({
        next: (data) => this.jobListings = data.sort((a, b) => b.jobListingId - a.jobListingId),
        error: (err) => console.error(err)
      });
    }

    loadJobCount() {
      this.employeeService.getJobCount().subscribe({
        next: (response) => this.jobCount = response.totalJobListings,
        error: (err) => console.error(err)        
      });
    }

    loadAppliedCount() {
      this.employeeService.getAppliedCount().subscribe({
        next: (response) => this.appliedCount = response.jobListingsWithApplications,
        error: (err) => console.error(err)
      });
    }

    loadApplications() {
      this.employeeService.getMyApplications().subscribe({
        next: (data) => this.applications = data,
        error: (err) => console.error(err)
      });
    }

    approveApplication(id: number) {
      if (confirm('Are you sure you want to approve this application?')) {
        this.employeeService.updateApplicationStatus(id, 'Approved').subscribe({
          next: () => this.loadApplications(),
          error: (err) => console.error(err)
        });
      }

    }

    rejectApplication(id: number) {
      if (confirm('Are you sure you want to reject this application?')) {
        this.employeeService.updateApplicationStatus(id, 'Rejected').subscribe({
          next: () => this.loadApplications(),
          error: (err) => console.error(err)
          });
        }
      }

      viewApplication(applicationId: number) {
        this.router.navigate(['/jobseekerapplicationdetails', applicationId]);
      }

      editJobListing(jobListingId: number) {
        // Navigate to edit profile page with the jobListingId
        this.router.navigate(['/employee/EditProfile', jobListingId]);
      }
      
      logout() {
        sessionStorage.clear();
        this.router.navigate(['/home']);
        this.authservice.logout();
    }

    async navigateWithTransition(path: string) {
      await this.transitionservice.navigateWithTransition([path]);
  }


      // deleteJobListing(jobListingId: number) {
      //   if (confirm('Are you sure you want to delete this job listing?')) {
      //     this.employeeService.deleteJobListing(jobListingId).subscribe({
      //       next: () => {
      //         // Reload job listings after deletion
      //         this.loadJobListings();
      //       },
      //       error: (err) => console.error(err)
      //     });
      //   }

      // navigatetoHome() {
      //   this.router.navigate(['/home']);
      // }
      // navigatetoAbout() {
      //   this.router.navigate(['/about']);
      // }
      // navigatetoJobs() {
      //   this.router.navigate(['/mainjoblistings']);
      // }

  
}
