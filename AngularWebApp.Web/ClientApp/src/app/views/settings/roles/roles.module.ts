import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';

import { CommonModule } from '@angular/common';

import { RolesComponent } from './roles.component';
import { RolesRoutingModule } from './roles-routing.module';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    RolesRoutingModule
  ],
  declarations: [ RolesComponent ]
})
export class RolesModule { }
