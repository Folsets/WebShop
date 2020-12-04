import { Injectable } from '@angular/core';

import { Product } from '../entities/product.entity';

@Injectable({providedIn: 'root'})
export class ProductService {
    private products: Product[];
    private url = "https://www.bellmts.ca/file_source/mts/assets/img/wireless_devices/Sony_Xperia_XZ1@2x.png";
    constructor() {
        this.products = [
            {id: 0, name: "name1", price: 100, photo: this.url},
            {id: 1, name: "name2", price: 140, photo: this.url},
            {id: 2, name: "name3", price: 250, photo: this.url}
        ]
    }

    findAll(): Product[] {
        return this.products;
    }

    find(id: number): Product {
        return this.products[this.getSelectedIndex(id)];
    }

    private getSelectedIndex(id: number) {
        for (var i=0; i < this.products.length; i++) {
            if (this.products[i].id == id) {
                return i;
            }
        }
        return -1;
    }
}
