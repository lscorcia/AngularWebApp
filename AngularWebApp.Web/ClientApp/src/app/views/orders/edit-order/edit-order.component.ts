import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { BsModalService } from 'ngx-bootstrap/modal';
import { OrdersService, Order } from '../../../services/orders.service';

@Component({
  selector: 'app-edit-order',
  templateUrl: './edit-order.component.html'
})
export class EditOrderComponent implements OnInit {
  private order: Order;

  constructor(private route: ActivatedRoute,
    private ordersService: OrdersService,
    private modalService: BsModalService) { }

  ngOnInit() {
    this.route.data
      .subscribe((data: { order: Order }) => {
        this.order = data.order;
      });
  }
}
