import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

import { ModalModule } from 'ngx-bootstrap/modal';

import { RolesService } from "../../../services/roles.service";
import { UserRolesService } from '../../../services/userroles.service';

import { UserRolesComponent } from './user-roles.component';
import { UserRolesRoutingModule } from './user-roles-routing.module';
import { AddUserRolePopupComponent } from './add-user-role-popup/add-user-role-popup.component';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    ModalModule.forRoot(),
    UserRolesRoutingModule
  ],
  declarations: [
    UserRolesComponent,
    AddUserRolePopupComponent
  ],
  providers: [
    RolesService,
    UserRolesService
  ],
  entryComponents: [
    AddUserRolePopupComponent
  ]
})
export class UserRolesModule { }
