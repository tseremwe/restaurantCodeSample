import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Item } from '../models/item';
import { Observable } from 'rxjs';
import { Extra } from '../models/extra';
import { Customer } from '../models/customer';
import { Order } from '../models/order';

@Injectable({
    providedIn: 'root'
  })

export class DataServiceComponent {

    constructor(private httpClient: HttpClient) { }

    public getAllItems(): Observable<Item[]>{
        return this.httpClient.get<Item[]>(`http://localhost:61998/api/items`);
      }

    public getExtras(ItemID: number): Observable<Extra[]>{
    return this.httpClient.get<Extra[]>(`http://localhost:61998/api/items/extras/ `+ItemID);
    }

    public addCustomer(customer: Customer): Observable<Customer>{
        return this.httpClient.post<Customer>(`http://localhost:61998/api/addcustomer`, customer);
    }

    public completeOrder(order: Order): Observable<Order>{
        return this.httpClient.post<Order>(`http://localhost:61998/api/completeorder`, order);
    }
}
