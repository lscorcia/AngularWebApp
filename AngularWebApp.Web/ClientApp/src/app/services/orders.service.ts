import { Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';

export interface Order {
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
}
