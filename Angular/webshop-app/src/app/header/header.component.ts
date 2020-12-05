import {Component, OnDestroy, OnInit} from '@angular/core';
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
  private userDataSub: Subscription;
  private isAuthSub: Subscription;

  constructor(private oidc: OidcSecurityService) { }

  login() {
    this.oidc.authorize();
  }

  logout() {
    this.oidc.logoffAndRevokeTokens().subscribe(data => {
      console.log("After logout: ", data);
    });
  }

  register() {
    this.oidc.authorize({
      customParams: {"mode":"register"}
    });
  }

  ngOnInit(): void {
    this.userDataSub = this.oidc.userData$.subscribe(data => {
      this.userData = data;
    });
    this.oidc.isAuthenticated$.subscribe(isAuth => {
      if (this.isAuthenticated != isAuth){
        this.isAuthenticated = isAuth;
      }
    });
  }

  ngOnDestroy() {
    this.userDataSub.unsubscribe();
    this.isAuthSub.unsubscribe();
  }

}
