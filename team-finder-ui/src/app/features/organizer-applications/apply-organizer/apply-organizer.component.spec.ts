import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ApplyOrganizerComponent } from './apply-organizer.component';

describe('ApplyOrganizerComponent', () => {
  let component: ApplyOrganizerComponent;
  let fixture: ComponentFixture<ApplyOrganizerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ApplyOrganizerComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(ApplyOrganizerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
