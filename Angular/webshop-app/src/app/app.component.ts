import {Component, OnDestroy, OnInit} from '@angular/core';
import {OidcSecurityService} from "angular-auth-oidc-client";
import {Subscription} from "rxjs";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit, OnDestroy{
  private checkAuthSub: Subscription;

  constructor(public oidc: OidcSecurityService) {}

  ngOnInit() {
    this.oidc.checkAuth().subscribe();
  }

  ngOnDestroy() {
    this.checkAuthSub.unsubscribe();
  }
}
