import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuard } from './guards/auth-guard.service';

// Import Containers
import { DefaultLayoutComponent } from './containers';

import { P404Component } from './views/error/404.component';
import { P500Component } from './views/error/500.component';
import { LoginComponent } from './views/login/login.component';
import { LogoutComponent } from './views/logout/logout.component';
import { RefreshTokenComponent } from './views/refreshToken/refreshToken.component';
import { RegisterComponent } from './views/register/register.component';

export const routes: Routes = [
  {
    path: '',
    redirectTo: 'dashboard',
    pathMatch: 'full',
    canActivate: [AuthGuard]
  },
  {
    path: '404',
    component: P404Component,
    data: {
      title: 'Page 404'
    }
  },
  {
    path: '500',
    component: P500Component,
    data: {
      title: 'Page 500'
    }
  },
  {
    path: 'login',
    component: LoginComponent,
    data: {
      title: 'Login Page'
    }
  },
  {
    path: 'logout',
    component: LogoutComponent,
    data: {
      title: 'Logout Page'
    }
  },
  {
    path: 'refreshToken',
    component: RefreshTokenComponent,
    data: {
      title: 'Refresh Token Page'
    }
  },
  {
    path: 'register',
    component: RegisterComponent,
    data: {
      title: 'Register Page'
    }
  },
  {
    path: '',
    component: DefaultLayoutComponent,
    data: {
      title: 'Home'
    },
    children: [
      {
        path: 'base',
        loadChildren: './views/base/base.module#BaseModule'
      },
      {
        path: 'dashboard',
        loadChildren: './views/dashboard/dashboard.module#DashboardModule',
        canActivate: [AuthGuard]
      },
      {
        path: 'weather',
        loadChildren: './views/weather/weather.module#WeatherModule',
        canActivate: [AuthGuard]
      },
      {
        path: 'orders',
        loadChildren: './views/orders/orders.module#OrdersModule',
        canActivate: [AuthGuard]
      },
      {
        path: 'roles',
        loadChildren: './views/settings/roles/roles.module#RolesModule',
        canActivate: [AuthGuard]
      },
      {
        path: 'userRoles',
        loadChildren: './views/settings/user-roles/user-roles.module#UserRolesModule',
        canActivate: [AuthGuard]
      },
      {
        path: 'refreshTokens',
        loadChildren: './views/settings/refresh-tokens/refresh-tokens.module#RefreshTokensModule',
        canActivate: [AuthGuard]
      }
    ]
  }
];

@NgModule({
  imports: [ RouterModule.forRoot(routes) ],
  exports: [ RouterModule ]
})
export class AppRoutingModule {}
