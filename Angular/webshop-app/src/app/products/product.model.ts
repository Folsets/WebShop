export interface ProductModel {
  id: number,
  name: string,
  price: number,
  category: number,
  characteristics: string[],
  discount?: number,
  discountEnds?: number,
  photos: string[]
}
