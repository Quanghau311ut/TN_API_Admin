using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly WebContext _dbContext;

        public CategoriesController(WebContext dbContext)
        {
             _dbContext= dbContext;
        }
        //api/categories/getdata
        [HttpGet("getdata")]
        public async Task<IActionResult> getdata()
        {
            var data = await _dbContext.Categories.ToListAsync();
            return Ok(data);
        }

        //api/categories/getdatabyID
        [HttpGet("getdatabyID/{id}")]
        public async Task<IActionResult> getdatabyID(int id)
        {
            var data= await _dbContext.Categories.FindAsync(id);
            return Ok(data);
        }

        //api/categories/create
        [HttpPost("Create")]
        public async Task<IActionResult> Create(Categories categories)
        {
             _dbContext.Categories.Add(categories);
            await _dbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(Create), new {id=categories.Id}, categories);
        }
        //api/categories/edit
        [HttpPut("edit/{id}")] 
        public async Task<IActionResult> edit(int id, Categories categories)
        {
            if(id != categories.Id)
            {
                return BadRequest();
            }
            var detail = await _dbContext.Categories.AsTracking().FirstOrDefaultAsync(x=>x.Id == id);
            if (detail == null)
            {
                throw new Exception("Không có dữ liệu");

            }
            detail.name= categories.name;
            detail.description= categories.description;
            detail.Created = "Admin";
            _dbContext.Entry(detail).State = EntityState.Modified;
            try
            {
                await _dbContext.SaveChangesAsync();

            }
            catch(DbUpdateConcurrencyException)
            {
                if (!CategoriesExit(id))
                {
                    return NotFound();

                }
                else
                {
                    throw;
                }
            }
            return Ok();
          
        }

        private bool CategoriesExit(long id)
        {
            return (_dbContext.Categories?.Any(x => x.Id == id)).GetValueOrDefault();
        }

        //api/categories/delete
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> delete(int id)
        {
            if (_dbContext.Categories == null)
            {
                return BadRequest();
            }
            var data = await _dbContext.Categories.FindAsync(id);
            _dbContext.Categories.Remove(data);
            await _dbContext.SaveChangesAsync();
            return Ok();
        }
        //api/categories/search
        [HttpGet("Search/{keyword}")]
        public async Task<IActionResult> Search(string keyword)
        {
            var data = await _dbContext.Categories.Where(x => x.name.Contains(keyword)
                                || x.Created.Contains(keyword)
                                || x.description.Contains(keyword)).ToListAsync();
            return Ok(data);
        }
    }
}
