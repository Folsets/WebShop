import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Constants} from "../shared/constants";


@Injectable({providedIn: 'root'})
export class UserService {
  constructor(private http: HttpClient) {

  }

  getAllUsers() {
    var users = this.http.get(Constants.apiRoot + "/Clients", {}).subscribe(value => {
      return value;
    });
    return users;
  }
}
