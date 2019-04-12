using System;
using System.Collections.Generic;
using System.Text;

namespace Cafe.Events.Cafe
{
    public class FoodOrdered
    {
        public Guid Id;
        public List<OrderedItem> Items;
    }
}
