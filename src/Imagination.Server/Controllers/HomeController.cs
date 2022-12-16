using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Jpeg.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using ImageProcessor;
using ImageProcessor.Imaging.Formats;

namespace Jpeg.Controllers
{
    public class HomeController : Controller
    {
        IHostingEnvironment hostingEnvironment;

        public HomeController(IHostingEnvironment hostingEnvironment)
        {
            this.hostingEnvironment = hostingEnvironment;
        }


        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(IFormFile image)
        {
            if (image == null) return View();
            if (image.Length < 0) return View();

            string[] allowedImageTypes = new string[] { "image/gif", "image/png" };
            if(allowedImageTypes.Contains(image.ContentType.ToLower())) return View();

            string imagePath = Path.Combine(hostingEnvironment.WebRootPath, "Images");
            string jpegFileName = Path.GetFileNameWithoutExtension(image.FileName) + ".jpeg";
            string normalImagePath = Path.Combine(imagePath, image.FileName);
            string jpegImagePath = Path.Combine(imagePath, jpegFileName);

            if(!Directory.Exists(imagePath))
                Directory.CreateDirectory(imagePath);

            //save image in original format
            using(var normalFileStream = new FileStream(normalImagePath, FileMode.Create))
            {
                image.CopyTo(normalFileStream);
            }

            //save image in jpeg format
            using (var jpegImageFileStream = new FileStream(jpegImagePath, FileMode.Create))
            {
                using (ImageFactory imageFactory = new ImageFactory(preserveExifData: false))
                {
                    imageFactory.Load(image.OpenReadStream())
                        .Format(new JpegFormat())
                        .Quality(100)
                        .Save(jpegImageFileStream);
                }
            }

            Images viewModel = new Images();
            viewModel.NormalImage = "/Images/" + image.FileName;
            viewModel.JpegImage = "/Images" + jpegFileName;

            return View(viewModel);

        }
    }
}