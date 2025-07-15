import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MainJoblistingComponent } from './main-joblisting.component';

describe('MainJoblistingComponent', () => {
  let component: MainJoblistingComponent;
  let fixture: ComponentFixture<MainJoblistingComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MainJoblistingComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MainJoblistingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
