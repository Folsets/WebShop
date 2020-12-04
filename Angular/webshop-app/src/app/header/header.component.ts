import {Component, OnDestroy, OnInit} from '@angular/core';
import {AuthService} from "../auth/auth.service";
import {Subscription} from "rxjs";
import {OidcSecurityService} from "angular-auth-oidc-client";

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit, OnDestroy {
  public isAuthenticated : boolean;
  public userAvatarPath = "assets/images/avatar.png";
  public userData;
  private sub: Subscription;

  constructor(private authService: AuthService, public oidcSecurityService: OidcSecurityService) { }

  login() {
    this.authService.login();
  }

  logout() {
    this.authService.logout();
  }

  register() {
    this.authService.register();
  }

  ngOnInit(): void {
    this.sub = this.authService.isAuthenticated$.subscribe(isAuth => {
      this.isAuthenticated = isAuth;
    });
    this.authService.userData$.subscribe(userData => {
      this.userData = userData;
    });
  }

  ngOnDestroy() {
    this.sub.unsubscribe();
  }

}
