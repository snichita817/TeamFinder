import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UpdateGetComponent } from './update-get.component';

describe('UpdateGetComponent', () => {
  let component: UpdateGetComponent;
  let fixture: ComponentFixture<UpdateGetComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [UpdateGetComponent]
    });
    fixture = TestBed.createComponent(UpdateGetComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
