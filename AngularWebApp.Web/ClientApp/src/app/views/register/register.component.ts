import { Component } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from "@angular/router";
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-register',
  templateUrl: 'register.component.html'
})
export class RegisterComponent {
  registerMessage: string;
  savedSuccessfully: boolean;

  constructor(private router: Router, private authService: AuthService) {
  }

  register(form: NgForm) {
    this.authService.register(form.value.email, form.value.username, form.value.password, form.value.confirmpassword)
      .subscribe((response) => {
        this.savedSuccessfully = (<any>response).savedSuccessfully;
        this.registerMessage = "User has been registered successfully, you will be redirected to login page in 2 seconds.";

        setTimeout(() => {
          this.router.navigate(['/']);
        }, 2000); 
      }, (err) => {
        this.savedSuccessfully = (<any>err).savedSuccessfully;
        this.registerMessage = (<any>err).registerMessage;
      });
  }
}
