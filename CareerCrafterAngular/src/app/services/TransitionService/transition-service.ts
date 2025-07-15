import { Injectable, ElementRef } from '@angular/core';
import gsap from 'gsap';
import { Router } from '@angular/router';

@Injectable({ providedIn: 'root' })
export class TransitionService {
  private overlayElement!: ElementRef;

  constructor(private router: Router) {}

  setOverlayElement(el: ElementRef) {
    this.overlayElement = el;
  }

  async navigateWithTransition(commands: any[], extras?: any) {
    const el = this.overlayElement.nativeElement;
    gsap.set(el, { display: 'block', y: '-100%' });

    await gsap.to(el, { y: '0%', duration: 0.5, ease: 'power2.out' }).then();

    // Now navigation happens AFTER animation
    await this.router.navigate(commands, extras);

    // Optional: slide out on new page
    await gsap.to(el, {
      y: '100%',
      duration: 0.5,
      ease: 'power2.in',
      onComplete: () => {
        gsap.set(el, { display: 'none' });
      }
    }).then();
  }
}
