import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { CommonModule } from '@angular/common';

import { RolesComponent } from './roles.component';
import { RolesRoutingModule } from './roles-routing.module';
import { RolesService } from '../../../../services/roles.service';
import { EditRolePopupComponent } from './edit-role-popup/edit-role-popup.component';
import { ModalModule } from 'ngx-bootstrap/modal';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    ModalModule.forRoot(),
    RolesRoutingModule
  ],
  declarations: [
    RolesComponent,
    EditRolePopupComponent
  ],
  providers: [
    RolesService
  ],
  entryComponents: [
    EditRolePopupComponent
  ]
})
export class RolesModule { }
