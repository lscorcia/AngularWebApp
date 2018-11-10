import { NgModule } from '@angular/core';
import { Routes,
     RouterModule } from '@angular/router';

import { RefreshTokensComponent } from './refresh-tokens.component';

const routes: Routes = [
  {
    path: '',
    component: RefreshTokensComponent,
    data: {
      title: 'Refresh Tokens'
    }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class RefreshTokensRoutingModule {}
