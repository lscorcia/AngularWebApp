import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { CommonModule } from '@angular/common';
import { DxDataGridModule } from 'devextreme-angular/ui/data-grid';

import { OrdersComponent } from './orders.component';
import { OrdersRoutingModule } from './orders-routing.module';
import { OrdersService } from "../../services/orders.service";
import { EditOrderComponent } from './edit-order/edit-order.component';
import { BackButtonDirective } from "../../back-button.directive";
import { OrdersDxgridComponent } from './orders-dxgrid/orders-dxgrid.component';

@NgModule({
  imports: [
    CommonModule,
    FormsModule, 
    ReactiveFormsModule,
    DxDataGridModule,
    OrdersRoutingModule
  ],
  declarations: [
    OrdersComponent,
    BackButtonDirective,
    EditOrderComponent,
    OrdersDxgridComponent
  ],
  providers: [
    OrdersService
  ]
})
export class OrdersModule { }
