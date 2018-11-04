import { Component } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { OrdersService, Order } from '../../services/orders.service';

@Component({
  selector: 'orders',
  templateUrl: './orders.component.html'
})
export class OrdersComponent {
  public orders: Order[];

  constructor(private ordersService: OrdersService, private toastr: ToastrService) {
  }

  ngOnInit() {
    this.refreshData();
  }

  refreshData() {
    this.orders = [];
    this.ordersService.list()
      .subscribe(response => {
        this.orders = response;
      }, err => {
        this.toastr.error('Error retrieving data');
      });
  }
}
