import { Item, ItemTotal } from './item';

export class Order{
    OrderID: number;
    CustomerID: number;
    Items : ItemTotal[] = [];
    TotalPrice: number;
    Price: number;

    constructor(customerID: number, totalPrice: number, price: number){
    this.CustomerID = customerID;
    this.TotalPrice = totalPrice;
    this.Price = price;
    }
}