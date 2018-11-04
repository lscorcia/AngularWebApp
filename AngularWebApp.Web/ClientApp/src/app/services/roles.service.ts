import { Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';

@Injectable()
export class RolesService {
  constructor(private http: HttpClient, 
    @Inject('BASE_URL') private baseUrl: string) {
  }

  list(): Observable<Role[]> {
    return new Observable((observer) => {
      return this.http.get(this.baseUrl + "api/roles/list")
        .subscribe((response: Role[]) => {
        observer.next(response);
        observer.complete();
      }, err => {
        observer.error(err);
      });
    });
  }

  add(role: Role) {
    var parameters = { Name: role.name };

    return new Observable((observer) => {
      return this.http.post(this.baseUrl + "api/roles/add", parameters)
        .subscribe((response) => {
          observer.next(response);
          observer.complete();
        }, err => {
          observer.error(err);
        });
    });
  }

  edit(role: Role) {
    var parameters = { Id: role.id, Name: role.name };

    return new Observable((observer) => {
      return this.http.post(this.baseUrl + "api/roles/edit", parameters)
        .subscribe((response) => {
          observer.next(response);
          observer.complete();
        }, err => {
          observer.error(err);
        });
    });
  }

  delete(roleId) {
    return new Observable((observer) => {
      this.http.delete(this.baseUrl + 'api/roles/delete/' + encodeURIComponent(roleId))
        .subscribe((response) => {
          observer.next(response);
          observer.complete();
        }, err => {
          observer.error(err);
        });
    });
  }
}

export class Role {
  id: string;
  name: string;
}
