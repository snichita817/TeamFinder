import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UpdateListPrivateComponent } from './update-list-private.component';

describe('UpdateListPrivateComponent', () => {
  let component: UpdateListPrivateComponent;
  let fixture: ComponentFixture<UpdateListPrivateComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [UpdateListPrivateComponent]
    });
    fixture = TestBed.createComponent(UpdateListPrivateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
