import { Component } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from "@angular/router";
import { AuthenticationService, RegisterResponse } from '../../services/authentication.service';

@Component({
  selector: 'app-register',
  templateUrl: 'register.component.html'
})
export class RegisterComponent {
  registerMessage: string;
  savedSuccessfully: boolean;

  constructor(private router: Router, private authenticationService: AuthenticationService) {
  }

  ngOnInit() {
    document.querySelector('body').classList.add('justify-content-center');
  }

  register(form: NgForm) {
    this.registerMessage = "";
    this.authenticationService.register(form.value.email, form.value.username, form.value.password, form.value.confirmpassword)
      .subscribe((response: RegisterResponse) => {
        this.savedSuccessfully = true;
        this.registerMessage = "User has been registered successfully, you will be redirected to login page in 2 seconds.";

        setTimeout(() => {
          this.router.navigate(['/']);
        }, 2000); 
      }, (err: RegisterResponse) => {
        this.savedSuccessfully = false;
        this.registerMessage = "Registration failed: " + err.messages.join(", ");
      });
  }

  ngOnDestroy() {
    document.querySelector('body').classList.remove('justify-content-center');
  }
}
