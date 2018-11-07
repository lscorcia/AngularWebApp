import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { RolesService, Role } from '../../../../services/roles.service';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { Subject } from 'rxjs/Subject';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-edit-role-popup',
  templateUrl: './edit-role-popup.component.html'
})
export class EditRolePopupComponent implements OnInit {
  public roleId: string;
  public onCommand: Subject<string>;
  editForm: FormGroup;

  constructor(private rolesService: RolesService, private toastr: ToastrService, private bsModalRef: BsModalRef, private fb: FormBuilder) {
  }

  ngOnInit() {
    this.editForm = this.fb.group({
      'role_id': [ this.roleId ],
      'name': ['', Validators.required]
    });
    this.onCommand = new Subject<string>();

    if (this.roleId) {
      this.rolesService.get(this.roleId)
        .subscribe((response: Role) => {
          this.editForm.setValue({ role_id: response.id, name: response.name });
        });
    }
  }

  onSubmit(): void {
    if (!this.editForm.valid) return;

    if (!this.editForm.value['role_id']) {
      var newRole = new Role();
      newRole.name = this.editForm.value['name'];

      this.rolesService.add(newRole)
        .subscribe((response) => {
            this.bsModalRef.hide();
            this.onCommand.next("ok");
          });
    } else {
      var editRole = new Role();
      editRole.id = this.editForm.value['role_id'];
      editRole.name = this.editForm.value['name'];

      this.rolesService.edit(editRole)
        .subscribe((response) => {
          this.bsModalRef.hide();
          this.onCommand.next("ok");
        });
    }
  }

  cancel(): void {
    this.bsModalRef.hide();
    this.onCommand.next("cancel");
  }
}
