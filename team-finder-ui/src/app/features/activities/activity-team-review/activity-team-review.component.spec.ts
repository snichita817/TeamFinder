import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ActivityTeamReviewComponent } from './activity-team-review.component';

describe('ActivityTeamReviewComponent', () => {
  let component: ActivityTeamReviewComponent;
  let fixture: ComponentFixture<ActivityTeamReviewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ActivityTeamReviewComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(ActivityTeamReviewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
