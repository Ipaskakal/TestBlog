using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Data.Files
{
    public class Files : IFiles
    {
        private string _imagePATH;

        public Files(IConfiguration configuration)
        {
            _imagePATH = configuration["Path:Images"];
        }

        public FileStream GetImageStream(string image)
        {
            try
            {
                return new FileStream(Path.Combine(_imagePATH, image), FileMode.Open, FileAccess.Read);
            }catch(Exception e)
            {
                return null;
            }
            
        }

        public async Task<string> SaveImage(IFormFile image)
        {
            try
            {
                var save_path = Path.Combine(_imagePATH);
                if (!Directory.Exists(save_path))
                {
                    Directory.CreateDirectory(save_path);
                }
                var afterDot = image.FileName.Substring(image.FileName.LastIndexOf('.'));
                var fileName = $"img_{DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss")}{afterDot}";

                using (var fileStream = new FileStream(Path.Combine(save_path, fileName), FileMode.Create))
                {
                    await image.CopyToAsync(fileStream);
                }
                return fileName;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return "Error"; 
            }
        }
    }
}
