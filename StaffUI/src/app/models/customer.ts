export class Customer {
    CustomerID: number;
    Firstname: string;
    Lastname: string;
    Telephone: string;
    Address: string;

    constructor(firstname: string, lastname: string, telephone: string, address: string){
        this.Firstname = firstname;
        this.Lastname = lastname;
        this.Telephone = telephone;
        this.Address = address;
    }
}