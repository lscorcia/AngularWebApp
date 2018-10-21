import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'orders',
  templateUrl: './orders.component.html'
})
export class OrdersComponent {
  public orders: Order[];

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    http.get<Order[]>(baseUrl + 'api/Orders/Get').subscribe(result => {
      this.orders = result;
    }, error => console.error(error));
  }
}

interface Order {
  orderId: number;
  customerName: string;
  shipperCity: string;
  isShipped: boolean;
}
