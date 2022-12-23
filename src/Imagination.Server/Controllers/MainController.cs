/*
    This file contains the main core function of the api.

*/


using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Collections.Generic;

namespace Imagination.Controllers
{
    [Route("/convert")]
    [ApiController]

    public class Main : ControllerBase
    {
        [HttpPost]
        [Consumes("multipart/form-data")]
        public IActionResult Post(IList<IFormFile> image)
        {
            foreach (var file in image)
            {
                if (file.Length > 0)
                {
                    string path = @"tmp.jpg";
                    using (Stream output = System.IO.File.Open(path, FileMode.OpenOrCreate))
                    {
                        int read;
                        using var fStream = file.OpenReadStream();
                        byte[] data = new byte[fStream.Length];
                        while ((read = fStream.Read(data, 0, data.Length)) > 0)
                        {
                            output.Write(data, 0, read);
                        }
                    }
                    return File(System.IO.File.OpenRead(path), "image/jpeg");

                }
                else
                {
                    return BadRequest();
                }
            }
        }
    }
}
