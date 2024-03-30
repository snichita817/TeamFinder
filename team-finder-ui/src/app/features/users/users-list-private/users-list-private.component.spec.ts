import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UsersListPrivateComponent } from './users-list-private.component';

describe('UsersListPrivateComponent', () => {
  let component: UsersListPrivateComponent;
  let fixture: ComponentFixture<UsersListPrivateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [UsersListPrivateComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(UsersListPrivateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
