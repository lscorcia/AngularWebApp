import { NgModule } from '@angular/core';
import { Routes,
     RouterModule } from '@angular/router';

import { WeatherComponent } from './weather.component';

const routes: Routes = [
  {
    path: '',
    data: {
      title: 'Weather'
    },
    children: [
      {
        path: '',
        redirectTo: 'forecasts',
      },
      {
        path: 'forecasts',
        component: WeatherComponent,
        data: {
          title: 'Forecasts'
        }
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class WeatherRoutingModule {}
