using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotlineController : ControllerBase
    {
        private readonly WebContext _dbContext;
        public HotlineController(WebContext dbContext)
        {
            _dbContext = dbContext;
        }

        //api/hotline/getdata
        [HttpGet]
        public async Task<IActionResult> getdata()
        {
            var data = await _dbContext.Hotline.ToListAsync();
            return Ok(data);
        }
        //api/hotline/getdatabyID
        [HttpGet("getdatabyID/{id}")]
        public async Task<IActionResult> getdatabyID(int id)
        {
            var data = await _dbContext.Hotline.FindAsync(id);
            return Ok(data);
        }


        //api/hotline/delete
        [HttpDelete("{id}")]
        public async Task<IActionResult> delete(int id)
        {
            var detail = await _dbContext.Hotline.FindAsync(id);
            _dbContext.Hotline.Remove(detail);
            await _dbContext.SaveChangesAsync();
            return Ok();
        }
        //api/hotline/search

        [HttpGet("Search/{key}")]
        public async Task<IActionResult> search(string key)
        {
            var data = await _dbContext.Hotline.Where(x => x.Content.Contains(key)
                                || x.Email.Contains(key)
                                || x.Tel.Contains(key)).ToListAsync();
            return Ok(data);
        }
    }
}
