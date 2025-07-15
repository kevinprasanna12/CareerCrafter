import { Component, ElementRef, ViewChild, AfterViewInit } from '@angular/core';
import { Router, RouterOutlet, NavigationStart } from '@angular/router';
import { Navbar } from './shared/navbar/navbar';
import { Footer } from "./shared/footer/footer";
import { CommonModule } from '@angular/common';
import { TransitionService } from './services/TransitionService/transition-service';
@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule,RouterOutlet, Navbar, Footer],
  templateUrl: './app.html',
  styleUrls: ['./app.scss']
})
export class App implements AfterViewInit{
  @ViewChild('pageTransition') pageTransition! : ElementRef;


  protected title = 'CareerCrafterAngular';
  protected hideNavAndFooter = false;

  constructor(private router: Router, private transitionService: TransitionService) {
    this.router.events.subscribe(() => {
      const currentUrl = this.router.url;
      this.hideNavAndFooter = 
        currentUrl.includes('/employee/dashboard') ||
        currentUrl.includes('/jobseeker/dashboard');
    });
  }

  ngAfterViewInit() {
    this.transitionService.setOverlayElement(this.pageTransition);
  }

  // playPageTransition() {
  //   const el = this.pageTransition.nativeElement;

  //   gsap.set(el, { display: 'block', y: '-100%' });

  //   const tl = gsap.timeline({
  //     onComplete: () => {
  //       gsap.set(el, { display: 'none' });
  //     }
  //   });

  //   tl.to(el, { y: '0%' }) 
  //     .to(el, { duration: 0.5 }) 
  //     .to(el, { y: '100%', duration: 0.5, ease: 'power2.in' }); 
  // }

  

}
