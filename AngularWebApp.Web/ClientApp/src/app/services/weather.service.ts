import { Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';

@Injectable()
export class WeatherService {
  constructor(private http: HttpClient, 
    @Inject('BASE_URL') private baseUrl: string) {
  }

  list(): Observable<WeatherForecast[]> {
    return new Observable((observer) => {
      this.http.get<WeatherForecast[]>(this.baseUrl + "api/WeatherForecasts/List")
        .subscribe(response => {
          observer.next(response);
          observer.complete();
        }, err => {
          console.log(err);
          observer.error(err);
        });
    });
  }
}

export interface WeatherForecast {
  dateFormatted: string;
  temperatureC: number;
  temperatureF: number;
  summary: string;
}
