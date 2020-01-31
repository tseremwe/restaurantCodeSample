using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Data.Entity;

namespace BurgerRestaurant.Controllers
{
    [DisableCors]
    public class ItemController : ApiController
    {
        [Route("api/items")]
        public IEnumerable<Item> GetAllItems()
        {
            using (OrderSystemEntitiesCtx entities = new OrderSystemEntitiesCtx())
            {
                return entities.Items.ToList();
            }
        }
        [Route("api/item/{ItemID}")]
        public Item GetItem(int ItemID)
        {
            using (OrderSystemEntitiesCtx entities = new OrderSystemEntitiesCtx())
            {
                return entities.Items.FirstOrDefault(i => i.ItemID == ItemID);
            }
        }
        [Route("api/items/extras/{ItemID}")]
        public IEnumerable<Extra> GetExtrasByID(int ItemID)
        {
            using (var entities = new OrderSystemEntitiesCtx())
            {
                 var listOfItemIDs = entities.ItemExtras.Where(ie => ie.ItemId == ItemID).Select(x => x.ExtraId).ToList();
                return entities.Extras.Where(e => listOfItemIDs.Contains(e.ExtraID)).ToList();
            }
        }

        [Route("api/item/extra/{ExtraID}")]
        public Extra GetExtra(int ExtraID)
        {
            using (OrderSystemEntitiesCtx entities = new OrderSystemEntitiesCtx())
            {
                return entities.Extras.FirstOrDefault(e => e.ExtraID == ExtraID);
            }
        }
    }
}

