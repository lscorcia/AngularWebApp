import { Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';

export class UserRole {
  UserName: string;
  Role: string;
}

@Injectable()
export class UserRolesService {
  constructor(private http: HttpClient, 
    @Inject('BASE_URL') private baseUrl: string) {
  }

  list(): Observable<UserRole[]> {
    return new Observable<UserRole[]>((observer) => {
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

  add(userRole: UserRole): Observable<any> {
    return new Observable<any>((observer) => {
      return this.http.post(this.baseUrl + "api/userroles/add", userRole)
        .subscribe((response) => {
          observer.next(response);
          observer.complete();
        }, err => {
          observer.error(err);
        });
    });
  }

  delete(roleName: string, userName: string): Observable<any> {
    return new Observable<any>((observer) => {
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
