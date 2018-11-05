import { Component } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { BsModalService } from 'ngx-bootstrap/modal';
import { RolesService, Role } from '../../../services/roles.service';
import { EditRolePopupComponent } from './edit-role-popup/edit-role-popup.component';

@Component({
  selector: 'app-roles',
  templateUrl: './roles.component.html'
})
export class RolesComponent {
  public roles: Role[] = [];

  constructor(private rolesService: RolesService, private toastr: ToastrService,
    private modalService: BsModalService) {
  }

  ngOnInit() {
    this.refresh();
  }

  refresh() {
    this.rolesService.list()
      .subscribe((response: Role[]) => {
        this.roles = response;
      }, err => {
        console.log(err);
        this.toastr.error("Error retrieving data");
      });
  }

  showEditRolePopup(roleId: string) {
    var modalRef = this.modalService.show(EditRolePopupComponent, { initialState: { roleId: roleId } });
    modalRef.content.onCommand.subscribe((result: string) => {
      if (result === "ok") {
          this.refresh();
      }
    });
  }

  deleteRole(index, roleId) {
    this.rolesService.delete(roleId)
      .subscribe((response) => {
        this.roles.splice(index, 1);
      }, err => {
        console.log(err);
        this.toastr.error("Error deleting role");
      });
  }
}
