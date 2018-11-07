import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { UserRolesService, UserRole } from '../../../../services/userroles.service';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { Subject } from 'rxjs/Subject';
import { ToastrService } from 'ngx-toastr';
import { RolesService } from "../../../../services/roles.service";

@Component({
  selector: 'app-add-user-role-popup',
  templateUrl: './add-user-role-popup.component.html'
})
export class AddUserRolePopupComponent implements OnInit {
  public roles: string[];
  public onCommand: Subject<string>;
  editForm: FormGroup;

  constructor(private userRolesService: UserRolesService, private rolesService: RolesService,
    private toastr: ToastrService, private bsModalRef: BsModalRef, private fb: FormBuilder) { }

  ngOnInit() {
    this.editForm = this.fb.group({
      'role': ['', Validators.required],
      'username': ['', Validators.required]
    });
    this.onCommand = new Subject<string>();

    this.rolesService.list().subscribe((response) => {
      this.roles = response.map(a => a.name);
    });
  }

  onSubmit(): void {
    if (!this.editForm.valid) return;

    var newRole = new UserRole();
    newRole.Role = this.editForm.value['role'];
    newRole.UserName = this.editForm.value['username'];

    this.userRolesService.add(newRole)
      .subscribe((response) => {
          this.bsModalRef.hide();
          this.onCommand.next("ok");
        });
  }

  cancel(): void {
    this.bsModalRef.hide();
    this.onCommand.next("cancel");
  }
}
