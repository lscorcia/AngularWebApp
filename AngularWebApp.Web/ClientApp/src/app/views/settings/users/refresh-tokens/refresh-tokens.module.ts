import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';

import { CommonModule } from '@angular/common';

import { RefreshTokensComponent } from './refresh-tokens.component';
import { RefreshTokensRoutingModule } from './refresh-tokens-routing.module';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    RefreshTokensRoutingModule
  ],
  declarations: [ RefreshTokensComponent ]
})
export class RefreshTokensModule { }
