import { Component, ViewChild, AfterViewInit } from '@angular/core';
import { DataServiceComponent } from 'src/app/services/dataService'
import { OrderHistory } from 'src/app/models/orderhistory';
import { ItemHistory } from 'src/app/models/itemhistory';
import { DxPivotGridComponent, DxChartComponent } from 'devextreme-angular';
import { Customer } from 'src/app/models/customer';

@Component({
  templateUrl: 'home.component.html',
  styleUrls: [ './home.component.scss' ]
})

export class HomeComponent implements AfterViewInit{
  @ViewChild(DxPivotGridComponent, { static: false }) pivotGrid: DxPivotGridComponent;
  @ViewChild(DxChartComponent, { static: false }) chart: DxChartComponent;
  orderHistoryItems: OrderHistory[] = [];
  itemHistoryItems: ItemHistory[] = [];
  customers: Customer[] = [];
  pivotGridDataSource: any;
  
  constructor(private dataService: DataServiceComponent ) {
    this.customizeTooltip = this.customizeTooltip.bind(this);
    this.dataService.getOrderHistory().subscribe(items => {
      this.orderHistoryItems = items;
  
    });

    this.dataService.getAllCustomers().subscribe(items => {
      this.customers = items;
      
    });

    this.dataService.getItemHistory().subscribe(items => {
      this.itemHistoryItems = items;
      //get all the individual item sales and put them in a list
      console.log(this.itemHistoryItems)
      this.pivotGridDataSource = {
        fields: [{
          caption: "Item",
          width: 120,
          dataField: "Name",
          area: "row"
        },{
          dataField: "Date",
          dataType: "date",
          area: "column"
        }, {
          groupName: "Date",
          groupInterval: "month",
          visible: true
        }, {
          caption: "Total",
          dataField: "Price",
          dataType: "number",
          summaryType: "sum",
          format: "$ #,##0.00",
          area: "data"
        }],
        store: this.itemHistoryItems
      }
    });

   
  }

  ngAfterViewInit(){
    this.pivotGrid.instance.bindChart(this.chart.instance, {
      dataFieldsDisplayMode: "splitPanes",
      alternateDataFields: false
    });
  }
  customizeTooltip(args) {
    return {
      html: args.seriesName + " | Total<div class='currency'>" + args.valueText + "</div>"
    };
  }
  CurrencyFormatter(cellInfo) {
    return "$" + cellInfo.value.toFixed(2).replace(/(\d)(?=(\d{3})+(?:.\d+)?$)/g, "$1,")
    }

  SummaryFormatter(cellInfo) {
    return "Total: $" + cellInfo.value.toFixed(2).replace(/(\d)(?=(\d{3})+(?:.\d+)?$)/g, "$1,")
    }

  TaxSummaryFormatter(cellInfo) {
    return "Total Tax: $" + cellInfo.value.toFixed(2).replace(/(\d)(?=(\d{3})+(?:.\d+)?$)/g, "$1,")
    }

  calculateMonthlyTax(options) {
    if (options.name === "TaxSummary") {
        if (options.summaryProcess === "start") {
            options.totalValue = 0;
        } else if (options.summaryProcess === "calculate") {
            options.totalValue = options.totalValue + options.value.TotalPrice*0.03;
        }
      }
  }

}
