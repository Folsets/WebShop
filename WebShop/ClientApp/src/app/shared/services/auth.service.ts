import {Injectable, OnInit} from '@angular/core';
import { OidcClientNotification, OidcSecurityService, PublicConfiguration} from "angular-auth-oidc-client";
import { Observable } from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class AuthService implements OnInit{

  constructor(
      public oidcSecurityService: OidcSecurityService
    ) {
  }

  login() {
    this.oidcSecurityService.authorize();
  }

  logout() {
    this.oidcSecurityService.logoff();
  }

  checkAuth()  {
    this.oidcSecurityService.checkAuth().subscribe((auth) => {
      console.log("isAuthenticated? : " + auth);
    });
  }

  ngOnInit() {
  }
}
