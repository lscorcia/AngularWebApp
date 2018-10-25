import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';

import { CommonModule } from '@angular/common';

import { FetchDataComponent } from './fetch-data.component';
import { FetchDataRoutingModule } from './fetch-data-routing.module';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    FetchDataRoutingModule
  ],
  declarations: [ FetchDataComponent ]
})
export class FetchDataModule { }
