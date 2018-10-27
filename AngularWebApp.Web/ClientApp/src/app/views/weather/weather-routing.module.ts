import { NgModule } from '@angular/core';
import { Routes,
     RouterModule } from '@angular/router';

import { WeatherComponent } from './weather.component';

const routes: Routes = [
  {
    path: '',
    component: WeatherComponent,
    data: {
      title: 'Weather Forecasts'
    }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class WeatherRoutingModule {}
