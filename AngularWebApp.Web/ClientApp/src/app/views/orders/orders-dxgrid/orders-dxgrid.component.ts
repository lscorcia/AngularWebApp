import { Component, ViewChild, OnInit, Query } from '@angular/core';
import { DxDataGridComponent } from 'devextreme-angular/ui/data-grid';
import { ToastrService } from "ngx-toastr";
import { OrdersService } from "../../../services/orders.service";

@Component({
  selector: 'app-orders-dxgrid',
  templateUrl: './orders-dxgrid.component.html'
})
export class OrdersDxgridComponent implements OnInit {
  @ViewChild(DxDataGridComponent) dataGrid: DxDataGridComponent;

  dataSource: any = {};

  constructor(private ordersService: OrdersService, private toastr: ToastrService) {
  }

  ngOnInit() {
    this.dataSource = this.ordersService.orders();
  }

  refreshData() {
    this.dataGrid.instance.refresh();
  }
}
