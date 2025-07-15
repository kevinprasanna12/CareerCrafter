import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ApiService } from '../../core/api.service';
import { ToastrService } from 'ngx-toastr';
import { ActivatedRoute } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { jwtDecode } from 'jwt-decode';


@Component({
  selector: 'app-main-joblisting.component',
  standalone:true,
  imports: [CommonModule, FormsModule],
  templateUrl: './main-joblisting.component.html',
  styleUrl: './main-joblisting.component.scss'
})
export class MainJoblistingComponent implements OnInit{

  jobListings: any[] = [];
  filteredJobListings: any[] = [];
  searchTerm: string = '';
  userRole : string='';

  constructor(
    private api: ApiService,
    private toastr: ToastrService,
    private route: ActivatedRoute,
    private dialog: MatDialog

  ) {}

  ngOnInit(): void {
    this.loadJobListings();

    // Capture search term from HomeComponent navigation
    this.route.queryParams.subscribe(params => {
      this.searchTerm = params['search'] || '';
      this.filterJobs();
    });

    const token = sessionStorage.getItem('token');
    if (token) {
      try {
        const decoded: any = jwtDecode(token);
        this.userRole = decoded.role || decoded.Role || decoded["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"] || '';
      } catch (error) {
        console.error('Failed to decode token', error);
      }
    }
  }

  loadJobListings() {
    this.api.get('v1/JobListing')
      .subscribe({
        next: (res : any) => {
          this.jobListings = res;
          this.filterJobs();
        },
        error: (err) => {
          console.error(err);
          this.toastr.error('Failed to load job listings.');
        }
      });
  }

  
  filterJobs() {
    const term = this.searchTerm.toLowerCase();
    this.filteredJobListings = this.jobListings.filter(job =>
      job.title.toLowerCase().includes(term) ||
      job.companyName.toLowerCase().includes(term) ||
      job.location.toLowerCase().includes(term)
    );
  }

  applyToJob(jobListingId: number) {
    const confirmed = confirm('Do you want to apply for this job?');
    if (!confirmed) return;

    this.api.post('v1/Application/apply', jobListingId)
      .subscribe({
        next: (res : any) => {
          this.toastr.success('You have successfully applied for this job.', 'Application Successful');

        },
        error: (err) => {
          console.error(err);
          this.toastr.error(err.error || 'Application failed.', 'Error');
        }
      });
  }

}
