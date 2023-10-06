using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private readonly WebContext _dbContext;

        public MenuController(WebContext dbContext)
        {
            _dbContext = dbContext;
        }
        //api/getdata
        [HttpGet("Getdata")]
        public async Task<IActionResult> Getdata()
        {
            var data = await _dbContext.Menu.ToListAsync();
            return Ok(data);
        }


        //api/getdatabyID/{id}
        [HttpGet("Getdataby/{id}")]
        public async Task<IActionResult> GetdatabyID(int id)
        {
            var data= await _dbContext.Menu.FindAsync(id);
            return Ok(data);
        }

        //api/menu/add
        [HttpPost("Create")]
        public async Task<IActionResult>Add(Menu menu)
        {
            _dbContext.Menu.Add(menu);
            await _dbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(Add), new { id = menu.Id }, menu);
        }

        //api/munu/update
        [HttpPut("Edit/{id}")]
        public async Task<IActionResult>Edit( int id, Menu menu)
        {
            if(id  != menu.Id)
            {
                return BadRequest();
            }
            var detail= await _dbContext.Menu.AsNoTracking().FirstOrDefaultAsync( x => x.Id == id);
            if(detail == null)
            {
                throw new Exception("không có dữ liệu");
            }
            detail.Name = menu.Name;
            detail.Description = menu.Description;
            detail.Dated= menu.Dated;
            detail.Creator= menu.Creator;
            _dbContext.Entry(detail).State = EntityState.Modified;
            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) {

                if (!MenuExists(id))
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

        private bool MenuExists(long id)
        {
            return (_dbContext.Menu?.Any(e => e.Id == id)).GetValueOrDefault();
        }



        //api/menu/delete
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (_dbContext.Menu == null)
            {
                return NotFound();
            }
            var data = await _dbContext.Menu.FindAsync(id);
            _dbContext.Menu.Remove(data);
            await _dbContext.SaveChangesAsync();
            return NoContent();
        }

         //api/menu/search
         [HttpGet("search{keyword}")]
         public async Task<IActionResult> Search(string keyword)
        {
            var data= await _dbContext.Menu.Where( x => x.Name.Contains(keyword)|| x.Description.Contains(keyword)|| x.Creator.Contains(keyword)).ToListAsync();
            return Ok(data);
        }

    }
}
