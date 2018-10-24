import { Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { Observable } from 'rxjs/Observable';

export class AuthInfo {
  public UserName: string; 
  public Email: string;
}

@Injectable()
export class AuthService {
  public AuthInfo: AuthInfo; 

  constructor(private http: HttpClient, public jwtHelper: JwtHelperService, 
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
        this.setSession(token);

        observer.next(token);
        observer.complete();
      }, err => {
        observer.error(err);
      });
    });
  }

  windowsLogin() {
    return new Observable((observer) => {
      return this.http.post(this.baseUrl + "api/auth/windowslogin", '')
        .subscribe(response => {
          var token = (<any>response).token;
          this.setSession(token);

          observer.next(token);
          observer.complete();
        }, err => {
          observer.error(err);
        });
    });
  }

  private setSession(token) {
    localStorage.setItem('jwt', token);
    this.AuthInfo = this.buildAuthInfo(token);
  }

  logout() {
    localStorage.removeItem("jwt");
  }

  public isLoggedIn() {
    const token = localStorage.getItem('jwt');

    // Check whether the token is expired
    return token && !this.jwtHelper.isTokenExpired(token);
  }

  private buildAuthInfo(token): AuthInfo {
    var authInfo = new AuthInfo();

    if (token) {
      var decodedContent = this.jwtHelper.decodeToken(token);
      authInfo.UserName = decodedContent['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'];
      authInfo.Email = decodedContent['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/email'];
    }

    return authInfo;
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
