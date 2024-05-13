import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewTeamsInActivityComponent } from './view-teams-in-activity.component';

describe('ViewTeamsInActivityComponent', () => {
  let component: ViewTeamsInActivityComponent;
  let fixture: ComponentFixture<ViewTeamsInActivityComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ViewTeamsInActivityComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(ViewTeamsInActivityComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
