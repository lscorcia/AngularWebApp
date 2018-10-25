import { Component, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Component({
  selector: 'tokens',
  templateUrl: './tokens.component.html'
})
export class TokensComponent {
  public tokens: Token[] = [];

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
  }

  ngOnInit() {
    let token = localStorage.getItem("jwt");
    this.http.get<Token[]>(this.baseUrl + "api/RefreshTokens", {
      headers: new HttpHeaders({
        "Authorization": "Bearer " + token,
        "Content-Type": "application/json"
      })
    }).subscribe(response => {
      this.tokens = response;
    }, err => {
      console.log(err);
    });
  }

  deleteRefreshTokens(index, tokenid) {
    let token = localStorage.getItem("jwt");
    this.http.delete(this.baseUrl + 'api/RefreshTokens/?id=' + tokenid,
        {
          headers: new HttpHeaders({
            "Authorization": "Bearer " + token
          })
        })
    .subscribe(response => {
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
