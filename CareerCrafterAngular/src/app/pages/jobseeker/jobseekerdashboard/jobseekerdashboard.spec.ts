import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Jobseekerdashboard } from './jobseekerdashboard';

describe('Jobseekerdashboard', () => {
  let component: Jobseekerdashboard;
  let fixture: ComponentFixture<Jobseekerdashboard>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Jobseekerdashboard]
    })
    .compileComponents();

    fixture = TestBed.createComponent(Jobseekerdashboard);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
