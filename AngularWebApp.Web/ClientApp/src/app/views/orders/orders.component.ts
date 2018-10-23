import { Component, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Component({
  selector: 'orders',
  templateUrl: './orders.component.html'
})
export class OrdersComponent {
  public orders: Order[];

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
  }

  ngOnInit() {
    let token = localStorage.getItem("jwt");
    this.http.get<Order[]>(this.baseUrl + "api/Orders/Get", {
      headers: new HttpHeaders({
        "Authorization": "Bearer " + token,
        "Content-Type": "application/json"
      })
    }).subscribe(response => {
      this.orders = response;
    }, err => {
      console.log(err);
    });
  }
}

interface Order {
  orderId: number;
  customerName: string;
  shipperCity: string;
  isShipped: boolean;
}
