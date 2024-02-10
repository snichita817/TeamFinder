import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ActivityGetComponent } from './activity-get.component';

describe('ActivityGetComponent', () => {
  let component: ActivityGetComponent;
  let fixture: ComponentFixture<ActivityGetComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ActivityGetComponent]
    });
    fixture = TestBed.createComponent(ActivityGetComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
