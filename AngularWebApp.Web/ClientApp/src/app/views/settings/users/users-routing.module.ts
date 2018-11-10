import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuard } from "../../../guards/auth-guard.service";

const routes: Routes = [
  {
    path: 'roles',
    loadChildren: './roles/roles.module#RolesModule',
    canActivate: [AuthGuard]
  },
  {
    path: 'userRoles',
    loadChildren: './user-roles/user-roles.module#UserRolesModule',
    canActivate: [AuthGuard]
  },
  {
    path: 'refreshTokens',
    loadChildren: './refresh-tokens/refresh-tokens.module#RefreshTokensModule',
    canActivate: [AuthGuard]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class UsersRoutingModule {}
