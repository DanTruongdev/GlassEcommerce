using Microsoft.AspNetCore.Mvc;

namespace GlassECommerce.Services.Interfaces
{
    public interface IUnitService
    {
        public Task<IActionResult> GetAllUnits();
        public Task<IActionResult> GetUnitById(int unitId);
        public Task<IActionResult> AddUnit(string unitName);
        public Task<IActionResult> EditUnit(int unitId, string unitName);
        public Task<IActionResult> RemoveUnit(int unitId);
    }
}
