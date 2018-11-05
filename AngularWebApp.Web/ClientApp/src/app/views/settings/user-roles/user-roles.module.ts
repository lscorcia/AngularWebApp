import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { CommonModule } from '@angular/common';

import { UserRolesComponent } from './user-roles.component';
import { UserRolesRoutingModule } from './user-roles-routing.module';
import { UserRolesService } from '../../../services/userroles.service';
import { ModalModule } from 'ngx-bootstrap/modal';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    ModalModule.forRoot(),
    UserRolesRoutingModule
  ],
  declarations: [
    UserRolesComponent
  ],
  providers: [
    UserRolesService
  ]
})
export class UserRolesModule { }
