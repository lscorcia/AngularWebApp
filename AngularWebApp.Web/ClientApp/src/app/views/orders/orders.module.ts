import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { CommonModule } from '@angular/common';

import { OrdersComponent } from './orders.component';
import { OrdersRoutingModule } from './orders-routing.module';
import { OrdersService } from "../../services/orders.service";
import { EditOrderComponent } from './edit-order/edit-order.component';
import { BackButtonDirective } from "../../back-button.directive";

@NgModule({
  imports: [
    CommonModule,
    FormsModule, 
    ReactiveFormsModule,
    OrdersRoutingModule
  ],
  declarations: [
    OrdersComponent,
    BackButtonDirective,
    EditOrderComponent
  ],
  providers: [
    OrdersService
  ]
})
export class OrdersModule { }
