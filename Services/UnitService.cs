using Castle.Core.Internal;
using GlassECommerce.Data;
using GlassECommerce.DTOs;
using GlassECommerce.Models;
using GlassECommerce.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GlassECommerce.Services
{
    public class UnitService : ControllerBase, IUnitService
    {
        private readonly ApplicationDbContext _dbContext;

        public UnitService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IActionResult> GetAllUnits()
        {
            var units = await _dbContext.Units.ToListAsync();
            if (units.IsNullOrEmpty()) return Ok(new List<Unit>());
            var response = units.Select(u => new
            {
                UnitId = u.UnitId,
                UnitName = u.UnitName
            }).ToList();
            return Ok(response);
        }

        public async Task<IActionResult> GetUnitById(int unitId)
        {
            Unit unitExist = await _dbContext.Units.FindAsync(unitId);
            if (unitExist == null) return NotFound();
            return Ok(new
            {
                UnitId = unitExist.UnitId,
                UnitName = unitExist.UnitName
            });
        }

        public async Task<IActionResult> AddUnit(string unitName)
        {
            var unitExist = await _dbContext.Units.FirstOrDefaultAsync(u => u.UnitName.ToUpper().Equals(unitName.ToUpper()));
            if (unitExist != null) return BadRequest(new Response("Error", "This unit name already exists"));
            Unit newUnit = new Unit()
            {
                UnitName = unitName
            };
            try
            {
                await _dbContext.AddAsync(newUnit);
                await _dbContext.SaveChangesAsync();
                return StatusCode(StatusCodes.Status201Created, new Response("Success", "Create new unit successfully"));
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response("Error", "An error occurs when creating new unit"));
            }
        }

        public async Task<IActionResult> EditUnit(int unitId, string unitName)
        {
            Unit editUnit = await _dbContext.Units.FindAsync(unitId);
            if (editUnit == null) return NotFound(new Response("Error", $"The unit with id = {unitId} was not found"));
            var unitDuplicate = await _dbContext.Units.FirstOrDefaultAsync(u => u.UnitName.ToUpper().Equals(unitName.ToUpper()));
            if (unitDuplicate != null && unitDuplicate.UnitId != editUnit.UnitId) return BadRequest(new Response("Error", "This unit name already exists"));
            editUnit.UnitName = unitName;
            try
            {
                _dbContext.Update(editUnit);
                await _dbContext.SaveChangesAsync();
                return Ok($"update the unit with id = {unitId} successfully");

            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response("Error", "An error occurs when updating unit"));
            }
        }

        public async Task<IActionResult> RemoveUnit(int unitId)
        {
            Unit deleteUnit = await _dbContext.Units.FindAsync(unitId);
            if (deleteUnit == null) return NotFound(new Response("Error", $"The unit with id = {unitId} was not found"));
            if (!deleteUnit.Models.IsNullOrEmpty())
            {
                return StatusCode(StatusCodes.Status403Forbidden,
                    new Response("Error", $"The unit with id = {unitId} cannot be deleted because there is model using this unit"));
            }
            try
            {
                _dbContext.Remove(deleteUnit);
                await _dbContext.SaveChangesAsync();
                return StatusCode(StatusCodes.Status204NoContent,
                    new Response("Success", $"remove unit with id = {unitId} successfully"));
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response("Error", "An error occurs when remove unit"));
            }
        }


    }
}
