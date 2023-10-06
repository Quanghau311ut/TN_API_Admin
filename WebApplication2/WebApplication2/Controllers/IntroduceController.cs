using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IntroduceController : ControllerBase
    {
        private readonly WebContext _dbContext;
        public IntroduceController(WebContext dbContext)
        {
            _dbContext = dbContext;
        }

        //api/introduce/getdat
        [HttpGet("Getdata")]
        public async Task<IActionResult> Getdata()
        {
            var data = await _dbContext.Introduce.ToListAsync();
            return Ok(data);
        }

        //api/introduce/getdatabyID
        [HttpGet("Getdataby/{id}")]
        public async Task<IActionResult> GetdatabyID(int id)
        {
            var data = await _dbContext.Introduce.FindAsync(id);
            return Ok(data);
        }

        //api/introduce/create
        [HttpPost("Create")]
        public async Task<IActionResult> Add(Introduce introduce)
        {
            _dbContext.Introduce.Add(introduce);
            await _dbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(Add), new { id = introduce.Id }, introduce);
        }

        //api/introduce/edit
        [HttpPut("Edit/{id}")]
        public async Task<IActionResult> Edit(int id, Introduce introduce)
        {
            if (id != introduce.Id)
            {
                return BadRequest();
            }
            var detail = await _dbContext.Introduce.AsTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (detail == null)
            {
                throw new Exception("Không có dữ liệu");
            }
            detail.Image = introduce.Image;
            detail.Content = introduce.Content;
            detail.Name = introduce.Name;
            detail.Description = introduce.Description;
            detail.Creator = "Admin";
            detail.Dated = introduce.Dated;
            _dbContext.Entry(detail).State = EntityState.Modified;
            try
            {
                await _dbContext.SaveChangesAsync();

            }

            catch (DbUpdateConcurrencyException)
            {

                if (!IntroduceExists(id))
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

        private bool IntroduceExists(long id)
        {
            return (_dbContext.Introduce?.Any(x=>x.Id==id)).GetValueOrDefault();
        }

        //api/introduce/delete
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (_dbContext.Introduce == null)
            {
                return BadRequest();
            }
            var data = await _dbContext.Introduce.FindAsync(id);
            _dbContext.Introduce.Remove(data);
            await _dbContext.SaveChangesAsync();
            return NoContent();
        }
        //api/introduce/search
        [HttpGet("Search/{keyword}")]
        public async Task<IActionResult> search(string keyword)
        {
            var data = await _dbContext.Introduce.Where(x => x.Name.Contains(keyword)
                                                || x.Creator.Contains(keyword)
                                                || x.Content.Contains(keyword)
                                                || x.Description.Contains(keyword)).ToListAsync();
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
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UploadFile/Introduce", file.FileName);

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
