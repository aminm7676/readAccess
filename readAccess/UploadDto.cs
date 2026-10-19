namespace readAccess
{
    public class UploadDto
    {
        public string TblName { get; set; }
        public IFormFile File { get; set; }
    }
}
