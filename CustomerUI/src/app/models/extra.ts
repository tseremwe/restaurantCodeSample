export class Extra{
    ExtraID: number;
    Name: string;
    Price: number;
}

export class ExtraList{
    ItemID :number;
    ExtraID: number;
    Name: string;
    Price: number;

    constructor(itemID: number, extraID: number, name:string, price: number){
        this.ItemID = itemID;
        this.ExtraID = extraID;
        this.Name = name;
        this.Price = price;
    }

}