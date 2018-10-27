import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'weather',
  templateUrl: './weather.component.html'
})
export class WeatherComponent {
  public forecasts: WeatherForecast[];

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
  }

  ngOnInit() {
    this.refreshData();
  }

  refreshData() {
    this.forecasts = [];
    this.http.get<WeatherForecast[]>(this.baseUrl + 'api/WeatherForecasts/List').subscribe(result => {
      this.forecasts = result;
    }, error => console.error(error));
  }
}

interface WeatherForecast {
  dateFormatted: string;
  temperatureC: number;
  temperatureF: number;
  summary: string;
}
