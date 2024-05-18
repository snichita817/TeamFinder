import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TeamMembershipRequestsViewComponent } from './team-membership-requests-view.component';

describe('TeamMembershipRequestsViewComponent', () => {
  let component: TeamMembershipRequestsViewComponent;
  let fixture: ComponentFixture<TeamMembershipRequestsViewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TeamMembershipRequestsViewComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(TeamMembershipRequestsViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
