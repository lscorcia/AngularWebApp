import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';

import { CommonModule } from '@angular/common';

import { RolesComponent } from './roles.component';
import { RolesRoutingModule } from './roles-routing.module';
import { RolesService } from '../../../services/roles.service';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    RolesRoutingModule
  ],
  declarations: [RolesComponent],
  providers: [
    RolesService
  ]
})
export class RolesModule { }
