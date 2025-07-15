import { Component, OnInit } from '@angular/core';
import { ApplicationReadDto } from '../../../models/ApplicationReaddto';
import { ApiService } from '../../../core/api.service';
import { ActivatedRoute } from '@angular/router';
import { CommonModule } from '@angular/common';
import { ResumeService } from '../../../services/resume-service';

@Component({
  selector: 'app-jobseekerapplicationdetails',
  standalone:true,
  imports: [CommonModule],
  templateUrl: './jobseekerapplicationdetails.html',
  styleUrls: ['./jobseekerapplicationdetails.scss']
})
export class JobseekerApplicationDetails implements OnInit {
  applicationId!: number;
  application!: ApplicationReadDto;
  loading = false;
  error: string | null = null;

  constructor(
    private route: ActivatedRoute, 
    private api: ApiService ,
    private resumeService : ResumeService
    ) {}

  ngOnInit(): void {
    this.applicationId = Number(this.route.snapshot.paramMap.get('id'));
    this.fetchApplicationDetails();
  }

  fetchApplicationDetails() {
    this.loading = true;
    this.api.get<ApplicationReadDto[]>('v1/application/my-job-applicants').subscribe({
      next: (data) => {
        this.application = data.find(a => a.applicationId === this.applicationId)!;
        this.loading = false;
      },
      error: (err) => {
        this.error = err.error || 'Failed to load application details.';
        this.loading = false;
      }
    });
  }

  updateStatus(newStatus: string) {
    if (!confirm(`Are you sure you want to mark this application as ${newStatus}?`)) return;
    this.loading = true;
    this.api.put(`v1/application/${this.applicationId}`, { status: newStatus }).subscribe({
      next: (updatedApp) => {
        // If your API returns the updated application:
        // this.application = updatedApp;

        // If your API returns nothing:
        this.application.status = newStatus;

        this.loading = false;
      },
      error: (err) => {
        console.error('Error updating status:', err);
        this.error = err.error || 'Failed to update status.';
        this.loading = false;
      }
    });
  }

  downloadResume(resumeId: number) {
    this.resumeService.downloadResume(resumeId).subscribe(blob => {
      const url = window.URL.createObjectURL(blob);
      const a = document.createElement('a');
      a.href = url;
      a.download = 'resume.pdf'; // optional: use actual filename
      a.click();
      window.URL.revokeObjectURL(url);
    });
  }
  

}
