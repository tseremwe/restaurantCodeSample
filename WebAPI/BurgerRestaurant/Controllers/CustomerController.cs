using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;

namespace BurgerRestaurant.Controllers
{
   // [DisableCors]
    public class CustomerController : ApiController
    {
        [Route("api/customers")]
        public IEnumerable<Customer> GetAllCustomers()
        {
            using (OrderSystemEntitiesCtx entities = new OrderSystemEntitiesCtx())
            {
                return entities.Customers.ToList();
            }
        }

        [Route("api/customer/{CustomerID}", Name = "GetCustomer")]
        public Customer GetCustomer(int CustomerID)
        {
            using (OrderSystemEntitiesCtx entities = new OrderSystemEntitiesCtx())
            {
                return entities.Customers.FirstOrDefault(c => c.CustomerID == CustomerID);
            }
        }

        [Route("api/addcustomer")]
        [ResponseType(typeof(Customer))]
        public IHttpActionResult AddCustomer(Customer Customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            using (OrderSystemEntitiesCtx entities = new OrderSystemEntitiesCtx())
            {
                entities.Customers.Add(Customer);
                entities.SaveChanges();
                return CreatedAtRoute("GetCustomer", new
                {
                    CustomerID = Customer.CustomerID
                }, Customer);
            }
        }
    }
}
