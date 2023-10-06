using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly WebContext _dbContext;
        public CommentController (WebContext dbContext)
        {
            _dbContext = dbContext;
        }
        //api/Comment/getdata
        [HttpGet("getdata")]
        public async Task<IActionResult> getdata()
        {
            var data = await _dbContext.Comment.ToListAsync();
            return Ok(data);
        }
        //api/Comment/getdatabyID/{id}
        [HttpGet("getdatabyID/{id}")]
        public async Task<IActionResult>getdatabyID(int id)
        {
            var data= await _dbContext.Comment.FindAsync(id);
            return Ok(data);
        }
        
      
        //api/Comment/delete
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> delete(int id)
        {
            var data = await _dbContext.Comment.FindAsync(id);
            _dbContext.Comment.Remove(data);
            await _dbContext.SaveChangesAsync();
            return Ok();
        }
        //api/Comment/search
        [HttpGet("search/{key}")]
        public async Task<IActionResult> search(string key)
        {
            var data = await _dbContext.Comment.Where(x => x.Name.Contains(key)
                            || x.Content.Contains(key)
                            || x.Email.Contains(key))
                            .ToListAsync();
            return Ok(data);
        }
    }
}
