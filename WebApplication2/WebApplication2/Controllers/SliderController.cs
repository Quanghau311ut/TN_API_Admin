using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SliderController : ControllerBase
    {
        private readonly WebContext _dbContext;
        private object file;

        public SliderController(WebContext dbContext)
        {
            _dbContext = dbContext;
        }
        //api/getdata
        [HttpGet("getdata")]
        public async Task<IActionResult> getdata()
        {
            var data = await _dbContext.Slider.ToListAsync();
            return Ok(data);
        }
        //api/getdatabyID
        [HttpGet("getdatabyID/{id}")]
        public async Task<IActionResult> getdatabyID(int id)
        {
            var data = await _dbContext.Slider.FindAsync(id);
            return Ok(data);
        }
        //api/create
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromForm] Slider slider)
        {
            try
            {
                if (slider.file == null || slider.file.Length == 0)
                    return BadRequest("Không có file");

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(slider.file.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UploadFile/Slider", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await slider.file.CopyToAsync(stream);
                }
                // Lưu tên tệp vào trường Image
                slider.Image = fileName; 

                _dbContext.Add(slider);
                await _dbContext.SaveChangesAsync();

                return Ok("Tải file lên và thêm Slider thành công");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }


        [HttpPut("edit/{id}")]
        public async Task<IActionResult> Edit(int id, [FromForm] Slider slider)
        {
            if (id != slider.Id)
            {
                return BadRequest();
            }

            var detail = await _dbContext.Slider.FindAsync(id);

            if (detail == null)
            {
                throw new Exception("Không hợp lệ");
            }

            detail.Name = slider.Name;
            detail.Content = slider.Content;

            if (slider.file != null && slider.file.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(slider.file.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UploadFile/Slider", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await slider.file.CopyToAsync(stream);
                }

                detail.Image = fileName;
            }

            _dbContext.Entry(detail).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SliderExists(id))
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

        private bool SliderExists(long id)
        {
            return _dbContext.Slider.Any(e => e.Id == id);
        }

        //api/delete
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> delete(int id)
        {
            var data = await _dbContext.Slider.FindAsync(id);
            _dbContext.Remove(data);
            await _dbContext.SaveChangesAsync();
            return Ok();
        }
        //api/search
        [HttpGet("search/{key}")]
        public async Task<IActionResult> search(string key)
        {
            var data = await _dbContext.Slider.Where(x => x.Name.Contains(key)
                            || x.Content.Contains(key))
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
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UploadFile/Slider", file.FileName);

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
