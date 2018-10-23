import { Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { JwtHelper } from 'angular2-jwt';
import { Observable } from 'rxjs/Observable';

@Injectable()
export class AuthService {
  constructor(private http: HttpClient, private jwtHelper: JwtHelper,
    @Inject('BASE_URL') private baseUrl: string) {

  }

  login(username: string, password: string) {
    return new Observable((observer) => {
      let credentials = JSON.stringify({ 'username': username, 'password': password });
      return this.http.post(this.baseUrl + "api/auth/login", credentials, {
        headers: new HttpHeaders({
          "Content-Type": "application/json"
        })
      }).subscribe(response => {
        var token = (<any>response).token;
        this.setSession(username, token);

        observer.next(token);
        observer.complete();
      }, err => {
        observer.error(err);
      });
    });
  }

  private setSession(userName, token) {
    localStorage.setItem('userName', userName);
    localStorage.setItem('jwt', token);
    console.log(this.jwtHelper.decodeToken(token));
  }

  logout() {
    localStorage.removeItem('userName');
    localStorage.removeItem("jwt");
  }

  public isLoggedIn() {
    const token = localStorage.getItem('jwt');

    // Check whether the token is expired and return
    // true or false
    return token && !this.jwtHelper.isTokenExpired(token);
  }

  getUserName() {
    return localStorage.getItem('userName');
  }

  isLoggedOut() {
    return !this.isLoggedIn();
  }

  register(email: string, username: string, password: string, confirmPassword: string) {
    return new Observable((observer) => {
      let credentials = JSON.stringify({
        'email': email,
        'username': username,
        'password': password,
        'confirmPassword': confirmPassword
      });
      this.http.post(this.baseUrl + "api/auth/register",
        credentials,
        {
          headers: new HttpHeaders({
            "Content-Type": "application/json"
          })
        }).subscribe(response => {
          observer.next({ savedSuccessfully: true });
          observer.complete();
        },
        err => {
          var errors = [];
          for (var key in err.error) {
            for (var i = 0; i < err.error[key].length; i++) {
              errors.push(err.error[key][i]);
            }
          }

          observer.error({ savedSuccessfully: false, registerMessage: "Failed to register user due to: " + errors.join(' ') });
        });
    });
  }
}
