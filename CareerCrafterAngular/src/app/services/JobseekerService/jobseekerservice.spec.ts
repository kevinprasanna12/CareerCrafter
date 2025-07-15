import { TestBed } from '@angular/core/testing';

import { Jobseekerservice } from './jobseeker.service';

describe('Jobseekerservice', () => {
  let service: Jobseekerservice;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(Jobseekerservice);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
