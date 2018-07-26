using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using ProductCatalogApi.Data;
using ProductCatalogApi.Domain;
using ProductCatalogApi.ViewModels;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;

namespace ProductCatalogApi.Controllers
{
    [Produces("application/json")]
    [Route("api/Catalog")]
    public class CatalogController : Controller
    {
        // We need a CatalogContext instance here
        private readonly CatalogContext _catalogContext;
        private readonly IOptionsSnapshot<CatalogSettings> _settings;
        public CatalogController(CatalogContext catalogContext, IOptionsSnapshot<CatalogSettings> settings)
        {
           _catalogContext = catalogContext; 
           _settings = settings;
           // Turn off tracking behavior to increase process speed
           ((DbContext)catalogContext).ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        // Action method for getting catalog types
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> CatalogTypes()
        {
            var items = await _catalogContext.CatalogTypes.ToListAsync();
            return Ok(items);
        }

        // Action method for getting catalog brands
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> CatalogBrands()
        {
            var items = await _catalogContext.CatalogBrands.ToListAsync();
            return Ok(items);
        }

        // Action method to get an item by its id
        [HttpGet]
        [Route("items/{id:int}")]
        public async Task<IActionResult> GetItemById(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }
            var item = await _catalogContext.CatalogItems.SingleOrDefaultAsync(c => c.Id == id);
            if(item != null)
            {
                // We have to replace the url 
                item.PictureUrl = item.PictureUrl.Replace("http://externalcatalogbaseurltobereplaced",_settings.Value.ExternalCatalogBaseUrl);
                return Ok(item);
            }
            return NotFound();
        }

        // GET api/Catalog/items[?pageSize=4&pageIndex=3]
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> Items([FromQuery] int pageSize = 6, [FromQuery] int pageIndex = 0)
        {
            var totalItems = await _catalogContext.CatalogItems
                                .LongCountAsync(); // count of items async
            var itemsOnPage = await _catalogContext.CatalogItems
                                .OrderBy(c => c.Name)
                                .Skip(pageIndex * pageSize)
                                .Take(pageSize)
                                .ToListAsync();
            
            itemsOnPage = ChangeUrlPlaceHolder(itemsOnPage);

            // Cover into a page list
            var model = new PaginatedItemsViewModel<CatalogItem>(pageIndex,pageSize,(int)totalItems,itemsOnPage);
            return Ok(model);
        }

        // GET api/Catalog/items/withname/star5[?pageSize=4&pageIndex=3]
        [HttpGet]
        // Change route to distinguish two methods
        [Route("[action]/withname/{name:minlength(1)}")]
        public async Task<IActionResult> Items(string name, [FromQuery] int pageSize = 6, [FromQuery] int pageIndex = 0)
        {
            var totalItems = await _catalogContext.CatalogItems
                                .Where(c => c.Name.StartsWith(name))
                                .LongCountAsync(); // count of items async
            var itemsOnPage = await _catalogContext.CatalogItems
                                .Where(c => c.Name.StartsWith(name))
                                .OrderBy(c => c.Name)
                                .Skip(pageIndex * pageSize)
                                .Take(pageSize)
                                .ToListAsync();
            
            itemsOnPage = ChangeUrlPlaceHolder(itemsOnPage);

            // Cover into a page list
            var model = new PaginatedItemsViewModel<CatalogItem>(pageIndex,pageSize,(int)totalItems,itemsOnPage);
            return Ok(model);
        }

        // GET api/Catalog/items/type/1/brand/null[?pageSize=4&pageIndex=3]
        [HttpGet]
        // Change route to cover both type and brand ids
        [Route("[action]/type/{catalogTypeId}/brand/{catalogBrandId}")]
        // Catalog Type and Brand Ids are optional/null value symbolized with "?"
        public async Task<IActionResult> Items(int ? catalogTypeId, int ? catalogBrandId, [FromQuery] int pageSize = 6, [FromQuery] int pageIndex = 0)
        {
            // Create a queryable catalog item
            var root = (IQueryable<CatalogItem>)_catalogContext.CatalogItems;
            // Filtering based on catalog type id or brand id supplied in arguments
            if(catalogTypeId.HasValue)
            {
                root = root.Where(c => c.CatalogTypeId == catalogTypeId);
            }  
            if(catalogBrandId.HasValue)
            {
                root = root.Where(c => c.CatalogBrandId == catalogBrandId);
            } 

            var totalItems = await _catalogContext.CatalogItems
                                .LongCountAsync(); // count of items async
            var itemsOnPage = await _catalogContext.CatalogItems
                                .OrderBy(c => c.Name)
                                .Skip(pageIndex * pageSize)
                                .Take(pageSize)
                                .ToListAsync();
            
            itemsOnPage = ChangeUrlPlaceHolder(itemsOnPage);

            // Cover into a page list
            var model = new PaginatedItemsViewModel<CatalogItem>(pageIndex,pageSize,(int)totalItems,itemsOnPage);
            return Ok(model);
        }

        // POST method of creating a CatalogItem using the body of the request
        [HttpPost]
        [Route("items")]
        public async Task<IActionResult> CreateProductAsync([FromBody] CatalogItem product)
        {
            var item = new CatalogItem
            {
                CatalogBrandId = product.CatalogBrandId,
                CatalogTypeId = product.CatalogTypeId,
                Description = product.Description,
                Name = product.Name,
                PictureFileName = product.PictureFileName,
                Price = product.Price
            };
            _catalogContext.CatalogItems.Add(item);
            await _catalogContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetItemById),new {id = item.Id});
        }

        // PUT method to update a record based on item id
        [HttpPut]
        [Route("items")]
        public async Task<IActionResult> UpdateProduct([FromBody] CatalogItem productToUpdate)
        {
            // Check if the item exists
            var catalogItem  = await _catalogContext.CatalogItems
                                    .SingleOrDefaultAsync(i => i.Id == productToUpdate.Id);
            // Null case
            if(catalogItem == null)
            {
                return NotFound(new { Message = $"Item with id {productToUpdate.Id} not found"});
            }
            // If exists
            catalogItem = productToUpdate;
            // Update the object in the list
            _catalogContext.CatalogItems.Update(catalogItem);
            await _catalogContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetItemById),new {id = productToUpdate.Id});
        }

        // DELETE a CatalogItem based on its id
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _catalogContext.CatalogItems.SingleOrDefaultAsync(p => p.Id == id);
            if(product == null)
            {
                return NotFound();
            }
            _catalogContext.CatalogItems.Remove(product);
            await _catalogContext.SaveChangesAsync();
            return NoContent();
        }

        private List<CatalogItem> ChangeUrlPlaceHolder(List<CatalogItem> items)
        {
            items.ForEach(x =>
                x.PictureUrl = x.PictureUrl.Replace("http://externalcatalogbaseurltobereplaced",_settings.Value.ExternalCatalogBaseUrl)
            );
            return items;
        }


    }
}
