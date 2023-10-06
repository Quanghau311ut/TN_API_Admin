using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoreImageController : ControllerBase
    {
        private readonly WebContext _dbContext;
        public MoreImageController(WebContext dbContext)
        {
            _dbContext = dbContext;
        }


        //api/MoreImage/getdata
        [HttpGet("Getdata")]
        public async Task<IActionResult> getdata()
        {
            var data = await _dbContext.MoreImage.ToListAsync();
            return Ok(data);
        }



        //api/MoreImage/getByNewId
        [HttpGet("GetByNewId/{id}")]
        public async Task<IActionResult> GetByNewId(int id)
        {
            var data = await _dbContext.MoreImage.Where(x => x.New_ID == id).ToListAsync();
            return Ok(data);
        }



        //api/MoreImage/getdatabyID
        [HttpGet("Getdataby/{id}")]
        public async Task<IActionResult> getdatabyID(int id)
        {
            var data = await _dbContext.MoreImage.FindAsync(id);
            return Ok(data);
        }




        //api/MoreImage/create
        [HttpPost("Create")]
        public async Task<IActionResult> add(MoreImage moreImage)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _dbContext.MoreImage.Add(moreImage);
            await _dbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(add), new { id = moreImage.Id }, moreImage);
        }



        //api/MoreImage/edit
        [HttpPut("Edit/{id}")]
        public async Task<IActionResult> edit(int id, MoreImage moreImage)
        {
            if (id != moreImage.Id)
            {
                return BadRequest();
            }
            var detail = await _dbContext.MoreImage.AsTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (detail == null)
            {
                throw new Exception("Không có dữ liệu");
            }
            detail.Name_img = moreImage.Name_img;
            detail.IMAGE = moreImage.IMAGE;
            _dbContext.Entry(detail).State = EntityState.Modified;
            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MoreImageExists(id))
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

        private bool MoreImageExists(long id)
        {
            return (_dbContext.MoreImage?.Any(x => x.Id == id)).GetValueOrDefault();
        }




        //api/MoreImage/delete
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> delete(int id)
        {
            if (_dbContext.MoreImage == null)
            {
                return BadRequest();
            }
            var data = await _dbContext.MoreImage.FindAsync(id);
            _dbContext.MoreImage.Remove(data);
            await _dbContext.SaveChangesAsync();
            return Ok();
        }




        //api/MoreImage/search
        [HttpGet("Search/{key}")]
        public async Task<IActionResult> search(string key)
        {
            var data = await _dbContext.MoreImage.Where(x => x.Name_img.Contains(key)).ToListAsync();
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
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UploadFile/MoreImage", file.FileName);

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
