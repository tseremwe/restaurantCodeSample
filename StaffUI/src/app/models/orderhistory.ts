export class OrderHistory{
    OrderID: number;
    CustomerID: number;
    Completed: boolean;
    Cancelled: boolean;
    TotalPrice: number;
    Date: Date;
    MonthYear: String;
    Items : Item[] = [];
}

export class Item {
    ItemID: number;
    Name: string;
    Price: number;
    Extras: number;
}