export interface JobListingReadDto {
    jobListingId: number;
    title: string;
    description: string;
    location: string;
    qualifications: string;
    salary: number | null;
    employerId: number;
    companyName : string;
}