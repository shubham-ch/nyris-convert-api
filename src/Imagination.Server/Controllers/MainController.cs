using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System;
using System.Drawing.Imaging;
using System.Drawing;
using System.Text;
using System.Collections.Generic;
using System.Net.Http;
using System.Net;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Imagination.Controllers
{
    [Route("/convert")]
    [ApiController]

    public class helloworld : ControllerBase
    {
        [HttpGet]

        public IActionResult Get()
        {
            return Ok("hello world");
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public IActionResult Post(IList<IFormFile> image)
        {
            //using(MemoryStream ms = new MemoryStream())
            //{
            //    img.CopyTo(ms);
            //    var fileBytes = ms.ToArray();
            //}
            foreach (var file in image)
            {
                if (file.Length > 0)
                {
                    string path = @"tmp.jpg";

                    // Delete the file if it exists.
                    //if (System.IO.File.Exists(path))
                    //{
                    //    System.IO.File.Delete(path);
                    //}

                    ////Create the file.
                    //using FileStream fs = System.IO.File.Create(path);
                    using (Stream output = System.IO.File.Open(path, FileMode.OpenOrCreate)/*new FileStream(path, FileAccess.Write)*/)
                    {
                        int read;
                        using var fStream = file.OpenReadStream();
                        byte[] data = new byte[fStream.Length];
                        while ((read = fStream.Read(data, 0, data.Length)) > 0)
                        {
                            output.Write(data, 0, read);
                        }
                    }
                    //using var fStream = file.OpenReadStream();
                    //byte[] data = new byte[fStream.Length];
                    //fStream.Read(data, 0, data.Length);

                    return File(System.IO.File.OpenRead(path), "image/jpeg");

                }
                else
                {
                    return BadRequest();
                }
            }
            //long length = img.Length;
            //if (length < 0)
            //    return BadRequest();

            //using var fileStream = img.OpenReadStream();
            //byte[] bytes = new byte[length];
            //fileStream.Read(bytes, 0, (int)img.Length);
            //Console.WriteLine(Encoding.Default.GetString(bytes));
            return Ok("Shubham is great!");
        }
    }
}
