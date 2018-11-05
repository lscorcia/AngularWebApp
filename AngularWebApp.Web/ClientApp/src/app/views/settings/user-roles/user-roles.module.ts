import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { CommonModule } from '@angular/common';

import { UserRolesComponent } from './user-roles.component';
import { UserRolesRoutingModule } from './user-roles-routing.module';
import { UserRolesService } from '../../../services/userroles.service';
import { ModalModule } from 'ngx-bootstrap/modal';
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
    UserRolesService
  ],
  entryComponents: [
    AddUserRolePopupComponent
  ]
})
export class UserRolesModule { }
