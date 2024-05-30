import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GetUserteamsComponent } from './get-userteams.component';

describe('GetUserteamsComponent', () => {
  let component: GetUserteamsComponent;
  let fixture: ComponentFixture<GetUserteamsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [GetUserteamsComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(GetUserteamsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
