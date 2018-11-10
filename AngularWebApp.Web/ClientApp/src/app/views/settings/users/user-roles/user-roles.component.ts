import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { UserRolesService, UserRole } from '../../../../services/userroles.service';
import { BsModalService } from 'ngx-bootstrap/modal';
import { AddUserRolePopupComponent } from "./add-user-role-popup/add-user-role-popup.component";

@Component({
  selector: 'app-user-roles',
  templateUrl: './user-roles.component.html'
})
export class UserRolesComponent implements OnInit {
  public userroles: UserRole[] = [];

  constructor(private userRolesService: UserRolesService, private toastr: ToastrService,
    private modalService: BsModalService) {
  }

  ngOnInit() {
    this.refresh();
  }

  refresh() {
    this.userRolesService.list()
      .subscribe((response: UserRole[]) => {
        this.userroles = response;
      });
  }

  showAddUserRolePopup() {
    var modalRef = this.modalService.show(AddUserRolePopupComponent);
    modalRef.content.onCommand.subscribe((result: string) => {
      if (result === "ok") {
        this.refresh();
      }
    });
  }

  deleteRole(index: number, role: string, userName: string) {
    this.userRolesService.delete(role, userName)
      .subscribe((response) => {
        this.userroles.splice(index, 1);
      });
  }
}
