using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.DAL.Interfaces
{
    public interface IFiles
    {
        string SaveImage(IFormFile image);

        FileStream GetImageStream(string image);
    }
}
