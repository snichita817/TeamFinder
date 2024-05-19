import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TeamGetAllComponent } from './team-get-all.component';

describe('TeamGetAllComponent', () => {
  let component: TeamGetAllComponent;
  let fixture: ComponentFixture<TeamGetAllComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TeamGetAllComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(TeamGetAllComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
