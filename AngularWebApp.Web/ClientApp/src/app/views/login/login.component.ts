import { Component } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from "@angular/router";
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: 'login.component.html'
})
export class LoginComponent {
  loginMessage: string;

  constructor(private router: Router,
    private authService: AuthService) {
  }

  ngOnInit() {
    document.querySelector('body').classList.add('justify-content-center');
  }

  login(form: NgForm) {
    this.authService.login(form.value.username, form.value.password)
      .subscribe(response => {
        this.loginMessage = null;
        this.router.navigate(["/"]);
      }, err => {
        this.loginMessage = "Credenziali errate";
      });
  }

  windowsLogin() {
    this.authService.windowsLogin()
      .subscribe(response => {
        this.loginMessage = null;
        this.router.navigate(["/"]);
      }, err => {
        this.loginMessage = "Login fallito";
      });
  }

  ngOnDestroy() {
    document.querySelector('body').classList.remove('justify-content-center');
  }
}
