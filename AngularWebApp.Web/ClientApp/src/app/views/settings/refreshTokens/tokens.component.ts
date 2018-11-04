import { Component } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { AuthenticationService, Token } from '../../../services/authentication.service';

@Component({
  selector: 'app-refreshtokens',
  templateUrl: './tokens.component.html'
})
export class TokensComponent {
  public tokens: Token[] = [];

  constructor(private authenticationService: AuthenticationService, private toastr: ToastrService) {
  }

  ngOnInit() {
    this.refresh();
  }

  refresh() {
    this.tokens = [];
    this.authenticationService.listRefreshTokens()
      .subscribe((response: Token[]) => {
        this.tokens = response;
      }, err => {
        this.toastr.error('Error retrieving data');
      });
  }

  deleteRefreshToken(index, tokenid) {
    this.authenticationService.deleteRefreshToken(tokenid)
      .subscribe(() => {
        this.tokens.splice(index, 1);
        this.toastr.success('Refresh token deleted');
      }, err => {
        this.toastr.error('Error deleting refresh token');
      });
  }
}
