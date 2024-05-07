using Castle.Core.Internal;
using GlassECommerce.Common;
using GlassECommerce.Data;
using GlassECommerce.DTOs;
using GlassECommerce.Models;
using GlassECommerce.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace GlassECommerce.Services
{
    public class OrderService : ControllerBase, IOrderService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IAuthenticationService _authService;
        private readonly INotificationService _notificationService;

        public OrderService(ApplicationDbContext dbContext, IAuthenticationService authService, INotificationService notificationService)
        {
            _dbContext = dbContext;
            _authService = authService;
            _notificationService = notificationService;
        }
        //order
        public async Task<IActionResult> GetAllOrdersOfCurrenUser(string status)
        {
            User currentUser = await _authService.GetCurrentLoggedInUser();
            if (currentUser == null) return Unauthorized();
            var userOrders = status.IsNullOrEmpty() ? currentUser.Orders.OrderByDescending(o => o.OrderDate) :
                             currentUser.Orders.Where(o => o.OrderStatus.Equals(status)).OrderByDescending(o => o.OrderDate);
            if (userOrders.IsNullOrEmpty()) return Ok(new List<Order>());
            string userName = currentUser.ToString();
            return Ok(userOrders.Select(o => new
            {
                OrderId = o.OrderId,
                User = userName,
                DeliveryAddress = o.DeliveryAddress,
                OrderDate = o.OrderDate,
                DeliveredDate = o?.DeliveredDate,
                OrderStatus = o.OrderStatus,
                Total = o.TotalCost,
                OrderIems = o.OrderItems.Select(od => new
                {
                    OrderItemId = od.OrderItemId,
                    ProductId = od.Model.ProductId,
                    ModelId = od.ModelId,
                    Quanty = od.Quantity,
                    Cost = od.Cost
                })
            }));
        }

        public async Task<IActionResult> GetAllUserOrders()
        {
          
            var allOrders = await _dbContext.Orders.OrderByDescending(o => o.OrderDate).ToListAsync();
            if (allOrders.IsNullOrEmpty()) return Ok(new List<Order>());
            return Ok(allOrders.Select(o => new
            {
                OrderId = o.OrderId,
                User = o.User.ToString(),
                DeliveryAddress = o.DeliveryAddress,
                OrderDate = o.OrderDate,
                DeliveredDate = o?.DeliveredDate,
                OrderStatus = o.OrderStatus,
                Total = o.TotalCost,
                OrderIems = o.OrderItems.Select(od => new
                {
                    OrderItemId = od.OrderItemId,
                    ProductId = od.Model.ProductId,
                    ModelId = od.ModelId,
                    Quanty = od.Quantity,
                    Cost = od.Cost
                })
            }));
        }

        public async Task<IActionResult> AddOrder(OrderDTO model)
        {

            if (!model.PaymentMethod.Equals(PaymentMethod.PAY_ON_DELIVERY) && !model.PaymentMethod.Equals(PaymentMethod.PAYPAL))
                return BadRequest(new Response("Error", $"The payment method must be \"{PaymentMethod.PAY_ON_DELIVERY}\" or \"{PaymentMethod.PAYPAL}\""));
            User userExist = await _authService.GetCurrentLoggedInUser();
            if (userExist == null) return Unauthorized();
            var userCart = userExist.CartItems;
            if (userCart.IsNullOrEmpty()) return BadRequest(new Response("Error", "There is no item in user cart"));
            try
            {

                Order newOrder = new Order()
                {
                    UserId = userExist.Id,
                    DeliveryAddress = model.DeliveryAddress,
                    Note = model?.Note,
                    OrderDate = DateTime.Now,
                    PaymentMethod = model.PaymentMethod,
                    OrderStatus = model.PaymentMethod.Equals(PaymentMethod.PAYPAL) ? "Processing" : "Pending",
                    TotalCost = 0
                };
                await _dbContext.AddAsync(newOrder);
                await _dbContext.SaveChangesAsync();
                foreach (var cartItem in userCart)
                {
                    var modelCart = cartItem.Model;
                    OrderItem newOrderItem = new OrderItem()
                    {
                        OrderId = newOrder.OrderId,
                        ModelId = modelCart.ModelId,
                        Quantity = cartItem.Quantity,
                        Cost = modelCart.Price
                    };
                    newOrder.TotalCost += newOrderItem.Cost * cartItem.Quantity;
                    modelCart.Available -= cartItem.Quantity;
                    await _dbContext.AddAsync(newOrderItem);

                }
                _dbContext.RemoveRange(userCart);
                _dbContext.Update(newOrder);
                await _dbContext.SaveChangesAsync();
                return Created("new order created", new
                {
                    OrderId = newOrder.OrderId,
                    UserId = newOrder.UserId,
                    DeliveryAddress = newOrder.DeliveryAddress,
                    Note = newOrder?.Note,
                    OrderDate = newOrder.OrderDate,
                    PaymentMethod = newOrder.PaymentMethod,
                    OrderStatus = newOrder.OrderStatus,
                    TotalCost = newOrder.TotalCost,
                    OrderItems = newOrder.OrderItems.Select(od => new
                    {
                        OrderItemId = od.OrderItemId,
                        ProductId = od.Model.ProductId,
                        ModelId = od.ModelId,
                        Quanty = od.Quantity,
                        Cost = od.Cost
                    })
                });
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                  new Response("Error", "An error occurs when adding order"));
            }
        }

        public async Task<IActionResult> CancelOrder(int orderId)
        {
            try
            {
                User userExist = await _authService.GetCurrentLoggedInUser();
                if (userExist == null) return Unauthorized();
                var orderExist = userExist.Orders.FirstOrDefault(o => o.OrderId == orderId);
                if (orderExist == null) return NotFound(new Response("Error", $"There is no order with id = {orderId} in user's order list"));
                if (!orderExist.OrderStatus.Equals("Pending") && !orderExist.OrderStatus.Equals("Processing")) return BadRequest(new Response("Error", $"Cannot cancel order in {orderExist.OrderStatus}"));
                orderExist.OrderStatus = "Canceled";
                _dbContext.Update(orderExist);
                await _dbContext.SaveChangesAsync();
                await _notificationService.AddNotification(userExist.Id, "Hủy đơn hàng thành công", "Đơn hàng của bạn đã được hủy thành công bởi hệ thống");
                return Ok(new Response("Success", $"Cancel order with id = {orderId} successfully"));
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                  new Response("Error", "An error occurs when canceling order"));
            }
        }

        public async Task<IActionResult> UpdateOrderStatus(ChangeOrderStatusDTO model)
        {
            try
            {
             
                Order orderExist = await _dbContext.Orders.FindAsync(model.OrderId);
                if (orderExist == null) return NotFound(new Response("Error", $"The order with id = {model.OrderId} was not found"));
                if (!OrderStatus.ValidStatus.Contains(model.Status))
                    return BadRequest(new Response("Error", $"The order status must be " +
                        $"\"{OrderStatus.PENDING}\", \"{OrderStatus.PROCESSING}\", \"{OrderStatus.DELIVERING}\", \"{OrderStatus.DELIVERED}\""));
                if (OrderStatus.ValidStatus.IndexOf(orderExist.OrderStatus) > OrderStatus.ValidStatus.IndexOf(model.Status))
                    return BadRequest(new Response("Error", $"Cannot change order status from {orderExist.OrderStatus} to {model.Status}"));
                orderExist.OrderStatus = model.Status;
                if (model.Status.Equals(OrderStatus.DELIVERED)) 
                    orderExist.DeliveredDate = DateTime.Now;
                _dbContext.Update(orderExist);
                await _dbContext.SaveChangesAsync();
                return Ok(new
                {
                    OrderId = orderExist.OrderId,
                    UserId = orderExist.UserId,
                    DeliveryAddress = orderExist.DeliveryAddress,
                    Note = orderExist?.Note,
                    OrderDate = orderExist.OrderDate,
                    OrderStatus = orderExist.OrderStatus,
                    TotalCost = orderExist.TotalCost,
                    OrderItems = orderExist.OrderItems.Select(od => new
                    {
                        OrderItemId = od.OrderItemId,
                        ProductId = od.Model.ProductId,
                        ModelId = od.ModelId,
                        Quanty = od.Quantity,
                        Cost = od.Cost
                    })
                });
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                 new Response("Error", "An error occurs when change order status"));
            }

        }
    }
}
