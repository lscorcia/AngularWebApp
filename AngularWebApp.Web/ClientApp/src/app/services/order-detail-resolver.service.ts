import { Injectable } from '@angular/core';
import {
  Router, Resolve,
  RouterStateSnapshot,
  ActivatedRouteSnapshot
} from '@angular/router';
import { Observable, of, EMPTY } from 'rxjs';
import { mergeMap, take } from 'rxjs/operators';

import { OrdersService, Order } from './orders.service';

@Injectable({
  providedIn: 'root',
})
export class OrderDetailResolverService implements Resolve<Order> {
  constructor(private ordersService: OrdersService, private router: Router) { }

  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<Order> | Observable<never> {
    let id = Number(route.paramMap.get('orderId'));

    return this.ordersService.get(id).pipe(
      take(1),
      mergeMap(order => {
        if (order) {
          return of(order);
        } else { // id not found
          this.router.navigate(['/orders']);
          return EMPTY;
        }
      })
    );
  }
}
