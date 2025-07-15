import { Injectable } from '@angular/core';
import { ApiService } from '../../core/api.service';
import { Observable } from 'rxjs';
import { JobListingReadDto } from '../../models/JoblistingReaddto';
import { ApplicationReadDto } from '../../models/ApplicationReaddto';
import { JobListingCountDto } from '../../models/JoblistingCountdto';

@Injectable({
  providedIn: 'root'
})

export class Employeeservice {

  constructor(private api:ApiService) { }

  getMyJobListings(): Observable<JobListingReadDto[]> {
    return this.api.get<JobListingReadDto[]>('v1/joblisting/my-jobs');
    }

  getJobCount(): Observable<JobListingCountDto> {
    return this.api.get<JobListingCountDto>('v1/application/my-job-applicants/count');
    }

  getAppliedCount(): Observable<JobListingCountDto> {
    return this.api.get<JobListingCountDto>('v1/application/my-job-applicants/count');
    }

  getMyApplications(): Observable<ApplicationReadDto[]> {
    return this.api.get<ApplicationReadDto[]>('v1/application/my-job-applicants');
    }

    updateApplicationStatus(id: number, status: string): Observable<void> {
      return this.api.put<void>(`v1/application/${id}`, { status });
  }
    //  NEW: Get a specific job listing by ID for editing
    getJobListingById(id: number): Observable<JobListingReadDto> {
      return this.api.get<JobListingReadDto>(`v1/joblisting/${id}`);
    }
  
    //  NEW: Update a job listing by ID
    updateJobListing(id: number, updatedJob: any): Observable<void> {
      return this.api.put<void>(`v1/joblisting/${id}`, updatedJob);
    }
  
    // NEW: Delete a job listing by ID
    deleteJobListing(id: number): Observable<void> {
      return this.api.delete<void>(`v1/joblisting/${id}`);
    }

    // create joblisting 

    createJobListing(jobData: any): Observable<JobListingReadDto> {
      return this.api.post<JobListingReadDto>('v1/joblisting', jobData);
  }
  

}

