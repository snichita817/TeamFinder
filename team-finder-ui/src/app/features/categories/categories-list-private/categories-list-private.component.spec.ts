import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CategoriesListPrivateComponent } from './categories-list-private.component';

describe('CategoriesListPrivateComponent', () => {
  let component: CategoriesListPrivateComponent;
  let fixture: ComponentFixture<CategoriesListPrivateComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CategoriesListPrivateComponent]
    });
    fixture = TestBed.createComponent(CategoriesListPrivateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
