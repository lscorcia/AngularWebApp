import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';

import { CommonModule } from '@angular/common';

import { WeatherComponent } from './weather.component';
import { WeatherRoutingModule } from './weather-routing.module';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    WeatherRoutingModule
  ],
  declarations: [ WeatherComponent ]
})
export class WeatherModule { }
