using BurgerRestaurant.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;

namespace BurgerRestaurant.Controllers
{
    [DisableCors]
    public class OrderController : ApiController
    {
        [Route("api/orders")]
        public IEnumerable<Order> GetAllOrders()//give it a read model
        {
            using (OrderSystemEntitiesCtx entities = new OrderSystemEntitiesCtx())
            {
                return entities.Orders.ToList();
            }
        }

        [Route("api/orders/order/{OrderID}", Name = "GetOrder")]
        public Order GetOrderByOrderID(int OrderID) 
        {
            using (OrderSystemEntitiesCtx entities = new OrderSystemEntitiesCtx())
            {
                return entities.Orders.FirstOrDefault(o => o.OrderID == OrderID);
            }
        }

        [Route("api/orders/orderitem/{OrderItemID}", Name = "GetOrderItem")]
        public OrderItem GetOrderItemByOrderID(int OrderItemID) 
        {
            using (OrderSystemEntitiesCtx entities = new OrderSystemEntitiesCtx())
            {
                return entities.OrderItems.FirstOrDefault(o => o.OrderItemID == OrderItemID);
            }
        }

        [Route("api/order/{OrderID}")]
        public Order GetOrderByCustomerID(int CustomerID) 
        {
            using (OrderSystemEntitiesCtx entities = new OrderSystemEntitiesCtx())
            {
                return entities.Orders.FirstOrDefault(o => o.CustomerID == CustomerID);
            }
        }

        [Route("api/completeorder")]
        [ResponseType(typeof(Order))]
        public IHttpActionResult CompleteOrder(OrderReadModel OrderItem) //give it a read model
        {
            IHttpActionResult newOrder;
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var listOfOrderItemIDs = this.addOrderItems(OrderItem);

            using (OrderSystemEntitiesCtx entities = new OrderSystemEntitiesCtx())
            {
                Order currentOrder = new Order()
                {
                    CustomerID = OrderItem.CustomerID,
                    Completed = true,
                    Cancelled = false,
                    TotalPrice = OrderItem.TotalPrice,
                    Tax = OrderItem.TotalPrice - OrderItem.Price,
                    Date = DateTime.Now,
                    MonthYear = string.Format("{0:MMMM yyyy}", DateTime.Now)
                };
                entities.Orders.Add(currentOrder);

                entities.SaveChanges();
                  newOrder = CreatedAtRoute("GetOrder", new
                 {
                    OrderID = currentOrder.OrderID
                  }, currentOrder);
                this.addOrderDetails(listOfOrderItemIDs, currentOrder.OrderID);

            }
           
            return newOrder;
        }

        public List<int> addOrderItems(OrderReadModel OrderItem)
        {
            IHttpActionResult newOrderItem;
            List<int> listOfOrderItemIDs = new List<int>();
            using (OrderSystemEntitiesCtx entities = new OrderSystemEntitiesCtx())
            {
                //for each item
                foreach (var item in OrderItem.Items)
                {
                    foreach (var extra in item.ExtraItems)
                    {
                        OrderItem currentOrderItem = new OrderItem()
                        {
                            ItemID = item.ItemID,
                            ExtraID = extra.ExtraID
                        };
                        OrderItem createdOrderItem = entities.OrderItems.Add(currentOrderItem);
                        entities.SaveChanges();
                        newOrderItem = CreatedAtRoute("GetOrderItem", new
                        {
                            OrderItemID = currentOrderItem.OrderItemID
                        }, currentOrderItem);
                        listOfOrderItemIDs.Add(createdOrderItem.OrderItemID);
                    }
                }
            }
            return listOfOrderItemIDs;
        }

        public void addOrderDetails(List<int> OrderItemIDs, int OrderID)
        {
            using (OrderSystemEntitiesCtx entities = new OrderSystemEntitiesCtx())
            {
                foreach (var item in OrderItemIDs)
                {
                    OrderDetail currentOrderDetail = new OrderDetail()
                    {
                        OrderID = OrderID,
                        OrderItemID = item
                    };
                    OrderDetail createdOrderDetail = entities.OrderDetails.Add(currentOrderDetail);
                }
                entities.SaveChanges();
            }
        }

