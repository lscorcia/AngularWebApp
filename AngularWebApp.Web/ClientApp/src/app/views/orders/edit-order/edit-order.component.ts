import { Component, OnInit } from '@angular/core';
import { Location } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { OrdersService, Order } from '../../../services/orders.service';
import { FormBuilder, FormGroup, Validators } from "@angular/forms";

@Component({
  selector: 'app-edit-order',
  templateUrl: './edit-order.component.html'
})
export class EditOrderComponent implements OnInit {
    private orderId: number;
    editForm: FormGroup;

  constructor(private route: ActivatedRoute,
    private router: Router,
    private location: Location,
    private ordersService: OrdersService,
    private fb: FormBuilder) { }

  ngOnInit() {
    this.route.data
      .subscribe((data: { order: Order }) => {
        var customerName: string;
        var shipperCity: string;
        var isShipped: boolean;

        if(data.order) {
          this.orderId = data.order.orderId;
          customerName = data.order.customerName;
          shipperCity = data.order.shipperCity;
          isShipped = data.order.isShipped;
        }

        this.editForm = this.fb.group({
          'customerName': [customerName, Validators.required],
          'shipperCity': [shipperCity, Validators.required],
          'isShipped': [isShipped]
        });
      });
  }

  onSubmit() {
    if (!this.editForm.valid) return;

    if (!this.orderId) {
      var newOrder = new Order();
      newOrder.customerName = this.editForm.value['customerName'];
      newOrder.shipperCity = this.editForm.value['shipperCity'];
      newOrder.isShipped = this.editForm.value['isShipped'] || false;

      this.ordersService.add(newOrder)
        .subscribe((response) => {
          this.location.back();
        });
    } else {
      var editOrder = new Order();
      editOrder.orderId = this.orderId;
      editOrder.customerName = this.editForm.value['customerName'];
      editOrder.shipperCity = this.editForm.value['shipperCity'];
      editOrder.isShipped = this.editForm.value['isShipped'] || false;

      this.ordersService.edit(editOrder)
        .subscribe((response) => {
          this.location.back();
        });
    }
  }
}
