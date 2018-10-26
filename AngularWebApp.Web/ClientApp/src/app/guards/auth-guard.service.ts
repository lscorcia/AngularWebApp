import { JwtHelperService } from '@auth0/angular-jwt';
import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { Observable } from 'rxjs/Observable';

@Injectable()
export class AuthGuard implements CanActivate {
  constructor(private router: Router, private jwtHelper: JwtHelperService, private authService: AuthService) {
  }
  canActivate(): Observable<boolean> | boolean {
    var token = this.authService.getAccessToken();
    if (token) {
      if (this.jwtHelper.isTokenExpired(token)) {
        return new Observable<boolean>((observer) => {
          this.authService.refresh().subscribe(() => {
            token = this.authService.getAccessToken();
            if (this.jwtHelper.isTokenExpired(token)) {
              this.router.navigate(["login"]);
              observer.next(false);
              observer.complete();
            } else {
              observer.next(true);
              observer.complete();
            }
          }, (err) => {
            this.router.navigate(["login"]);
            observer.next(false);
            observer.complete();
          });
        });
      } else {
        return true;
      }
    } else {
      this.router.navigate(["login"]);
      return false;
    }
  }
}