        [Route("api/order/history")]
        public List<OrderHistoryReadModel> GetOrderHistory()
        {
            List<OrderHistoryReadModel> orderHistory = new List<OrderHistoryReadModel>();
            List<Order> allOrders = new List<Order>();
            
            using (OrderSystemEntitiesCtx entities = new OrderSystemEntitiesCtx())
            {
                allOrders = entities.Orders.ToList();
                foreach(var order in allOrders)
                {
                    OrderHistoryReadModel currentReadModel = new OrderHistoryReadModel();
                    List<Item> allItems = new List<Item>();
                    List<int> allOrderItemIDs = new List<int>();
                    List<int> allItemIDs = new List<int>();
                    allOrderItemIDs = entities.OrderDetails.Where(od => od.OrderID == order.OrderID).Select(x => x.OrderItemID).ToList();
                    
                    foreach (var id in allOrderItemIDs)
                    {
                        Debug.WriteLine("itemid " + id);
                        //allItemIDs = entities.OrderItems.Where(od => od.OrderItemID == id).Select(x => x.ItemID).ToList();
                        OrderItem currentOrderItem = entities.OrderItems.FirstOrDefault(i => i.OrderItemID == id);
                        allItemIDs.Add(currentOrderItem.ItemID);
                    }
                    foreach (var id in allItemIDs)
                    {
                    //    allItemIDs = entities.OrderItems.Where(od => od.OrderItemID == id).Select(x => x.ItemID).ToList();
                        Item currentItem = entities.Items.FirstOrDefault(i => i.ItemID == id);
                        allItems.Add(currentItem);
                    }
                    currentReadModel.OrderID = order.OrderID;
                    currentReadModel.CustomerID = order.CustomerID;
                    currentReadModel.Completed = order.Completed;
                    currentReadModel.Cancelled = order.Cancelled;
                    currentReadModel.TotalPrice = order.TotalPrice;
                    currentReadModel.Date = order.Date;
                    currentReadModel.MonthYear = order.MonthYear;
                    currentReadModel.Items = allItems;
                    orderHistory.Add(currentReadModel);
                }
            }
          
            return orderHistory;
        }
        [Route("api/order/itemhistory")]
        public List<ItemHistoryReadModel> GetOrderItemHistory()
        {
            List<ItemHistoryReadModel> itemHistory = new List<ItemHistoryReadModel>();
            List<Order> allOrders = new List<Order>();

            using (OrderSystemEntitiesCtx entities = new OrderSystemEntitiesCtx())
            {
                try
                {
                    allOrders = entities.Orders.ToList();
                    foreach (var order in allOrders)
                    {
                        ItemHistoryReadModel currentReadModel = new ItemHistoryReadModel();
                        List<Item> allItems = new List<Item>();
                        List<int> allOrderItemIDs = new List<int>();
                        List<int> allItemIDs = new List<int>();
                        allOrderItemIDs = entities.OrderDetails.Where(od => od.OrderID == order.OrderID).Select(x => x.OrderItemID).ToList();

                        foreach (var id in allOrderItemIDs)
                        {
                            Debug.WriteLine("itemid " + id);
                            OrderItem currentOrderItem = entities.OrderItems.FirstOrDefault(i => i.OrderItemID == id);
                            allItemIDs.Add(currentOrderItem.ItemID);
                        }
                        foreach (var id in allItemIDs)
                        { 
                            Item currentItem = entities.Items.FirstOrDefault(i => i.ItemID == id);
                            allItems.Add(currentItem);
                            currentReadModel.ItemID = currentItem.ItemID;
                            currentReadModel.Name = currentItem.Name;
                            currentReadModel.Price = currentItem.Price;
                            currentReadModel.Extras = true;
                            currentReadModel.Date = order.Date;
                            currentReadModel.MonthYear = order.MonthYear;
                            itemHistory.Add(currentReadModel);
                        }

                    }
                }
                catch(Exception exc)
                {
                    Console.WriteLine(exc.GetType());
                    Debug.WriteLine(exc.GetType());
                }
            }

            return itemHistory;
        }
    }
}
