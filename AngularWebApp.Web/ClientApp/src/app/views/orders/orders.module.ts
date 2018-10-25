import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';

import { CommonModule } from '@angular/common';

import { OrdersComponent } from './orders.component';
import { OrdersRoutingModule } from './orders-routing.module';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    OrdersRoutingModule
  ],
  declarations: [ OrdersComponent ]
})
export class OrdersModule { }
