import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';

import { CommonModule } from '@angular/common';

import { OrdersComponent } from './orders.component';
import { OrdersRoutingModule } from './orders-routing.module';
import { OrdersService } from "../../services/orders.service";
import { EditOrderComponent } from './edit-order/edit-order.component';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    OrdersRoutingModule
  ],
  declarations: [
    OrdersComponent,
    EditOrderComponent
  ],
  providers: [
    OrdersService
  ]
})
export class OrdersModule { }
