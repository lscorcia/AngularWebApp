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
      }, err => {
        observer.error(err);
      });
    });
  }
}
