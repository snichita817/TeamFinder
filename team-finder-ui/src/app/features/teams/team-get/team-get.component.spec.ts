import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TeamGetComponent } from './team-get.component';

describe('TeamGetComponent', () => {
  let component: TeamGetComponent;
  let fixture: ComponentFixture<TeamGetComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TeamGetComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(TeamGetComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
