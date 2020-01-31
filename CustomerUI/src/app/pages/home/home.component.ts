import { Component } from '@angular/core';
import { DataServiceComponent } from 'src/app/services/dataService'
import { Item, ItemTotal } from 'src/app/models/item';
import { Extra, ExtraList } from 'src/app/models/extra';
import { Customer } from 'src/app/models/customer';
import { Order } from 'src/app/models/order';
import notify from 'devextreme/ui/notify';
import DataSource from 'devextreme/data/data_source';
import ArrayStore from 'devextreme/data/array_store';

@Component({
  templateUrl: 'home.component.html',
  styleUrls: [ './home.component.scss' ]
})

export class HomeComponent {
  items: Item[] = [];
  extras: Extra[] = [];
  extrasList: ExtraList[] = [];
  extraListItem: ExtraList;
  customer: Customer;
  showItems: boolean = false;
  showExtras: boolean = false;
  showPrice: boolean = false;
  showOrder: boolean = false;
  orderCompleted: boolean = false;
  itemPrice: number = 0;
  selectedExtras: Extra[] = [];
  selectedItems:  ItemTotal[] =[];
  currentItem: ItemTotal;
  currentOrder: Order;
  returnedOrder: Order;
  totalPrice: number = 0;
  price: number = 0;
  tasksData: any;

  constructor(private dataService: DataServiceComponent ) {
    this.dataService.getAllItems().subscribe(items => {
      this.items = items;
      console.log(this.items)
    });
    this.tasksData = [];
    
  }

  onFormSubmit(e, firstname, lastname, telephone, address){
    console.log(e);
    this.customer = new Customer(firstname, lastname,telephone, address)
    e.preventDefault();
      this.dataService.addCustomer(this.customer).subscribe(customer => {
      this.customer = customer;
      console.log(this.customer.Firstname);
      console.log(this.customer.CustomerID);
      this.showExtras = false;
      this.showItems = true;
    });
  }
  CurrencyFormatter(cellInfo) {
    return "$" + cellInfo.value.toFixed(2).replace(/(\d)(?=(\d{3})+(?:.\d+)?$)/g, "$1,")
    }

    SummaryFormatter(cellInfo) {
      return "Total Including Tax: $" + cellInfo.value.toFixed(2).replace(/(\d)(?=(\d{3})+(?:.\d+)?$)/g, "$1,")
      }

  itemSelected(e){
    console.log(e);
    if(e.selectedItem.Extras){
      this.showExtras = true; 
      //go and get the extras and assign them to extras
      this.dataService.getExtras(e.selectedItem.ItemID).subscribe(extras => {
        this.extras = extras;
      });
    } else{
      this.showExtras = false;
    }
    this.selectedExtras = [];
    this.currentItem = new ItemTotal(e.selectedItem.ItemID, e.selectedItem.Name, e.selectedItem.Price, e.selectedItem.Extras)
    this.showPrice = true;
    this.itemPrice = e.selectedItem.Price;
  }

  extrasChanged(e){
    if(e.addedItems.length > 0){
      this.addExtra(e.addedItems[0])
    }
    if(e.removedItems.length > 0){
      this.removeExtra(e.removedItems[0])
    }
    console.log(this.selectedExtras);
  }

  //add the extra to this item
  addExtra(item : Extra){
    this.itemPrice = this.itemPrice + item.Price
    this.selectedExtras.push(item);
    this.extraListItem = new ExtraList(this.currentItem.ItemID, item.ExtraID, item.Name, item.Price);
    this.extrasList.push(this.extraListItem);
  }

  //remove the extra from this item
  removeExtra(item : Extra){
    this.itemPrice = this.itemPrice - item.Price
    this.selectedExtras.splice( this.selectedExtras.indexOf(item), 1 );
    this.extraListItem = new ExtraList(this.currentItem.ItemID, item.ExtraID, item.Name, item.Price);
    this.extrasList.splice(this.extrasList.indexOf(this.extraListItem), 1);
  }

  addToOrder(){
    this.currentItem.ExtraItems = this.selectedExtras;
    this.currentItem.TotalPrice = this.itemPrice;
    this.selectedItems.push(this.currentItem);
    this.showOrder = true;
    console.log(this.extrasList)
    this.resetItems();
  }
  calculateTotalWithTax(options) {
    if (options.name === "TotalSummary") {
        if (options.summaryProcess === "start") {
            options.totalValue = 0;
        } else if (options.summaryProcess === "calculate") {
            options.totalValue = options.totalValue + options.value.TotalPrice*1.1;
        }
      }
  }

  getExtras(itemID){
    let extra  = this.tasksData.find((e) => e.key === itemID);
    if (!extra) {
      extra = {
          key: itemID,
          dataSourceInstance: new DataSource({
              store: new ArrayStore({
                  data: this.extrasList,
                  key: "ItemID"
              }),
              filter: ["ItemID", "=", itemID]
          })
      };
      this.tasksData.push(extra)
  }
  return extra.dataSourceInstance;
  }

  resetItems(){
    this.showExtras = false;
    this.selectedExtras = [];
    //this.extrasList = [];
    this.showPrice = false;
  }

  completeOrder(){
    for(var i = 0; i < this.selectedItems.length; i++){ //calc tax total
      this.totalPrice = this.totalPrice +this.selectedItems[i].TotalPrice*1.1;
      this.price = this.price + this.selectedItems[i].TotalPrice;
    }
    console.log(this.totalPrice)
    this.currentOrder = new Order(this.customer.CustomerID, this.totalPrice, this.price);
    this.currentOrder.Items = this.selectedItems;
    this.dataService.completeOrder(this.currentOrder).subscribe(returnedOrder => {
     this.returnedOrder = returnedOrder;
     this.orderCompleted=true; //reset the form
     notify("Your Order Has Been Created", "success", 2000);
      console.log(this.returnedOrder)
    });
  }

  cancelOrder(){//ask to confirm
    var result = confirm("Are you sure you would like to cancel this order?");  

  }
}
