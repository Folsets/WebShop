import { Injectable } from '@angular/core';

import { Product } from '../entities/product.entity';

@Injectable({providedIn: 'root'})
export class ProductService {
    private products: Product[];

    constructor() {
        this.products = [
            {id: 0, name: "name1", price: 100, photo: 'Sony_Xperia_X.png'},
            {id: 1, name: "name2", price: 140, photo: 'Sony_Xperia_X.png'},
            {id: 2, name: "name3", price: 250, photo: 'Sony_Xperia_X.png'}
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