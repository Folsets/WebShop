import { Injectable } from '@angular/core';
import { OidcClientNotification, OidcSecurityService } from 'angular-auth-oidc-client';
import {BehaviorSubject, Observable} from 'rxjs';
import {HttpClient} from "@angular/common/http";

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  public isAuthenticated$;
  public userData$;

  constructor(public oidcSecurityService: OidcSecurityService, private http: HttpClient) {
    this.isAuthenticated$ = oidcSecurityService.isAuthenticated$;
    this.userData$ = oidcSecurityService.userData$;
  }

  login() {
    this.oidcSecurityService.authorize();
  }

  register() {

  }

  logout() {
    this.oidcSecurityService.logoff();
  }

}
