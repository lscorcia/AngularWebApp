import { Inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import CustomStore from 'devextreme/data/custom_store';

export class Order {
  orderId: number;
  customerName: string;
  shipperCity: string;
  isShipped: boolean;
}

@Injectable({
  providedIn: 'root'
})
export class OrdersService {
  constructor(private http: HttpClient, 
    @Inject('BASE_URL') private baseUrl: string) {
  }

  list(): Observable<Order[]> {
    return new Observable((observer) => {
      this.http.get<Order[]>(this.baseUrl + "api/Orders/List")
        .subscribe(response => {
          observer.next(response);
          observer.complete();
        });
    });
  }

  orders(): CustomStore {
    function isNotEmpty(value: any): boolean {
      return value !== undefined && value !== null && value !== "";
    }

    return new CustomStore({
      key: "orderId",
      load: (loadOptions) => {
        let params: HttpParams = new HttpParams();
        [
          "skip",
          "take",
          "requireTotalCount",
          "requireGroupCount",
          "sort",
          "filter",
          "totalSummary",
          "group",
          "groupSummary"
        ].forEach(function (i) {
          if (i in loadOptions && isNotEmpty(loadOptions[i]))
            params = params.set(i, JSON.stringify(loadOptions[i]));
        });
        return this.http.get(this.baseUrl + "api/Orders/Orders", { params: params })
          .toPromise<any>()
          .then(result => {
            return {
              data: result.data,
              totalCount: result.totalCount,
              summary: result.summary,
              groupCount: result.groupCount
            };
          });
      }
    });
  }

  get(id: number): Observable<Order> {
    return new Observable((observer) => {
      return this.http.get(this.baseUrl + "api/orders/get/" + id)
        .subscribe((response: Order) => {
          observer.next(response);
          observer.complete();
        }, err => {
          observer.error(err);
        });
    });
  }

  add(order: Order) {
    var parameters = { customerName: order.customerName, isShipped: order.isShipped, shipperCity: order.shipperCity };

    return new Observable((observer) => {
      return this.http.post(this.baseUrl + "api/orders/add", parameters)
        .subscribe((response) => {
          observer.next(response);
          observer.complete();
        }, err => {
          observer.error(err);
        });
    });
  }

  edit(order: Order) {
    var parameters = { orderId: order.orderId, customerName: order.customerName, isShipped: order.isShipped, shipperCity: order.shipperCity };

    return new Observable((observer) => {
      return this.http.post(this.baseUrl + "api/orders/edit", parameters)
        .subscribe((response) => {
          observer.next(response);
          observer.complete();
        }, err => {
          observer.error(err);
        });
    });
  }

  delete(orderId) {
    return new Observable((observer) => {
      this.http.delete(this.baseUrl + 'api/orders/delete/' + encodeURIComponent(orderId))
        .subscribe((response) => {
          observer.next(response);
          observer.complete();
        }, err => {
          observer.error(err);
        });
    });
  }
}
