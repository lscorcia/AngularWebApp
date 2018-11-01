import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-roles',
  templateUrl: './roles.component.html'
})
export class RolesComponent {
  public roles: Role[] = [];

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
  }

  ngOnInit() {
    this.refresh();
  }

  refresh() {
    this.roles = [];
    this.http.get<Role[]>(this.baseUrl + "api/Roles/List")
      .subscribe((response: Role[]) => {
        this.roles = response;
      }, err => {
        console.log(err);
      });
  }

  deleteRole(index, tokenid) {
    this.http.delete(this.baseUrl + 'api/Roles/Delete/?id=' + encodeURIComponent(tokenid))
      .subscribe(() => {
        this.roles.splice(index, 1);
      }, err => {
        console.log(err);
      });
  }
}

interface Role {
  name: string;
}
