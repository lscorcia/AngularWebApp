import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';

import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';

import { TokensComponent } from './tokens.component';
import { TokensRoutingModule } from './tokens-routing.module';

@NgModule({
  imports: [
    CommonModule,
    HttpClientModule,
    FormsModule,
    TokensRoutingModule
  ],
  declarations: [ TokensComponent ]
})
export class TokensModule { }
