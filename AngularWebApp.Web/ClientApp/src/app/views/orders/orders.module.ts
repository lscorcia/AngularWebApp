import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';

import { CommonModule } from '@angular/common';

import { OrdersComponent } from './orders.component';
import { OrdersRoutingModule } from './orders-routing.module';
import { OrdersService } from "../../services/orders.service";

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    OrdersRoutingModule,
  ],
  declarations: [OrdersComponent],
  providers: [
    OrdersService
  ]
})
export class OrdersModule { }
