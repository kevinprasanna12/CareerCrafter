import { Routes } from '@angular/router';
import { Home } from './pages/home/home';
import { Register } from './pages/register/register';
import { Login } from './pages/login/login';
import { About } from './pages/about/about';
import { EmployeeCompleteProfile } from './pages/employee/complete-profile/complete-profile';
import { JobseekerCompleteProfile } from './pages/jobseeker/complete-profile/complete-profile';
import { Editprofile } from './pages/employee/EditProfile/editprofile/editprofile';
import { EmployeeDashboard } from './pages/employee/dashboard/dashboard';
import { Jobseekerdashboard } from './pages/jobseeker/jobseekerdashboard/jobseekerdashboard';
import { JobseekerApplicationDetails } from './pages/jobseeker/jobseekerapplicationdetails/jobseekerapplicationdetails';
import { MainJoblistingComponent } from './pages/main-joblisting.component/main-joblisting.component';



export const routes: Routes = [
    { path: 'home', component: Home },
    {path:'about', component:About},
    { path: 'signup', component: Register },
    { path: 'login', component: Login },
    { path: 'employee/completeprofile', component: EmployeeCompleteProfile },
    { path: 'jobseeker/completeprofile', component: JobseekerCompleteProfile },
    {path:'employee/dashboard',component:EmployeeDashboard},
    { path: 'employee/EditProfile/:id', component: Editprofile},
    { path: 'jobseekerapplicationdetails/:id', component: JobseekerApplicationDetails},
    {path:'jobseeker/dashboard', component:Jobseekerdashboard},
    {path:'mainjoblistings', component:MainJoblistingComponent},
    
    { path: '', redirectTo: 'home', pathMatch: 'full' },
    { path: '**', redirectTo: 'home' }
];
