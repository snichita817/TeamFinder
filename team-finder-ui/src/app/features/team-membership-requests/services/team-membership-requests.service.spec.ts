import { TestBed } from '@angular/core/testing';

import { TeamMembershipRequestsService } from './team-membership-requests.service';

describe('TeamMembershipRequestsService', () => {
  let service: TeamMembershipRequestsService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(TeamMembershipRequestsService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
