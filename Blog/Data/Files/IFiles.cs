using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Data.Files
{
    public interface IFiles
    {
        Task<string> SaveImage(IFormFile image);

        FileStream GetImageStream(string image);
    }
}
