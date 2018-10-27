import { Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { Observable } from 'rxjs/Observable';

export class AuthInfo {
  public UserName: string; 
  public Email: string;
}

interface LoginResponse {
  accessToken: string;
  refreshToken: string;
}

@Injectable()
export class AuthenticationService {
  public AuthInfo: AuthInfo; 

  constructor(private http: HttpClient, private jwtHelper: JwtHelperService, 
    @Inject('BASE_URL') private baseUrl: string) {
    this.AuthInfo = this.buildAuthInfo(this.getAccessToken());
  }

  login(username: string, password: string) {
    return new Observable((observer) => {
      let credentials = { clientId: 'AngularWebApp.Web.Client', username: username, password: password };
      return this.http.post(this.baseUrl + "api/auth/login", credentials)
        .subscribe((response: LoginResponse) => {
        var accessToken = response.accessToken;
        var refreshToken = response.refreshToken;
        this.setSession(accessToken, refreshToken);

        observer.next(accessToken);
        observer.complete();
      }, err => {
        observer.error(err);
      });
    });
  }

  refresh() {
    return new Observable((observer) => {
      const oldAccessToken = this.getAccessToken();
      const oldRefreshToken = this.getRefreshToken();
      let credentials = { clientId: 'AngularWebApp.Web.Client', accessToken: oldAccessToken, refreshToken: oldRefreshToken };
      return this.http.post(this.baseUrl + "api/auth/refresh", credentials)
        .subscribe((response: LoginResponse) => {
        var accessToken = response.accessToken;
        var refreshToken = response.refreshToken;
        this.setSession(accessToken, refreshToken);

        observer.next(accessToken);
        observer.complete();
      }, err => {
        observer.error(err);
      });
    });
  }

  windowsLogin() {
    return new Observable((observer) => {
      let credentials = { clientId: 'AngularWebApp.Web.Client' };
      return this.http.post(this.baseUrl + "sso/windowsauth/login", credentials)
        .subscribe((response: LoginResponse) => {
          var accessToken = response.accessToken;
          var refreshToken = response.refreshToken;
          this.setSession(accessToken, refreshToken);

          observer.next(accessToken);
          observer.complete();
        }, err => {
          observer.error(err);
        });
    });
  }

  private setSession(accessToken, refreshToken) {
    localStorage.setItem('jwt', accessToken);
    localStorage.setItem('refreshToken', refreshToken);
    this.AuthInfo = this.buildAuthInfo(accessToken);
  }

  logout() {
    localStorage.removeItem("jwt");
    localStorage.removeItem("refreshToken");
  }

  public isLoggedIn() {
    const token = this.getAccessToken();

    // Check whether the token is expired
    return token && !this.jwtHelper.isTokenExpired(token);
  }

  private buildAuthInfo(token): AuthInfo {
    var authInfo = new AuthInfo();

    if (token) {
      var decodedContent = this.jwtHelper.decodeToken(token);
      console.log(decodedContent);

      authInfo.UserName = decodedContent['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'];
      authInfo.Email = decodedContent['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/email'];
    }

    return authInfo;
  }

  isLoggedOut() {
    return !this.isLoggedIn();
  }

  getAccessToken() {
    const token = localStorage.getItem('jwt');
    return token;
  }

  getRefreshToken() {
    const token = localStorage.getItem('refreshToken');
    return token;
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
        }).subscribe(() => {
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
