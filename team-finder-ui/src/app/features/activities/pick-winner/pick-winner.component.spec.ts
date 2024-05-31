import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PickWinnerComponent } from './pick-winner.component';

describe('PickWinnerComponent', () => {
  let component: PickWinnerComponent;
  let fixture: ComponentFixture<PickWinnerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PickWinnerComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(PickWinnerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
