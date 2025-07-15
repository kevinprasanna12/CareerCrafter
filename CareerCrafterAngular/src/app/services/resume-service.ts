import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from '../core/api.service';

@Injectable({
  providedIn: 'root'
})
export class ResumeService {

  constructor(private api:ApiService ) {}

  downloadResume(resumeId: number): Observable<Blob> {
    const url = `v1/Resume/download/${resumeId}`;
    return this.api.getWithOptions(url, { responseType: 'blob' as 'blob' });
}

}
