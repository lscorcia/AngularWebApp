import { Component } from '@angular/core';
import { Router } from "@angular/router";
import { AuthenticationService } from '../../services/authentication.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-refreshtoken',
  template: ``
})
export class RefreshTokenComponent {
  constructor(private router: Router, private authenticationService: AuthenticationService, private toastr: ToastrService) { }

  ngOnInit() {
    this.authenticationService.refresh()
      .subscribe(() => {
        this.toastr.success('Access token refreshed');
        this.router.navigate(["/"]);
      }, (err) => {
        console.log(err);
        this.toastr.error('Error refreshing Access token');
        this.router.navigate(["/"]);
      });
  }
}
