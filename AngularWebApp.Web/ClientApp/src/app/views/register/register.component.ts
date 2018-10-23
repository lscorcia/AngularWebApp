import { Component, Inject } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from "@angular/router";
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Component({
  selector: 'app-register',
  templateUrl: 'register.component.html'
})
export class RegisterComponent {
  registerMessage: string;
  savedSuccessfully: boolean;

  constructor(private router: Router, private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
  }

  register(form: NgForm) {
    let credentials = JSON.stringify(form.value);
    this.http.post(this.baseUrl + "api/auth/register", credentials, {
      headers: new HttpHeaders({
        "Content-Type": "application/json"
      })
    }).subscribe(response => {
      this.savedSuccessfully = true;
      this.registerMessage = "User has been registered successfully, you will be redicted to login page in 2 seconds.";
    }, err => {
      this.savedSuccessfully = false;
      var errors = [];
      for (var key in err.error) {
        for (var i = 0; i < err.error[key].length; i++) {
          errors.push(err.error[key][i]);
        }
      }
      this.registerMessage = "Failed to register user due to:" + errors.join(' ');
    });
  }
}
