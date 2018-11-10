import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AddUserRolePopupComponent } from './add-user-role-popup.component';

describe('AddUserRolePopupComponent', () => {
  let component: AddUserRolePopupComponent;
  let fixture: ComponentFixture<AddUserRolePopupComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AddUserRolePopupComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AddUserRolePopupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
