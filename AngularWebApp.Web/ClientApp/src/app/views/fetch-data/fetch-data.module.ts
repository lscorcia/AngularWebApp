import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';

import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';

import { FetchDataComponent } from './fetch-data.component';
import { FetchDataRoutingModule } from './fetch-data-routing.module';

@NgModule({
  imports: [
    CommonModule,
    HttpClientModule,
    FormsModule,
    FetchDataRoutingModule
  ],
  declarations: [ FetchDataComponent ]
})
export class FetchDataModule { }
