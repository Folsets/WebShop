import { Routes, RouterModule } from '@angular/router';

import {ProductsComponent} from "./components/products/products.component";

const routes: Routes = [
	{ path: '', component: ProductsComponent },
	{ path: 'products', component: ProductsComponent },
	{ path: '**', redirectTo: '' }
];

export const routing = RouterModule.forRoot(routes);
