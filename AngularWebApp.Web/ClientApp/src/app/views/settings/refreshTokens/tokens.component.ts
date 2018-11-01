import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-refreshtokens',
  templateUrl: './tokens.component.html'
})
export class TokensComponent {
  public tokens: Token[] = [];

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string, private toastr: ToastrService) {
  }

  ngOnInit() {
    this.refresh();
  }

  refresh() {
    this.tokens = [];
    this.http.get<Token[]>(this.baseUrl + "api/RefreshTokens/List")
      .subscribe(response => {
        this.tokens = response;
      }, err => {
        console.log(err);
      });
  }

  deleteRefreshTokens(index, tokenid) {
    this.http.delete(this.baseUrl + 'api/RefreshTokens/Delete/?id=' + encodeURIComponent(tokenid))
      .subscribe(() => {
        this.tokens.splice(index, 1);
        this.toastr.success('Refresh token deleted');
      }, err => {
        this.toastr.error('Error deleting refresh token');
        console.log(err);
      });
  }
}

interface Token {
  id: number;
  subject: string;
  clientId: string;
  issuedUtc: Date;
  expiresUtc: Date;
}
