import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GetUserRequestsComponent } from './get-user-requests.component';

describe('GetUserRequestsComponent', () => {
  let component: GetUserRequestsComponent;
  let fixture: ComponentFixture<GetUserRequestsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [GetUserRequestsComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(GetUserRequestsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
