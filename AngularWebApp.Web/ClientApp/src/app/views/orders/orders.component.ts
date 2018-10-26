import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'orders',
  templateUrl: './orders.component.html'
})
export class OrdersComponent {
  public orders: Order[];

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
  }

  ngOnInit() {
    this.refreshData();
  }

  refreshData() {
    this.orders = [];
    this.http.get<Order[]>(this.baseUrl + "api/Orders/Get")
      .subscribe(response => {
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
