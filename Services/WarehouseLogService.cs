using Castle.Core.Internal;
using GlassECommerce.Data;
using GlassECommerce.DTOs;
using GlassECommerce.Models;
using GlassECommerce.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GlassECommerce.Services
{
    public class WarehouseLogService : ControllerBase, IWarehouseLogService
    {
        private readonly ApplicationDbContext _dbContext;

        public WarehouseLogService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> GetAllWarehouseLogs(int page)
        {
            var warehouseLogs = await _dbContext.WarehousLogs.Skip(20 * (page - 1)).Take(20).ToListAsync();
            if (warehouseLogs.IsNullOrEmpty()) return Ok(new List<WarehouseLog>());
            var response = warehouseLogs.Select(w => new
            {
                WarehouseLogId = w.WarehouseLogId,
                ModelId = w.ModelId,
                CreatedBy = w.User.ToString(),
                IsImport = w.IsImport,
                Quantity = w.Quantity,
                Note = w?.Note,
                CreationDate = w.CreationDate
            }).ToList();
            return Ok(response);
        }

        public async Task<IActionResult> GetWarehouseLogById(int warehouseLogId)
        {
            var warehouseLog = await _dbContext.WarehousLogs.FindAsync(warehouseLogId);
            if (warehouseLog == null) return NotFound(new Response("Error", $"The warehouse log with id = {warehouseLogId} was not found"));
            return Ok(new
            {
                WarehouseLogId = warehouseLog.WarehouseLogId,
                ModelId = warehouseLog.ModelId,
                CreatedBy = warehouseLog.User.ToString(),
                IsImport = warehouseLog.IsImport,
                Quantity = warehouseLog.Quantity,
                Note = warehouseLog?.Note,
                CreationDate = warehouseLog.CreationDate
            });
        }

        public async Task<IActionResult> AddWarehouseLog(string userId, Model modelExist, WarehouseLogDTO model)
        {
            try
            {
                WarehouseLog newWarehouseLog = new WarehouseLog()
                {
                    ModelId = model.ModelId,
                    UserId = userId,
                    IsImport = model.IsImport,
                    Quantity = model.Quantity,
                    Note = model.Note.IsNullOrEmpty() ? "" : model.Note,
                    CreationDate = DateTime.Now
                };
                if (model.IsImport) modelExist.Available += model.Quantity;
                else modelExist.Available -= model.Quantity;
                await _dbContext.AddAsync(newWarehouseLog);
                _dbContext.Update(modelExist);
                await _dbContext.SaveChangesAsync();
                return Created("New import created", new
                {
                    WarehouseLogId = newWarehouseLog.WarehouseLogId,
                    ModelId = newWarehouseLog.ModelId,
                    CreatedBy = newWarehouseLog.User.ToString(),
                    IsImport = newWarehouseLog.IsImport,
                    Quantity = newWarehouseLog.Quantity,
                    Note = newWarehouseLog.Note,
                    CreationDate = newWarehouseLog.CreationDate
                });
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                   new Response("Error", "An error occurs when adding new warehouse log"));
            }
        }

        //public async Task<IActionResult> WarehouseLogCSV(DateTime fromeDate, DateTime toDate)
        //{
        //    var data = await _dbContext.WarehousLogs.Where(w => w.CreationDate >= fromeDate && w.CreationDate <= toDate).OrderByDescending(w => w.CreationDate).ToListAsync();
        //    var total = data.Count();
        //    var csv = new StringBuilder();
        //    string downloadDate = "Download Update: " + DateTime.Now.ToString();
        //    string heading = "Số thứ tự,Người tạo,Loại,Tên sản phẩm,Phân loại,Số lượng,Ghi chú,Ngày tạo";
        //    csv.AppendLine(downloadDate);
        //    csv.AppendLine(heading);
        //    for(int i = 0; i < total; i++)
        //    {
        //        var log = data[i];
        //        var newRow = string.Format("{0},{1},{2},{3},{4},{5},{6},{7}",
        //               i+1,
        //               log.User.ToString(),
        //               log.IsImport ? "Nhập kho" : "Xuất kho",
        //               log.Model.Product.ProductName,
        //               log.
        //               );
        //        csv.AppendLine(newRow);
        //    }
        //    byte[] bytes = Encoding.ASCII.GetBytes(csv.ToString());
        //    return File(bytes, "text/csv", "User_Information.csv");
        //}

    }
}
