import { Component } from '@angular/core';
import { BsModalService } from 'ngx-bootstrap/modal';
import { OrdersService, Order } from '../../services/orders.service';

@Component({
  selector: 'orders',
  templateUrl: './orders.component.html'
})
export class OrdersComponent {
  public orders: Order[];

  constructor(private ordersService: OrdersService,
    private modalService: BsModalService) {
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

  edit(orderId: number = null) {
    /*var modalRef = this.modalService.show(EditOrderPopupComponent, { initialState: { orderId: orderId } });
    modalRef.content.onCommand.subscribe((result: string) => {
      if (result === "ok") {
        this.refresh();
      }
    });*/
    if (orderId == null) {
      var order = new Order();
      order.customerName = 'Luca';
      order.isShipped = false;
      order.shipperCity = 'Bari';

      this.ordersService.add(order)
        .subscribe((response) => {
          this.refresh();
        });
    } else {
      var order = new Order();
      order.orderId = orderId;
      order.customerName = 'Chiara';
      order.isShipped = true;
      order.shipperCity = 'Bari';

      this.ordersService.add(order)
        .subscribe((response) => {
          this.refresh();
        });
    }
  }

  delete(index, roleId) {
    this.ordersService.delete(roleId)
      .subscribe((response) => {
        this.orders.splice(index, 1);
      });
  }
}
