using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private readonly WebContext _dbContext;
        public BrandController(WebContext dbContext)
        {
            _dbContext = dbContext;
        }

        //api/getdata
        [HttpGet("getdata")]
        public async Task<IActionResult> getdata()
        {
            var data = await _dbContext.Brand.ToListAsync();
            return Ok(data);
        }
        //api/getdatabyID
        [HttpGet("getdatabyID/{id}")]
        public async Task<IActionResult>getdatabyID(int id)
        {
            var data= await _dbContext.Brand.FindAsync(id);
            return Ok(data);
        }
        //api/create
        [HttpPost("create")]
        public async Task<IActionResult> create(Brand brand)
        {
            _dbContext.Brand.Add(brand);
            await _dbContext.SaveChangesAsync();
            return Ok();
        }
        //api/edit
        [HttpPut("edit/{id}")]
        public async Task<IActionResult> edit(int id, Brand brand)
        {
            if(id!= brand.Id)
            {
                return BadRequest();
            }
            var detail= await _dbContext.Brand.AsTracking().FirstOrDefaultAsync(x=>x.Id==id);
            if(detail == null)
            {
                throw new Exception("Không hợp lệ");
            }
            detail.Name = brand.Name;
            detail.Content = brand.Content;
            detail.Image = brand.Image;
            _dbContext.Entry(detail).State=EntityState.Modified;
            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException)
            {
                if (!BrandExist(id))
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

        private bool BrandExist(long id)
        {
            return (_dbContext.Brand?.Any(x=>x.Id==id)).GetValueOrDefault();
        }

        //api/delete
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> delete(int id)
        {
            var detail = await _dbContext.Brand.FindAsync(id);
            _dbContext.Brand.Remove(detail);
            await _dbContext.SaveChangesAsync();
            return Ok();
        }
        //api/search
        [HttpGet("search/{key}")]
        public async Task<IActionResult> search(string key)
        {
            var data=await _dbContext.Brand.Where(x=>x.Name.Contains(key)
                        ||x.Content.Contains(key))
                        .ToListAsync();
            return Ok(data);
        }
        //api/upload-image
        [HttpPost("UploadFile")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                    return BadRequest("Không có file");

                // Đường dẫn lưu trữ ảnh trên server
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UploadFile/Brand", file.FileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                return Ok("Tải file lên thành công");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
    }
}
