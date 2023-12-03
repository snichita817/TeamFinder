import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ActivityDeleteComponent } from './activity-delete.component';

describe('ActivityDeleteComponent', () => {
  let component: ActivityDeleteComponent;
  let fixture: ComponentFixture<ActivityDeleteComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ActivityDeleteComponent]
    });
    fixture = TestBed.createComponent(ActivityDeleteComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
