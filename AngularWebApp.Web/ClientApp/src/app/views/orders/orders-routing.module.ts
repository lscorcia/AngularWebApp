import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { OrdersComponent } from './orders.component';
import { EditOrderComponent } from "./edit-order/edit-order.component";
import { AuthGuard } from "../../guards/auth-guard.service";

const routes: Routes = [
  {
    path: '',
    data: {
      title: 'Orders'
    },
    children: [
      {
        path: '',
        component: OrdersComponent,
        canActivate: [AuthGuard],
        data: {
          title: 'Order list'
        }
      },
      {
        path: 'add',
        component: EditOrderComponent,
        canActivate: [AuthGuard],
        data: {
          title: 'Add order'
        }
      },
      {
        path: ':orderid/edit',
        component: EditOrderComponent,
        canActivate: [AuthGuard],
        data: {
          title: 'Edit order'
        }
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class OrdersRoutingModule {}
