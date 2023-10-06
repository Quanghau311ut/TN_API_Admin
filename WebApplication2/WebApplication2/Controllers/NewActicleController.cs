using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewActicleController : ControllerBase
    {
        private readonly WebContext _dbContext;
        public NewActicleController(WebContext dbcontext)
        {
            _dbContext = dbcontext;
        }


        //api/newacticle/getdata
        [HttpGet("Getdata")]
        public async Task<IActionResult> getdata()
        {
            var data = await _dbContext.NewActicle.ToListAsync();
            return Ok(data);
        }



        //api/newacticle/getdatabyID
        [HttpGet("getdatabyId/{id}")]
        public async Task<IActionResult> getdatabyID(int id)
        {
            var data = await _dbContext.NewActicle.FirstOrDefaultAsync(x => x.ID == id);
            if (data == null)
            {
                return BadRequest();
            }
            data.ListMoreImage = await (from moreImage in _dbContext.MoreImage
                                     join newArticle in _dbContext.NewActicle
                                     on moreImage.New_ID equals newArticle.ID
                                     where newArticle.ID == data.ID
                                     select new 
                                     {
                                         // moreImage.Id,
                                         moreImage.Name_img,
                                         // moreImage.Dated,
                                         MoreImage = moreImage.IMAGE,
                                         //NewArticleId = newArticle.ID,
                                         //Name = newArticle.Name,
                                         //Image = newArticle.Image,
                                         //Description = newArticle.Description,
                                         //Categories_ID = newArticle.Categories_ID,
                                         //Content = newArticle.Content,
                                         //Dated = newArticle.Dated,
                                         //Created = newArticle.Created
                                     }).ToListAsync();
            return Ok(data);
        }



        //api/listImage
        [HttpGet("listImage/{id}")]
        public async Task<IActionResult> listImage(int id)
        {
            var data = from moreImage in _dbContext.MoreImage
                       join newArticle in _dbContext.NewActicle
                       on moreImage.New_ID equals newArticle.ID
                       where newArticle.ID == id
                       select new
                       {
                           // moreImage.Id,
                           // moreImage.Name_img,
                           // moreImage.Dated,
                           MoreImage = moreImage.IMAGE,
                           NewArticleId = newArticle.ID,
                           Name = newArticle.Name,
                           Image = newArticle.Image,
                           Description = newArticle.Description,
                           Categories_ID = newArticle.Categories_ID,
                           Content = newArticle.Content,
                           Dated = newArticle.Dated,
                           Created = newArticle.Created
                       };

            return Ok(data);
        }



        //api/newacticle/create
        [HttpPost("Create")]
        public async Task<IActionResult> add(NewActicle newActicle)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _dbContext.NewActicle.Add(newActicle);
            await _dbContext.SaveChangesAsync();
            return Ok();
        }

       

        //api/newacticle/edit
        [HttpPut("Edit/{id}")]
        public async Task<IActionResult> edit(int id, NewActicle newActicle)
        {
            if (id != newActicle.ID)
            {
                return BadRequest();
            }
            var detail = await _dbContext.NewActicle.AsTracking().FirstOrDefaultAsync(x => x.ID == id);
            if (detail == null)
            {
                throw new Exception("Không có dữ liệu");
            }
            detail.Name = newActicle.Name;
            detail.Image = newActicle.Image;
            detail.Description = newActicle.Description;
            detail.Content = newActicle.Content;
            detail.Created = "Admin";
            _dbContext.Entry(detail).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return Ok();

        }



        //api/newacticle/delete
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> delete(int id)
        {
            if (_dbContext.NewActicle == null)
            {
                return BadRequest();
            }
            var data = await _dbContext.NewActicle.FindAsync(id);
            _dbContext.NewActicle.Remove(data);
            await _dbContext.SaveChangesAsync();
            return Ok();
        }




        //api/newacticle/search
        [HttpGet("Search/{keyword}")]
        public async Task<IActionResult> search(string keyword)
        {
            var data = await _dbContext.NewActicle.Where(x => x.Name.Contains(keyword)
                            || x.Description.Contains(keyword)
                            || x.Created.Contains(keyword)
                            || x.Content.Contains(keyword)).ToListAsync();
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
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UploadFile/NewArticle", file.FileName);

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
