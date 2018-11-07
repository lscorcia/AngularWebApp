import { HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';

@Injectable()
export class HttpErrorInterceptor implements HttpInterceptor {
  constructor(private toastr: ToastrService) {
  }

  intercept (req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(req).pipe(
      catchError((err) => {
        const errorResponse = err as HttpErrorResponse;
        if (errorResponse.status !== 418) {
          if (errorResponse && errorResponse.error) {
            this.toastr.error('Error in API call: ' + errorResponse.error.message);
          }
          else if (errorResponse.message) {
            this.toastr.error('Error in API call: ' + errorResponse.message);
          }
        }

        return throwError(errorResponse);
      }));
  }
}
