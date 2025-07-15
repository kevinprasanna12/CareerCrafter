import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';


@Component({
  selector: 'app-home',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './home.html',
  styleUrls: ['./home.scss']
})
export class Home {
  searchTerm: string = '';

  constructor(private router: Router) {}

  searchJobs() {
    if (this.searchTerm.trim()) {
      this.router.navigate(['/mainjoblistings'], { queryParams: { search: this.searchTerm.trim() } });
    }
}
}