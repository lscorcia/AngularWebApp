import { Component } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from "@angular/router";
import { AuthenticationService } from '../../services/authentication.service';

@Component({
  selector: 'app-login',
  templateUrl: 'login.component.html'
})
export class LoginComponent {
  loginMessage: string;

  constructor(private router: Router,
    private authenticationService: AuthenticationService) {
  }

  ngOnInit() {
    document.querySelector('body').classList.add('justify-content-center');
  }

  login(form: NgForm) {
    this.authenticationService.login(form.value.username, form.value.password)
      .subscribe(response => {
        this.loginMessage = null;
        this.router.navigate(["/"]);
      }, err => {
        if (err.status === 418)
          this.loginMessage = "Login failed: Invalid credentials";
        else {
          var errors = [];
          for (var fieldName in err.error) {
            if (err.error.hasOwnProperty(fieldName)) {
              errors.push(err.error[fieldName]);
            }
          }

          this.loginMessage = "Login failed: " + (errors.join(", ") || err.message);
        }
      });
  }

  windowsLogin() {
    this.authenticationService.windowsLogin()
      .subscribe(response => {
        this.loginMessage = null;
        this.router.navigate(["/"]);
      }, err => {
        var errors = [];
        for (var fieldName in err.error) {
          if (err.error.hasOwnProperty(fieldName)) {
            errors.push(err.error[fieldName]);
          }
        }

        this.loginMessage = "Login failed: " + (errors.join(", ") || err.message);
      });
  }

  ngOnDestroy() {
    document.querySelector('body').classList.remove('justify-content-center');
  }
}
