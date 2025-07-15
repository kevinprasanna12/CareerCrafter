import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ApplicationReadDto } from '../../../models/ApplicationReaddto';
import { Jobseekerservice } from '../../../services/JobseekerService/jobseeker.service';
import { CommonModule } from '@angular/common';
import { JobSeekerReadDto } from '../../../models/Jobseeker/JobSeekerReaddto';
import { AuthService } from '../../../services/auth';
import { TransitionService } from '../../../services/TransitionService/transition-service';
@Component({
  selector: 'app-jobseekerdashboard',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './jobseekerdashboard.html',
  styleUrls: ['./jobseekerdashboard.scss']
})

export class Jobseekerdashboard implements OnInit {

  applications: ApplicationReadDto[] = [];
  loadingApplications = true;
  selectedFile?: File;
  uploading = false;
  jobSeekerProfile?: JobSeekerReadDto;
  resumeIdForDownload?: number;
  applicationCount: number = 0;
  loadingApplicationCount = true;

  constructor(
    private router: Router,
    private jobseekerservice: Jobseekerservice,
    private authservice : AuthService,
    private transitionservice : TransitionService
    ) {}

  ngOnInit(): void {
    this.loadApplications();
    this.loadJobSeekerProfile();
    this.loadResumeIdForDownload();
    this.loadApplicationCount();
  }

  loadApplications(): void {
    this.jobseekerservice.getMyApplications().subscribe({
      next: (data) => {
        this.applications = data;
        this.loadingApplications = false;
      },
      error: (err) => {
        console.error('Failed to load applications', err);
        this.loadingApplications = false;
      }
    });
  }

  loadJobSeekerProfile(): void {
    this.jobseekerservice.getMyProfile().subscribe({
      next: (data) => {
        this.jobSeekerProfile = data;
      },
      error: (err) => {
        console.error('Failed to load profile', err);
      }
    });
  }

  /**
   * Fetches the resumeId for download.
   * You should replace this with an actual API call that fetches the current resume tied to the JobSeeker.
   */
  loadResumeIdForDownload(): void {
    this.jobseekerservice.getMyResumeId().subscribe({
      next: (id) => {
        this.resumeIdForDownload = id;
      },
      error: (err) => {
        console.error('Failed to fetch resume ID', err);
        alert('Failed to fetch resume ID.');
      }
    });
  }

  onFileSelected(event: Event): void {
    const target = event.target as HTMLInputElement;
    if (target.files && target.files.length > 0) {
      this.selectedFile = target.files[0];
    }
  }

  uploadResume(): void {
    if (!this.selectedFile) {
      alert('Please select a file to upload.');
      return;
    }

    this.uploading = true;

    this.jobseekerservice.uploadResume(this.selectedFile).subscribe({
      next: (res) => {
        console.log('Resume uploaded successfully:', res);
        alert('Resume uploaded successfully.');
        this.uploading = false;
        this.selectedFile = undefined;
        this.loadResumeIdForDownload(); // refresh download availability if needed
      },
      error: (err) => {
        console.error('Failed to upload resume', err);
        alert('Failed to upload resume.');
        this.uploading = false;
      }
    });
  }

  downloadResume(): void {
    if (!this.resumeIdForDownload) {
        alert('No resume available for download.');
        return;
    }

    this.jobseekerservice.downloadResume(this.resumeIdForDownload).subscribe({
        next: (blob) => {
            const url = window.URL.createObjectURL(blob);
            const a = document.createElement('a');
            a.href = url;
            a.download = 'resume.pdf'; // or dynamically fetched name
            a.click();
            window.URL.revokeObjectURL(url);
        },
        error: (err) => {
            console.error('Failed to download resume', err);
            alert('Failed to download resume.');
        }
    });
}

loadApplicationCount(): void {
  this.jobseekerservice.getMyApplicationCount().subscribe({
      next: (count) => {
          this.applicationCount = count;
          this.loadingApplicationCount = false;
      },
      error: (err) => {
          console.error('Failed to load application count', err);
          this.loadingApplicationCount = false;
      }
  });
}

logout() {
  sessionStorage.clear();
  this.router.navigate(['/home']);
  this.authservice.logout();
}

async navigateWithTransition(path: string) {
  await this.transitionservice.navigateWithTransition([path]);
}
  
}
