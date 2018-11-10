import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EditRolePopupComponent } from './edit-role-popup.component';

describe('EditRolePopupComponent', () => {
  let component: EditRolePopupComponent;
  let fixture: ComponentFixture<EditRolePopupComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EditRolePopupComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EditRolePopupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
