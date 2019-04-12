using System;
using System.Collections.Generic;
using System.Text;

namespace Cafe.Events.Cafe
{
    public class TabClosed
    {
        public Guid Id;
        public decimal AmountPaid;
        public decimal OrderValue;
        public decimal TipValue;
    }
}
