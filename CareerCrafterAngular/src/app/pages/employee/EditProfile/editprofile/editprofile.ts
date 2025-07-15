import { Component ,OnInit} from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { Employeeservice } from '../../../../services/EmployeeService/employeeservice';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { JobListingReadDto } from '../../../../models/JoblistingReaddto';
import { ReactiveFormsModule } from '@angular/forms';
import { TransitionService } from '../../../../services/TransitionService/transition-service';

@Component({
  selector: 'app-editprofile',
  standalone:true,
  imports: [ReactiveFormsModule,CommonModule],
  templateUrl: './editprofile.html',
  styleUrls: ['./editprofile.scss']
})
export class Editprofile implements OnInit {
  editForm!: FormGroup;
  jobListingId!: number;
  jobListing!: JobListingReadDto;

  constructor(
    private route: ActivatedRoute,
    private fb: FormBuilder,
    private employeeService: Employeeservice,
    protected router: Router,
    private transitionservice : TransitionService
  ) {}

  ngOnInit(): void {
    this.jobListingId = Number(this.route.snapshot.paramMap.get('id'));
    this.loadJobListing();
  }

  loadJobListing() {
    this.employeeService.getJobListingById(this.jobListingId).subscribe({
      next: (data) => {
        this.jobListing = data;
        this.initializeForm();
      },
      error: (err) => console.error(err)
    });
  }
  initializeForm() {
    this.editForm = this.fb.group({
      title: [this.jobListing.title, Validators.required],
      description: [this.jobListing.description, Validators.required],
      location: [this.jobListing.location, Validators.required],
      qualifications: [this.jobListing.qualifications, Validators.required],
      salary: [this.jobListing.salary, [Validators.required, Validators.min(0)]],
    });
  }

  onSubmit() {
    if (this.editForm.invalid) return;

    const updatedJob = {
      ...this.jobListing,
      ...this.editForm.value
    };

    this.employeeService.updateJobListing(this.jobListingId, updatedJob).subscribe({
      next: () => {
        alert('Job listing updated successfully!');
        this.router.navigate(['/employee/dashboard']);
      },
      error: (err) => console.error(err)
    });
  }

  deleteJobListing(jobListingId: number) {
      if (confirm('Are you sure you want to delete this job listing?')) {
        this.employeeService.deleteJobListing(jobListingId).subscribe({
          next: () => {
            this.transitionservice.navigateWithTransition(['/employee/dashboard'])
          },
          error: (err) => console.error(err)
        });
      }
    } 
}
