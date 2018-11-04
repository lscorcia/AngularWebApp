import { Component } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { WeatherService, WeatherForecast } from '../../services/weather.service';

@Component({
  selector: 'weather',
  templateUrl: './weather.component.html'
})
export class WeatherComponent {
  public forecasts: WeatherForecast[];

  constructor(private weatherService: WeatherService, private toastr: ToastrService) {
  }

  ngOnInit() {
    this.refreshData();
  }

  refreshData() {
    this.forecasts = [];
    this.weatherService.list()
      .subscribe(result => {
        this.forecasts = result;
      },
      err => {
        this.toastr.error('Error retrieving data');
      }
    );
  }
}
