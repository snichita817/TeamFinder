import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminApplicationReviewComponent } from './admin-application-review.component';

describe('AdminApplicationReviewComponent', () => {
  let component: AdminApplicationReviewComponent;
  let fixture: ComponentFixture<AdminApplicationReviewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AdminApplicationReviewComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(AdminApplicationReviewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
