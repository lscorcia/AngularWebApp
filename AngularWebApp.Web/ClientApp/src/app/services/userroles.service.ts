import { Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';

export class UserRole {
  username: string;
  role: string;
}

@Injectable()
export class UserRolesService {
  constructor(private http: HttpClient, 
    @Inject('BASE_URL') private baseUrl: string) {
  }

  list(): Observable<UserRole[]> {
    return new Observable((observer) => {
      return this.http.get(this.baseUrl + "api/userroles/list")
        .subscribe((response: UserRole[]) => {
            observer.next(response);
            observer.complete();
          },
          err => {
            observer.error(err);
          });
    });
  }

  add(role: string, userName: string) {
    var parameters = { userName: userName, role: role };

    return new Observable((observer) => {
      return this.http.post(this.baseUrl + "api/userroles/add", parameters)
        .subscribe((response) => {
          observer.next(response);
          observer.complete();
        }, err => {
          observer.error(err);
        });
    });
  }

  delete(roleName: string, userName: string) {
    return new Observable((observer) => {
      this.http.delete(this.baseUrl + 'api/userroles/delete/' + encodeURIComponent(roleName) + '/' + encodeURIComponent(userName))
        .subscribe((response) => {
          observer.next(response);
          observer.complete();
        }, err => {
          observer.error(err);
        });
    });
  }
}
