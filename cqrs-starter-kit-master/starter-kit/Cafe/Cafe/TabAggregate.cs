using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cafe.Events.Cafe;
using Edument.CQRS;

namespace Cafe.Cafe
{
    public class TabAggregate : Aggregate,
        IHandleCommand<OpenTab>,
        IHandleCommand<PlaceOrder>,
        IHandleCommand<MarkDrinksServed>,
        IApplyEvent<TabOpened>,
        IApplyEvent<DrinksOrdered>,
        IApplyEvent<DrinksServed>

    {
        private bool open = false;
        private List<int> outstandingDrinks = new List<int>();

        public void Apply(TabOpened e)
        {
            open = true;
        }

        public void Apply(DrinksOrdered e)
        {
            outstandingDrinks.AddRange(e.Items.Select(_ => _.MenuNumber));
        }

        public void Apply(DrinksServed e)
        {
            e.MenuNumbers.ToList().ForEach(_ => outstandingDrinks.Remove(_));
        }

        public IEnumerable Handle(OpenTab c)
        {
            yield return new TabOpened
            {
                Id = c.Id,
                TableNumber = c.TableNumber,
                Waiter = c.Waiter
            };
        }

        public IEnumerable Handle(PlaceOrder c)
        {
            if (!open)
                throw new TabNotOpen();

            var drink = c.Items.Where(_ => _.IsDrink).ToList();

            if (drink.Any())
                yield return new DrinksOrdered
                {
                    Id = c.Id,
                    Items = drink
                };

            var food = c.Items.Where(_ => !_.IsDrink).ToList();

            if (food.Any())
                yield return new FoodOrdered
                {
                    Id = c.Id,
                    Items = food
                };
        }

        public IEnumerable Handle(MarkDrinksServed c)
        {
            if (!AreDrinksOutStanding(c.MenuNumbers))
                throw new DrinksNotOutStanding();
            yield return new DrinksServed
            {
                Id = c.Id,
                MenuNumbers = c.MenuNumbers
            };
        }

        private bool AreDrinksOutStanding(List<int> menuNumbers)
        {
            var curOutStanding = new List<int>(outstandingDrinks);
            foreach (var num in menuNumbers)
                if (curOutStanding.Contains(num))
                    curOutStanding.Remove(num);
                else
                    return false;

            return true;
        }
    }
}
