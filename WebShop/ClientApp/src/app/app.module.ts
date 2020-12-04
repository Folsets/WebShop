import {BrowserModule} from '@angular/platform-browser';
import {APP_INITIALIZER, NgModule} from '@angular/core';
import {FormsModule} from '@angular/forms';
import {HttpClientModule} from '@angular/common/http';

import {AppComponent} from './app.component';
import {NavMenuComponent} from './nav-menu/nav-menu.component';
import {FooterComponent} from './footer/footer.component';
import {ProductsComponent} from './components/products/products.component';
import {ProductDetailComponent} from './components/products/product-detail/product-detail.component';
import {ProductListComponent} from './components/products/product-list/product-list.component';
import {ProductItemComponent} from "./components/products/product-list/product-item/product-item.component";
import {ProductEditComponent} from './components/products/product-edit/product-edit.component';

import { AuthModule, LogLevel, OidcConfigService } from 'angular-auth-oidc-client';
import {routing} from './app.routing';
import {Constants} from "./shared/constants";

export function configureAuth(oidcConfigService: OidcConfigService) {
  return () =>
    oidcConfigService.withConfig({
      stsServer: Constants.idpAuthority,
      redirectUrl: Constants.clientRoot+"/signin-callback",
      postLogoutRedirectUri: Constants.clientRoot+"/signout-callback",
      clientId: Constants.client_id,
      scope: 'openid profile WebShop.Api',
      responseType: 'code',
      logLevel: LogLevel.Debug
    });
}

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    FooterComponent,
    ProductsComponent,
    ProductDetailComponent,
    ProductListComponent,
    ProductItemComponent,
    ProductEditComponent,
  ],
  imports: [
    BrowserModule.withServerTransition({appId: 'ng-cli-universal'}),
    HttpClientModule,
    FormsModule,
    routing,
    AuthModule.forRoot(),
  ],
  providers: [
    OidcConfigService,
    {
      provide: APP_INITIALIZER,
      useFactory: configureAuth,
      deps: [OidcConfigService],
      multi: true,
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule {
}
