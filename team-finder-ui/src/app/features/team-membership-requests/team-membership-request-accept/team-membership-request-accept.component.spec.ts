import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TeamMembershipRequestAcceptComponent } from './team-membership-request-accept.component';

describe('TeamMembershipRequestAcceptComponent', () => {
  let component: TeamMembershipRequestAcceptComponent;
  let fixture: ComponentFixture<TeamMembershipRequestAcceptComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TeamMembershipRequestAcceptComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(TeamMembershipRequestAcceptComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
