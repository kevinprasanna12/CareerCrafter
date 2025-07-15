import { ResumeReadDto } from "./ResumeReaddto";

export interface ApplicationReadDto {
    applicationId: number;
    jobSeekerId: number;
    jobListingId: number;
    appliedDate: Date;
    status: string;
    jobTitle: string;          
    jobSeekerName: string;
    companyName : string;
    resume: ResumeReadDto | null;
}