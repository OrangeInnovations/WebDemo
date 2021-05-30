using Common.Entities;
using Demo.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Demo.Domain.AggregatesModels.OrderAggregate
{
    public class Order : Entity<int>, IAggregateRoot
    {
        public DateTime OrderDate { get; set; }

        public Address Address { get; private set; }

        public string BuerId { get; set; }

        public OrderStatus OrderStatus { get; private set; }
        private int _orderStatusId;

        public List<OrderItem> OrderItems { get; set; }
        public int? PaymentMethodId { get; set; }
        protected Order()
        {
            OrderItems = new List<OrderItem>();
        }

        public Order(string userId, string userName, Address address, int cardTypeId, string cardNumber, string cardSecurityNumber,
                string cardHolderName, DateTime cardExpiration, string buyerId = null, int? paymentMethodId = null) : this()
        {
            BuerId = buyerId;
            PaymentMethodId = paymentMethodId;
            _orderStatusId = OrderStatus.Submitted.Id;
            OrderDate = DateTime.UtcNow;
            Address = address;

            AddOrderStartedDomainEvent(userId, userName, cardTypeId, cardNumber,
                                       cardSecurityNumber, cardHolderName, cardExpiration);
        }

        public void AddOrderItem(int productId, string productName, decimal unitPrice, decimal discount, string pictureUrl, int units = 1)
        {
            var existingOrderItem = OrderItems.Where(o => o.ProductId == productId)
                .SingleOrDefault();

            if (existingOrderItem != null)
            {
                if (discount > existingOrderItem.Discount)
                {
                    existingOrderItem.SetNewDiscount(discount);
                }

                existingOrderItem.AddUnits(units);
            }
            else
            {
                var orderItem = new OrderItem(productId, productName, unitPrice, discount, pictureUrl, units);
                OrderItems.Add(orderItem);
            }
        }

        public decimal GetTotal()
        {
            return OrderItems.Sum(c => c.Units * c.UnitPrice);
        }

        private void AddOrderStartedDomainEvent(string userId, string userName, int cardTypeId, string cardNumber, string cardSecurityNumber, string cardHolderName, DateTime cardExpiration)
        {
            var orderStartedDomainEvent = new OrderStartedDomainEvent(this, userId, userName, cardTypeId,
                                                                      cardNumber, cardSecurityNumber,
                                                                      cardHolderName, cardExpiration);

            this.AddDomainEvent(orderStartedDomainEvent);
        }

        
    }
}
