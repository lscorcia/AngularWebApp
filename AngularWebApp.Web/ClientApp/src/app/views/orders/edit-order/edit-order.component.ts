import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { BsModalService } from 'ngx-bootstrap/modal';
import { OrdersService, Order } from '../../../services/orders.service';
import { FormBuilder, FormGroup, Validators } from "@angular/forms";

@Component({
  selector: 'app-edit-order',
  templateUrl: './edit-order.component.html'
})
export class EditOrderComponent implements OnInit {
  private order: Order;
  editForm: FormGroup;

  constructor(private route: ActivatedRoute,
    private ordersService: OrdersService,
    private modalService: BsModalService,
    private fb: FormBuilder) { }

  ngOnInit() {
    this.route.data
      .subscribe((data: { order: Order }) => {
        this.order = data.order;
      });

    this.editForm = this.fb.group({
      'role': ['', Validators.required],
      'username': ['', Validators.required],
      'isShipped': [false]
    });
  }

  onSubmit() {
    if (!this.editForm.valid) return;

  }
}
