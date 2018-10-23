import { Component, Inject } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from "@angular/router";
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: 'login.component.html'
})
export class LoginComponent {
  loginMessage: string;

  constructor(private router: Router,
    private http: HttpClient,
    @Inject('BASE_URL') private baseUrl: string,
    private authService: AuthService) {
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
}
