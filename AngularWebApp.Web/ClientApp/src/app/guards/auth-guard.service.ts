import { JwtHelperService } from '@auth0/angular-jwt';
import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';

@Injectable()
export class AuthGuard implements CanActivate {
  constructor(private router: Router, public jwtHelper: JwtHelperService) {
  }
  canActivate() {
    var token = localStorage.getItem("jwt");

    if (token && !this.jwtHelper.isTokenExpired(token)){
      console.log(this.jwtHelper.decodeToken(token));
      return true;
    }
    this.router.navigate(["login"]);
    return false;
  }
}
