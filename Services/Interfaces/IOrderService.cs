using GlassECommerce.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace GlassECommerce.Services.Interfaces
{
    public interface IOrderService
    {
        public Task<IActionResult> GetAllUserOrders();
        public Task<IActionResult> GetAllOrdersOfCurrenUser(string status);
        public Task<IActionResult> AddOrder(OrderDTO model);
        public Task<IActionResult> UpdateOrderStatus(ChangeOrderStatusDTO model);
        public Task<IActionResult> CancelOrder(int orderId);
    }
}
