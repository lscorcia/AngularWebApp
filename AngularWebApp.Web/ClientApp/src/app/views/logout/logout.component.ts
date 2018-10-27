import { Component } from '@angular/core';
import { Router } from "@angular/router";
import { AuthenticationService } from '../../services/authentication.service';

@Component({
  selector: 'app-logout',
  template: ``
})
export class LogoutComponent {
  constructor(private router: Router, private authenticationService: AuthenticationService) { }

  ngOnInit() {
    this.authenticationService.logout();
    this.router.navigate(["/"]);
  }
}
