using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace SERP.Utilities.BlobUtility
{
    public static class UploadImage
    {
        public static async Task<List<string>> UploadImageOnFolder(List<IFormFile> imageFiles, IHostingEnvironment _hostingEnvironment)
        {
            List<string> imageStoragePaths = new List<string>();
            foreach(var file in imageFiles)
            {
                try
                {
                    if (file.Length > 0)
                    {
                        var upload = Path.Combine(_hostingEnvironment.WebRootPath, "Images//");
                        using (FileStream fs = new FileStream(Path.Combine(upload, file.FileName), FileMode.Create))
                        {
                            await file.CopyToAsync(fs);
                        }
                        imageStoragePaths.Add("Images//" + file.FileName);
                    }
                }
                catch (Exception ex)
                {
                    string message = ex.Message;
                }
            }


            return imageStoragePaths;
        }
    }
}
