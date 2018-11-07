import { Component } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { AuthenticationService, Token } from '../../../services/authentication.service';

@Component({
  selector: 'app-refresh-tokens',
  templateUrl: './refresh-tokens.component.html'
})
export class RefreshTokensComponent {
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
      });
  }

  deleteRefreshToken(index, tokenid) {
    this.authenticationService.deleteRefreshToken(tokenid)
      .subscribe(() => {
        this.tokens.splice(index, 1);
        this.toastr.success('Refresh token deleted');
      });
  }
}
