import { NgModule } from '@angular/core';
import { Routes,
     RouterModule } from '@angular/router';

import { FetchDataComponent } from './fetch-data.component';

const routes: Routes = [
  {
    path: '',
    component: FetchDataComponent,
    data: {
      title: 'Fetch Data'
    }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class FetchDataRoutingModule {}
