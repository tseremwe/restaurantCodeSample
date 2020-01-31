import { Extra } from './extra';

export class Item {
    ItemID: number;
    Name: string;
    Price: number;
    Extras: number;
}

export class ItemTotal {
    ItemID: number;
    Name: string;
    Price: number;
    TotalPrice: number;
    Extras: number;
    ExtraItems: Extra[];

    constructor(itemID: number, name: string, price: number, extras: number){
        this.ItemID  = itemID;
        this.Name = name;
        this.Price = price;
        this.TotalPrice = price;
        this.Extras = extras;
        this.ExtraItems = [];

    }
}