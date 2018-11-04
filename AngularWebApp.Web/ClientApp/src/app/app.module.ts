import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { LocationStrategy, HashLocationStrategy } from '@angular/common';
import { JWT_OPTIONS, JwtInterceptor, JwtModule } from '@auth0/angular-jwt';
import { AuthenticationService } from './services/authentication.service';
import { RefreshTokenInterceptor } from './services/refresh-token-interceptor';
import { OrdersService } from './services/orders.service';
import { WeatherService } from './services/weather.service';
import { AuthGuard } from './guards/auth-guard.service';
import { Injector } from '@angular/core';
import { parse } from 'url';
import { NgProgressModule } from '@ngx-progressbar/core';
import { NgProgressHttpModule } from '@ngx-progressbar/http';

// Perfect Scrollbar
import { PerfectScrollbarModule } from 'ngx-perfect-scrollbar';
import { PERFECT_SCROLLBAR_CONFIG } from 'ngx-perfect-scrollbar';
import { PerfectScrollbarConfigInterface } from 'ngx-perfect-scrollbar';

const DEFAULT_PERFECT_SCROLLBAR_CONFIG: PerfectScrollbarConfigInterface = {
  suppressScrollX: true
};

// ngxToaster
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ToastrModule } from 'ngx-toastr';

import { AppComponent } from './app.component';

// Import containers
import { DefaultLayoutComponent } from './containers';

import { P404Component } from './views/error/404.component';
import { P500Component } from './views/error/500.component';
import { LoginComponent } from './views/login/login.component';
import { LogoutComponent } from './views/logout/logout.component';
import { RefreshTokenComponent } from './views/refreshToken/refreshToken.component';
import { RegisterComponent } from './views/register/register.component';

const APP_CONTAINERS = [
  DefaultLayoutComponent
];

import {
  AppAsideModule,
  AppBreadcrumbModule,
  AppHeaderModule,
  AppFooterModule,
  AppSidebarModule,
} from '@coreui/angular';

// Import routing module
import { AppRoutingModule } from './app.routing';

// Import 3rd party components
import { ModalModule } from 'ngx-bootstrap/modal';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { ChartsModule } from 'ng2-charts/ng2-charts';

// JwtHelper cruft
export function jwtOptionsFactory(injector: Injector) {
  var baseUrl = injector.get('BASE_URL');
  var requestUrl = parse(baseUrl, false, true);

  return {
    tokenGetter: () => { return localStorage.getItem('jwt'); },
    whitelistedDomains: [requestUrl.host],
    blacklistedRoutes: [
      new RegExp(baseUrl + 'api/auth/'),
      new RegExp(baseUrl + 'sso/auth/')
    ]
  };
}

@NgModule({
  imports: [
    BrowserModule,
    AppRoutingModule,
    AppAsideModule,
    AppBreadcrumbModule.forRoot(),
    AppFooterModule,
    AppHeaderModule,
    AppSidebarModule,
    PerfectScrollbarModule,
    BsDropdownModule.forRoot(),
    ModalModule.forRoot(),
    TabsModule.forRoot(),
    ChartsModule,
    BrowserAnimationsModule, // required by ToastrModule
    ToastrModule.forRoot({ closeButton: true, positionClass: 'toast-top-center' }),
    NgProgressModule.forRoot({ spinner: false }),
    NgProgressHttpModule.forRoot(),
    JwtModule.forRoot({ jwtOptionsProvider: { provide: JWT_OPTIONS, useFactory: jwtOptionsFactory, deps: [Injector] } }),
    HttpClientModule,
    FormsModule
  ],
  declarations: [
    AppComponent,
    ...APP_CONTAINERS,
    P404Component,
    P500Component,
    LoginComponent,
    RefreshTokenComponent,
    LogoutComponent,
    RegisterComponent
  ],
  providers: [
    AuthenticationService,
    OrdersService,
    WeatherService,
    AuthGuard,
    JwtInterceptor,
    {
      provide: HTTP_INTERCEPTORS,
      useExisting: JwtInterceptor,
      multi: true
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: RefreshTokenInterceptor,
      multi: true
    },
    {
      provide: LocationStrategy,
      useClass: HashLocationStrategy
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
