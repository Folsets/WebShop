import { Component, OnInit } from "@angular/core";
import { ProductService } from "src/app/services/product.service";
import { Product } from '../../entities/product.entity';

@Component({
    templateUrl: './product.component.html'
})

export class ProductComponent implements OnInit {

    private products: Product[];

    constructor (private productService: ProductService) {
    }

    ngOnInit(): void {
        this.products = this.productService.findAll();
    }
}