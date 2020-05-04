using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyApi.DTO;
using MyApi.Models;

namespace MyApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly Context _context;

        public ItemsController(Context context)
        {
            _context = context;
        }

        // GET: api/Items
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ItemDTO>>> GetItems()
        {
            return await _context.Items
            .Select(x => ItemToDTO(x))
            .ToListAsync();
        }

        // GET: api/Items/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDTO>> GetItem(long id)
        {
            var item = await _context.Items.FindAsync(id);

            if (item == null)
            {
                return NotFound();
            }

            return ItemToDTO(item);
        }

        // PUT: api/Items/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutItem(long id, Item item)
        {
            if (id != item.Id)
            {
                return BadRequest();
            }

            _context.Entry(item).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Items
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
    public async Task<ActionResult<ItemDTO>> CreateTodoItem(ItemDTO itemDTO)
    {
        var item = new Item
        {
            IsComplete = itemDTO.IsComplete,
            Name = itemDTO.Name
        };

        _context.Items.Add(item);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetItem),
            new { id = item.Id },
            ItemToDTO(item));
    }

        // DELETE: api/Items/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ItemDTO>> DeleteItem(long id)
        {
            var item = await _context.Items.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            _context.Items.Remove(item);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ItemExists(long id)
        {
            return _context.Items.Any(e => e.Id == id);
        }

        private static ItemDTO ItemToDTO(Item todoItem) =>
        new ItemDTO
        {
            Id = todoItem.Id,
            Name = todoItem.Name,
            IsComplete = todoItem.IsComplete
        };  
    }
}
