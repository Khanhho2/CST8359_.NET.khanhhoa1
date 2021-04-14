using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using A1.Models;
using A1.Data;
using Microsoft.EntityFrameworkCore;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace A1.Controllers
{
    public class AdvertisementsController : Controller
    {
        private readonly BlobServiceClient _blobClient;
        private readonly string blobContainerName = "communityadvs";
        private readonly SchoolCommunityContext _context;

        public AdvertisementsController(SchoolCommunityContext context, BlobServiceClient blobClient)
        {
            _context = context;
            _blobClient = blobClient;
        }

        [HttpGet]
        public ActionResult Index(string ID)
        {
            return View(_context.Communities.Include(c => c.Advertisements).First(c => c.ID == ID));
        }

        [HttpGet]
        public ActionResult Create(string ID)
        {
            return View(_context.Communities.First(c => c.ID == ID));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(string ID, IFormFile File)
        {
            if (File == null || File.Length == 0) { 
            
                ModelState.AddModelError("", "Invalid File");
                return View(_context.Communities.First(c => c.ID == ID));
            }
            

            var blobKey = Guid.NewGuid().ToString();

            var con = _blobClient.GetBlobContainerClient(this.blobContainerName);
            con.CreateIfNotExists(Azure.Storage.Blobs.Models.PublicAccessType.Blob);
            var client = con.GetBlobClient(blobKey);
            client.Upload(File.OpenReadStream(), new BlobUploadOptions
            {
                HttpHeaders = new BlobHttpHeaders
                {
                    ContentType = File.ContentType
                }
            });
            var Add = new Advertisement
            {
                BlobKey = blobKey,
                Url = client.Uri.AbsoluteUri,
                FileName = File.FileName,
                Community = _context.Communities.First(c => c.ID == ID)
            };
            _context.Advertisements.Add(Add);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index), new { id = ID });
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var Advertisment = _context.Advertisements.Include(c => c.Community).Where(n => n.ID == id).First();
            return View(Advertisment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Advertisement arg)
        {
            var Advertisment = _context.Advertisements.Include(v=>v.Community).First(n => n.ID == arg.ID);

            var c = _blobClient.GetBlobContainerClient(blobContainerName);
            c.DeleteBlobIfExists(Advertisment.BlobKey);

            var communityId = Advertisment.Community.ID;
            _context.Advertisements.Remove(Advertisment);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index),new { id = communityId });
        }
    }
}
