import { TestBed } from '@angular/core/testing';

import { ProcessCvService } from './process-cv.service';

describe('ProcessCvService', () => {
  let service: ProcessCvService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ProcessCvService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
