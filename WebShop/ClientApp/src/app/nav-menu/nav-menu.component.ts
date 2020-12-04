import {Component, OnInit} from '@angular/core';
import {AuthService} from "../shared/services/auth.service";
import {UserService} from "../services/user.service.";

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent implements OnInit{
  private isAuthenticated;
  private userAvatarPath = "/assets/images/avatar.png";
  constructor(private _authService: AuthService, private userService: UserService) {
  }

  login() {
    this._authService.login();
  }

  register() {

  }

  logout() {
    this._authService.logout();
  }

  pizdec() {
    this._authService.checkAuth();
  }

  ngOnInit(): void {
    this.isAuthenticated = false;
  }



}
