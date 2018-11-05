import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';

import { CommonModule } from '@angular/common';

import { WeatherComponent } from './weather.component';
import { WeatherRoutingModule } from './weather-routing.module';
import { WeatherService } from "../../services/weather.service";

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    WeatherRoutingModule
  ],
  declarations: [WeatherComponent],
  providers: [
    WeatherService
  ]
})
export class WeatherModule { }
