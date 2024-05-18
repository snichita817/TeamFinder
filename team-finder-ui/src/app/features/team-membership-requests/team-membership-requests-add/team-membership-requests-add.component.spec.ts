import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TeamMembershipRequestsAddComponent } from './team-membership-requests-add.component';

describe('TeamMembershipRequestsAddComponent', () => {
  let component: TeamMembershipRequestsAddComponent;
  let fixture: ComponentFixture<TeamMembershipRequestsAddComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TeamMembershipRequestsAddComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(TeamMembershipRequestsAddComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
