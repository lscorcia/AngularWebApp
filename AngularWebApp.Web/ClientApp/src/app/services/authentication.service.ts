import { Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { Observable } from 'rxjs/Observable';
import { environment } from '../../environments/environment';

export class AuthInfo {
  public UserName: string; 
  public Email: string;
  public Roles: string[];
}

interface LoginResponse {
  accessToken: string;
  refreshToken: string;
}

export class RegisterResponse {
  savedSuccessfully: boolean;
  messages: string[];

  constructor(savedSuccessully: boolean, messages: string[] = []) {
    this.savedSuccessfully = savedSuccessully;
    this.messages = messages;
  }
}

export class Token {
  id: number;
  subject: string;
  clientId: string;
  issuedUtc: Date;
  expiresUtc: Date;
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
      let parameters = { clientId: environment.clientId, username: username, password: password };
      return this.http.post(this.baseUrl + "api/jwtauth/login", parameters)
        .subscribe((response: LoginResponse) => {
          var accessToken = response.accessToken;
          var refreshToken = response.refreshToken;
          this.setSession(accessToken, refreshToken);

          observer.next(accessToken);
          observer.complete();
        }, err => {
          this.logout();
          observer.error(err);
        });
    });
  }

  refresh() {
    return new Observable((observer) => {
      const oldAccessToken = this.getAccessToken();
      const oldRefreshToken = this.getRefreshToken();
      let parameters = { clientId: environment.clientId, accessToken: oldAccessToken, refreshToken: oldRefreshToken };
      return this.http.post(this.baseUrl + "api/jwtauth/refresh", parameters)
        .subscribe((response: LoginResponse) => {
          var accessToken = response.accessToken;
          var refreshToken = response.refreshToken;
          this.setSession(accessToken, refreshToken);

          observer.next(accessToken);
          observer.complete();
        }, err => {
          this.logout();
          observer.error(err);
        });
    });
  }

  windowsLogin() {
    return new Observable((observer) => {
      let parameters = { clientId: environment.clientId };
      return this.http.post(this.baseUrl + "sso/windowsauth/login", parameters)
        .subscribe((response: LoginResponse) => {
          var accessToken = response.accessToken;
          var refreshToken = response.refreshToken;
          this.setSession(accessToken, refreshToken);

          observer.next(accessToken);
          observer.complete();
        }, err => {
          this.logout();
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

      var roles = decodedContent['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
      authInfo.Roles = Array.isArray(roles || []) ? roles : [roles];
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

  register(email: string, username: string, password: string, confirmPassword: string): Observable<RegisterResponse> {
    return new Observable((observer) => {
      this.http.post(this.baseUrl + "api/jwtauth/register",
        {
          email: email,
          username: username,
          password: password,
          confirmPassword: confirmPassword
        }).subscribe(() => {
          observer.next(new RegisterResponse(true));
          observer.complete();
        },
        err => {
          if (err.error) {
            var errors = [];
            for (var fieldName in err.error) {
              if (err.error.hasOwnProperty(fieldName)) {
                errors.push(err.error[fieldName]);
              }
            }

            observer.error(new RegisterResponse(false, errors));
          }
          else
            observer.error(new RegisterResponse(false, err.message));
        });
    });
  }

  listRefreshTokens() {
    return new Observable<Token[]>((observer) => {
      this.http.get<Token[]>(this.baseUrl + "api/refreshtokens/list")
        .subscribe(response => {
            observer.next(response);
            observer.complete();
        },
        err => {
          console.log(err);
          observer.error(err);
        });
    });
  }

  deleteRefreshToken(tokenid) {
    return new Observable((observer) => {
      this.http.delete(this.baseUrl + 'api/RefreshTokens/Delete/' + encodeURIComponent(tokenid))
        .subscribe((response) => {
          observer.next(response);
          observer.complete();
        },
        err => {
          console.log(err);
          observer.error(err);
        });
    });
  }

  public isAdministrator(): boolean {
    return this.isInRole('administrators');
  }

  public isInRole(role: string) {
    return this.isInAnyRole([ role ]);
  }

  public isInAnyRole(roles: string[]) {
    if (!this.AuthInfo || !this.AuthInfo.Roles || this.AuthInfo.Roles.length === 0) return false;
    return this.AuthInfo.Roles.map(t => t.toLowerCase())
      .some(r => roles.map(t => t.toLowerCase()).includes(r));
  }
}
