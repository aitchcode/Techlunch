﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechlunchApi.Data;
using TechlunchApi.Models;

namespace TechlunchApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoriesController : ControllerBase
    {
        private readonly TechlunchDbContext _context;

        public InventoriesController(TechlunchDbContext context)
        {
            _context = context;
        }

        // GET: api/Inventories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Inventory>>> GetInventory()
        {
            return await _context.Inventory.Include(i => i.IngredientFK).ToListAsync();
        }

        // GET: api/Inventories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Inventory>> GetInventory(int id)
        {
            var inventory = await _context.Inventory.Include(i => i.IngredientFK)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (inventory == null)
            {
                return NotFound();
            }

            return Ok(inventory);
        }

        // GET: api/Inventories/Ingredient/5
        [HttpGet("Ingredient/{id}")]
        public async Task<ActionResult<IEnumerable<IngredientHistory>>> GetIngredientHistory(int id)
        {
            var ingredientHistory = _context.Inventory.Where(i => i.IngredientId == id).Select(i =>
                new IngredientHistory(i.Quantity, i.Price, i.AddedOn)).ToList();

            if (ingredientHistory == null)
            {
                return NotFound();
            }

            return Ok(ingredientHistory);
        }

        // POST: api/Inventories
        [HttpPost]
        public async Task<ActionResult<Inventory>> PostInventory(Inventory inventory)
        {

            var ingredientObj = await _context.Ingredients.FindAsync(inventory.IngredientId);
            
            if (ingredientObj == null || !ingredientObj.Status)
            {
                return NotFound("Error 404: Ingredient item not found");
            }

            _context.Inventory.Add(inventory);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetInventory", new { id = inventory.Id }, inventory);
        }

    }
}
