using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UploadTest.Context;
using UploadTest.Models;

namespace UploadTest.Controllers
{
    public class UploadController : Controller
    {
        private readonly MyContext _context;

        public UploadController(MyContext myContext)
        {
            _context = myContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(IFormFile files)
        {
            if (files != null)
            {
                if (files.Length > 0)
                {
                    //Getting FileName
                    var fileName = Path.GetFileName(files.FileName);
                    //Getting file Extension
                    var fileExtension = Path.GetExtension(fileName);
                    // concatenating  FileName + FileExtension
                    var newFileName = String.Concat(Convert.ToString(Guid.NewGuid()), fileExtension);

                    var objfiles = new Files()
                    {
                        Id = 0,
                        Name = newFileName,
                        FileType = fileExtension,
                        CreatedOn = DateTime.Now
                    };

                    using (var target = new MemoryStream())
                    {
                        files.CopyTo(target);
                        objfiles.DataFile = target.ToArray();
                    }

                    _context.Files.Add(objfiles);
                    _context.SaveChanges();

                }
            }
            return View();
        }

        //[HttpGet("id")]
        [HttpGet]
        public FileContentResult Download(int id)
        {
            var file = _context.Files.SingleOrDefault(a => a.Id == 1);
            string fileName = file.Name;
            byte[] pdfasBytes = file.DataFile;
            return File(pdfasBytes, "application/pdf", fileName);
        }
    }
}
