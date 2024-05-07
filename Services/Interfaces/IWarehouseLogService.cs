using GlassECommerce.DTOs;
using GlassECommerce.Models;
using Microsoft.AspNetCore.Mvc;

namespace GlassECommerce.Services.Interfaces
{
    public interface IWarehouseLogService
    {
        public Task<IActionResult> GetAllWarehouseLogs(int page);

        public Task<IActionResult> GetWarehouseLogById(int warehouseLogId);

        public Task<IActionResult> AddWarehouseLog(string userId, Model modelExist, WarehouseLogDTO model);

        //public Task<IActionResult> WarehouseLogCSV(DateTime fromeDate, DateTime toDate)

    }
}
