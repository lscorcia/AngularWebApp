import { Component } from '@angular/core';
import { OrdersService, Order } from '../../services/orders.service';

@Component({
  selector: 'orders',
  templateUrl: './orders.component.html'
})
export class OrdersComponent {
  public orders: Order[];

  constructor(private ordersService: OrdersService) {
  }

  ngOnInit() {
    this.refresh();
  }

  refresh() {
    this.orders = [];
    this.ordersService.list()
      .subscribe(response => {
        this.orders = response;
      });
  }

  delete(index, roleId) {
    this.ordersService.delete(roleId)
      .subscribe((response) => {
        this.orders.splice(index, 1);
      });
  }
}
