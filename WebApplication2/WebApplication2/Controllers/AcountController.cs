using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AcountController : ControllerBase
    {
        private readonly WebContext _dbContext;
        public AcountController(WebContext dbContext)
        {
            _dbContext = dbContext;
        }
        //api/Acount/getdata
        [HttpGet("getdata")]
        public async Task<IActionResult> getdata()
        {
            var data = await _dbContext.Acount.ToListAsync();
            return Ok(data);
        }
        //api/Acount/getdatabyID
        [HttpGet("getdatabyID/{id}")]
        public async Task<IActionResult> getdatabyID(int id)
        {
            var data = await _dbContext.Acount.FindAsync(id);
            return Ok(data);
        }
        //api/Acount/create
        [HttpPost("create")]
        public async Task<IActionResult> create(Acount acount)
        {
            _dbContext.Acount.Add(acount);
            await _dbContext.SaveChangesAsync();
            return Ok();
        }
        //api/Acount/edit
        [HttpPut("edit/{id}")]
        public async Task<IActionResult> edit(int id, Acount acount)
        {
            if (id != acount.Id)
            {
                return BadRequest();

            }
            var detail = await _dbContext.Acount.AsTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (detail == null)
            {
                throw new Exception("Không hợp lệ");
            }
            detail.Name = acount.Name;
            detail.Address = acount.Address;
            detail.Email = acount.Email;
            detail.Password = acount.Password;
            detail.Username = acount.Username;
            detail.Dateofbirth = acount.Dateofbirth;
            _dbContext.Entry(detail).State = EntityState.Modified;
            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AcountExist(id))
                {
                    return NotFound();
                }
                else { throw; }
            }
            return Ok();
        }

        private bool AcountExist(long id)
        {
            return (_dbContext.Acount?.Any(x => x.Id == id)).GetValueOrDefault();
        }
        //api/Acount/delete
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> delete(int id)
        {
            var data = await _dbContext.Acount.FindAsync(id);
            _dbContext.Acount.Remove(data);
            await _dbContext.SaveChangesAsync();
            return Ok();
        }
        //api/Acount/search
        [HttpGet("search/{key}")]
        public async Task<IActionResult> search(string key)
        {
            var data = await _dbContext.Acount.Where(
                x => x.Name.Contains(key)
                || x.Address.Contains(key)
                || x.Username.Contains(key)
                || x.Dateofbirth.Equals(key)
                || x.Email.Contains(key)).ToListAsync();
            return Ok(data);
        }
    }
}
