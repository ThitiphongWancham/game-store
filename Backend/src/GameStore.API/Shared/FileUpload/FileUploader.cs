namespace GameStore.API.Shared.FileUpload;

public class FileUploader(IWebHostEnvironment env, IHttpContextAccessor accessor)
{
    public async Task<FileUploadResult> UploadFileAsync(IFormFile file, string folder)
    {
        // If the file is bigger than 64kb, then the ASP.NET will store it temporarily in the temporal location in the server
        // This is known as a buffered approach, and it's meant for small files. Huge files should use streaming approach.
        var result = new FileUploadResult();

        if (file.Length == 0) 
        {
            result.IsSuccess = false;
            result.ErrorMessage = "No file uploaded";
            return result;
        }

        // file size is limited to 10mb
        if (file.Length > 10 * 1024 * 1024)
        {
            result.IsSuccess = false;
            result.ErrorMessage = "file is too large";
            return result;
        }
        
        var permittedExtensions = new List<string> { ".jpg", ".jpeg", ".png" };
        var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (string.IsNullOrEmpty(fileExtension) || !permittedExtensions.Contains(fileExtension))
        {
            result.IsSuccess = false;
            result.ErrorMessage = "Invalid file type";
            return result;
        }
        
        var uploadFolder = Path.Combine(env.WebRootPath, folder);
        if (!Directory.Exists(uploadFolder))
        {
            Directory.CreateDirectory(uploadFolder);
        }

        var safeFileName = Guid.NewGuid() + fileExtension;
        var fullPath = Path.Combine(uploadFolder, safeFileName);

        await using var stream = new FileStream(fullPath, FileMode.Create);
        await file.CopyToAsync(stream);

        var httpContext = accessor.HttpContext;
        var fileUrl = $"{httpContext?.Request.Scheme}://{httpContext?.Request.Host}/{folder}/{safeFileName}";
        
        result.IsSuccess = true;
        result.FileUrl = fileUrl;
        return result;
    }
}