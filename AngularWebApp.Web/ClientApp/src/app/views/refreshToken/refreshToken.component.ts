import { Component } from '@angular/core';
import { Router } from "@angular/router";
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-refreshtoken',
  template: ``
})
export class RefreshTokenComponent {
  constructor(private router: Router, private authService: AuthService) { }

  ngOnInit() {
    this.authService.refresh()
      .subscribe(() => this.router.navigate(["/"]));
  }
}
