import { Injectable } from '@angular/core';
import {ProductModel} from "./product.model";
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {Observable} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class ProductService {
  // dummyProducts: ProductModel[] = [
  //   { Id:1, Name:"Samsung Galaxy S5", Discount: 10,Category:0, Characteristics:["Battery: 5200Mhz", "Ekran: 0.3\""], Price: 260000, Photos: ["https://www.i-rite.com/wp-content/uploads/2016/12/samsung-galaxy-s5-repair-9.png", "https://external-content.duckduckgo.com/iu/?u=https%3A%2F%2Fupload.wikimedia.org%2Fwikipedia%2Fcommons%2Fc%2Fcf%2FSamsung_Galaxy_S5.png&f=1&nofb=1"]},
  //   { Id:2, Name:"One Plus 5", Category:0, Characteristics:[""], Price: 260000, Photos: ["https://www.i-rite.com/wp-content/uploads/2016/12/samsung-galaxy-s5-repair-9.png", "https://external-content.duckduckgo.com/iu/?u=https%3A%2F%2Fupload.wikimedia.org%2Fwikipedia%2Fcommons%2Fc%2Fcf%2FSamsung_Galaxy_S5.png&f=1&nofb=1"]},
  //   { Id:3, Name:"Xiaomi Redmi Note 8", Discount: 25,Category:0, Characteristics:[""], Price: 260000, Photos: ["https://www.i-rite.com/wp-content/uploads/2016/12/samsung-galaxy-s5-repair-9.png", "https://external-content.duckduckgo.com/iu/?u=https%3A%2F%2Fupload.wikimedia.org%2Fwikipedia%2Fcommons%2Fc%2Fcf%2FSamsung_Galaxy_S5.png&f=1&nofb=1"]},
  //   { Id:4, Name:"Xiaomi Redmi Note 7", Discount: 3,Category:0, Characteristics:[""], Price: 260000, Photos: ["https://www.i-rite.com/wp-content/uploads/2016/12/samsung-galaxy-s5-repair-9.png", "https://external-content.duckduckgo.com/iu/?u=https%3A%2F%2Fupload.wikimedia.org%2Fwikipedia%2Fcommons%2Fc%2Fcf%2FSamsung_Galaxy_S5.png&f=1&nofb=1"]},
  //   { Id:4, Name:"Xiaomi Redmi Note 7", Discount: 3,Category:0, Characteristics:[""], Price: 260000, Photos: ["https://www.i-rite.com/wp-content/uploads/2016/12/samsung-galaxy-s5-repair-9.png", "https://external-content.duckduckgo.com/iu/?u=https%3A%2F%2Fupload.wikimedia.org%2Fwikipedia%2Fcommons%2Fc%2Fcf%2FSamsung_Galaxy_S5.png&f=1&nofb=1"]},
  //   { Id:4, Name:"Xiaomi Redmi Note 7", Discount: 3,Category:0, Characteristics:[""], Price: 260000, Photos: ["https://www.i-rite.com/wp-content/uploads/2016/12/samsung-galaxy-s5-repair-9.png", "https://external-content.duckduckgo.com/iu/?u=https%3A%2F%2Fupload.wikimedia.org%2Fwikipedia%2Fcommons%2Fc%2Fcf%2FSamsung_Galaxy_S5.png&f=1&nofb=1"]},
  //   { Id:4, Name:"Xiaomi Redmi Note 7", Discount: 3,Category:0, Characteristics:[""], Price: 260000, Photos: ["https://www.i-rite.com/wp-content/uploads/2016/12/samsung-galaxy-s5-repair-9.png", "https://external-content.duckduckgo.com/iu/?u=https%3A%2F%2Fupload.wikimedia.org%2Fwikipedia%2Fcommons%2Fc%2Fcf%2FSamsung_Galaxy_S5.png&f=1&nofb=1"]},
  // ]

  constructor(private http: HttpClient) { }
  //
  // getProducts(): ProductModel[] {
  //   return this.dummyProducts;
  // }

  getAllProducts(): Observable<ProductModel[]> {
    return this.http.get<ProductModel[]>("https://localhost:5003/api/products");
  }
}
