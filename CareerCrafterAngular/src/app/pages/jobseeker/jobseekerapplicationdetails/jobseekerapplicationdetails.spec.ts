import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Jobseekerapplicationdetails } from './jobseekerapplicationdetails';

describe('Jobseekerapplicationdetails', () => {
  let component: Jobseekerapplicationdetails;
  let fixture: ComponentFixture<Jobseekerapplicationdetails>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Jobseekerapplicationdetails]
    })
    .compileComponents();

    fixture = TestBed.createComponent(Jobseekerapplicationdetails);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
