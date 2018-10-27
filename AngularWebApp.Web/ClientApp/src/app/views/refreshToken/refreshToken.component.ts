import { Component } from '@angular/core';
import { Router } from "@angular/router";
import { AuthenticationService } from '../../services/authentication.service';

@Component({
  selector: 'app-refreshtoken',
  template: ``
})
export class RefreshTokenComponent {
  constructor(private router: Router, private authenticationService: AuthenticationService) { }

  ngOnInit() {
    this.authenticationService.refresh()
      .subscribe(() => this.router.navigate(["/"]));
  }
}
