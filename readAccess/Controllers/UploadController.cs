using Microsoft.AspNetCore.Mvc;

namespace readAccess.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        [HttpPost("{page}/{take}")]
        public async Task<IActionResult> Index([FromForm] UploadDto dto, [FromRoute] int page , [FromRoute] int take)
        {
            if (dto.File == null || dto.File.Length == 0)
                return BadRequest("No file uploaded");

            // Create temp file path
            //  string tempFilePath = Path.GetTempFileName();
            string rootPath = Directory.GetCurrentDirectory();
            string uploadFolder = Path.Combine(rootPath, "Uploads"); // یا هر پوشه دلخواه

            // Create directory if it doesn't exist
            if (!Directory.Exists(uploadFolder))
            {
                Directory.CreateDirectory(uploadFolder);
            }

            string fileName = $"{Guid.NewGuid()}_{dto.File.FileName}";
            string tempFilePath = Path.Combine(uploadFolder, fileName);


            try
            {
                // Save uploaded file to temp location
                using (var stream = new FileStream(tempFilePath, FileMode.Create))
                {
                    await dto.File.CopyToAsync(stream);
                }

               // var data = await dto.File.ReadAsAsync<CardExport>(tempFilePath);
               var data = await AccessDatabaseReader.ReadAccessFileAsync(tempFilePath);

                return Ok(data.Skip((page - 1)*take).Take(take).ToList());
            }
            catch (Exception ex)
            {

            }
            finally
            {
                // Clean up temp file
                if (System.IO.File.Exists(tempFilePath))
                    System.IO.File.Delete(tempFilePath);
            }

            return Ok();
        }
    }
}
