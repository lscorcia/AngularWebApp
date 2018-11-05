import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Observable } from 'rxjs/Observable';
import { UserRolesService, UserRole } from '../../../services/userroles.service';

@Component({
  selector: 'app-user-roles',
  templateUrl: './user-roles.component.html'
})
export class UserRolesComponent implements OnInit {
  public userroles: UserRole[] = [];

  constructor(private userRolesService: UserRolesService, private toastr: ToastrService) {
  }

  ngOnInit() {
    this.refresh();
  }

  refresh() {
    this.userRolesService.list()
      .subscribe((response: UserRole[]) => {
        this.userroles = response;
      }, err => {
        console.log(err);
        this.toastr.error("Error retrieving data");
      });
  }

  addRole(role: string, userName: string) {
    this.userRolesService.add(role, userName)
      .subscribe((response) => {
        this.refresh();
      }, err => {
        console.log(err);
        this.toastr.error("Error adding role");
      });
  }

  deleteRole(index: number, role: string, userName: string) {
    this.userRolesService.delete(role, userName)
      .subscribe((response) => {
        this.userroles.splice(index, 1);
      }, err => {
        console.log(err);
        this.toastr.error("Error deleting role");
      });
  }
}