import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'refreshtokens',
  templateUrl: './tokens.component.html'
})
export class TokensComponent {
  public tokens: Token[] = [];

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
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
      }, err => {
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
