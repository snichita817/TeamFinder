import { TestBed } from '@angular/core/testing';

import { OrganizerApplicationService } from './organizer-application.service';

describe('OrganizerApplicationService', () => {
  let service: OrganizerApplicationService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(OrganizerApplicationService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
