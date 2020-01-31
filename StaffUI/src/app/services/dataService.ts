import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { OrderHistory } from '../models/orderhistory';
import { ItemHistory } from '../models/itemhistory';
import { Customer } from '../models/customer';

@Injectable({
    providedIn: 'root'
  })

  export class DataServiceComponent {

    constructor(private httpClient: HttpClient){

    }

    public getAllCustomers(): Observable<Customer[]>{
        return this.httpClient.get<Customer[]>(`http://localhost:61998/api/customers/`);
        }

    public getOrderHistory(): Observable<OrderHistory[]>{
        return this.httpClient.get<OrderHistory[]>(`http://localhost:61998/api/order/history/`);
        }

    public getItemHistory(): Observable<ItemHistory[]>{
        return this.httpClient.get<ItemHistory[]>(`http://localhost:61998/api/order/itemhistory/`);
        }
        
    
  }
