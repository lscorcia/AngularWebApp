import { Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';

export class Order {
  orderId: number;
  customerName: string;
  shipperCity: string;
  isShipped: boolean;
}

@Injectable()
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

  get(id: number): Observable<Order> {
    return new Observable((observer) => {
      return this.http.get(this.baseUrl + "api/roles/get/" + id)
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
    var parameters = { Id: order.orderId, customerName: order.customerName, isShipped: order.isShipped, shipperCity: order.shipperCity };

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
