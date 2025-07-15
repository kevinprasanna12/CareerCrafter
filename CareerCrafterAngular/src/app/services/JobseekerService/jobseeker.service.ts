import { Injectable } from '@angular/core';
import { ApiService } from '../../core/api.service';
import { ApplicationReadDto } from '../../models/ApplicationReaddto';
import { Observable } from 'rxjs';
import { JobSeekerReadDto } from '../../models/Jobseeker/JobSeekerReaddto';

@Injectable({
  providedIn: 'root'
})
export class Jobseekerservice {

  constructor(private api : ApiService) { }


  getMyApplications(): Observable<ApplicationReadDto[]> {
    return this.api.get<ApplicationReadDto[]>('v1/application/my-applications');
    }

  getMyProfile(): Observable<JobSeekerReadDto> {
      return this.api.get<JobSeekerReadDto>('v1/jobseeker/my-profile');
    }

    uploadResume(file: File): Observable<string> {
      const formData = new FormData();
      formData.append('file', file);
  
      return this.api.post<string>('v1/resume/upload-resume', formData);
    }

  downloadResume(resumeId: number): Observable<Blob> {
    const url = `v1/Resume/download/${resumeId}`;
    return this.api.getWithOptions(url, { responseType: 'blob' as 'blob' });
  }

  getMyResumeId(): Observable<number> {
    return this.api.get<number>('v1/resume/my-resume-id');
  }

  getMyApplicationCount(): Observable<number> {
    return this.api.get<number>('v1/application/my-applications/count');
}

}
