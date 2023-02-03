using Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ItemsController : ControllerBase
{
    private readonly List<Item> _items = new List<Item>
    {
        new Item
        {
            Id = Guid.NewGuid(),
            Name = "Item 1",
            SKU = "ITM00001"
        },
        new Item
        {
            Id = Guid.NewGuid(),
            Name = "Item 2",
            SKU = "ITM00002"
        }
    };

    [HttpGet]
    public ActionResult<List<Item>> GetAllItems()
    {
        return Ok(_items);
    }

    [HttpGet("{id:guid}")]
    public ActionResult<Item> GetItemById(Guid id)
    {
        Item found = _items.Find(x => x.Id == id);

        if (found is null)
        {
            return NotFound();
        }

        return Ok(found);
    }

    [HttpPost]
    public ActionResult<Item> AddItem([FromBody] Item item)
    {
        _items.Add(item);

        return CreatedAtAction(nameof(GetItemById), new { id = item.Id }, item);
    }
}
