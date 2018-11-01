import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { RolesService } from '../../../services/roles.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-roles',
  templateUrl: './roles.component.html'
})
export class RolesComponent {
  public roles: Role[] = [];

  constructor(private http: HttpClient, private rolesService: RolesService, private toastr: ToastrService,
    @Inject('BASE_URL') private baseUrl: string) {
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

  deleteRole(index, roleId) {
    this.rolesService.delete(roleId)
      .subscribe((response: Role[]) => {
        this.roles = response;
        this.roles.splice(index, 1);
      }, err => {
        console.log(err);
        this.toastr.error("Error retrieving data");
      });
  }
}

interface Role {
  id: string,
  name: string;
}
