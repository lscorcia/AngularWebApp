import { Component, Inject, TemplateRef } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { RolesService } from '../../../services/roles.service';
import { ToastrService } from 'ngx-toastr';
import { BsModalService } from 'ngx-bootstrap/modal';
import { BsModalRef } from 'ngx-bootstrap/modal/bs-modal-ref.service';

@Component({
  selector: 'app-roles',
  templateUrl: './roles.component.html'
})
export class RolesComponent {
  public roles: Role[] = [];
  private modalRef: BsModalRef;

  constructor(private http: HttpClient, private rolesService: RolesService, private toastr: ToastrService,
    private modalService: BsModalService,
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

  openModal(template: TemplateRef<any>) {
    this.modalRef = this.modalService.show(template);
  }

  addRole(roleName) {
    this.rolesService.add(roleName)
      .subscribe((response) => {
        this.modalRef.hide();
        this.refresh();
      }, err => {
        console.log(err);
        this.toastr.error("Error adding role");
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
