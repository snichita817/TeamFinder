import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ActivityListPrivateComponent } from './activity-list-private.component';

describe('ActivityListPrivateComponent', () => {
  let component: ActivityListPrivateComponent;
  let fixture: ComponentFixture<ActivityListPrivateComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ActivityListPrivateComponent]
    });
    fixture = TestBed.createComponent(ActivityListPrivateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
