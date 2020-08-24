using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KeyVaultSecret.Services;
using KeyVaultSecret.Models;
using System.IO;

namespace KeyVaultSecret.Controllers
{
    public class ContactController : Controller
    {
        private ContactService contactService;
        public ContactController()
        {
            contactService = new ContactService();
        }
        // GET: Contact
        public ActionResult Index()
        {
            var contacts = contactService.GetContacts();

            var viewModel = contacts.Select(c => new Contact()
            {
                Id = c.Id,
                Name = c.Name,
                Phone = c.Phone,
                Email = c.Email,
                Address = c.Address
            });

            return View(viewModel);
        }

        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(Contact model)
        {
            // Handling file upload; save the uploaded contact picture into Azure blob storage.
            string pictureFilename = string.Empty;
            pictureFilename = "4k";
            //if (Request.Files.Count > 0)
            //{
            //    var file = Request.Files[0];

            //    if (file != null && file.ContentLength > 0)
            //    {
            //        var fileName = Path.GetFileName(file.FileName);
            //        var path = Path.Combine(Server.MapPath("~/Images/"), fileName);
            //        file.SaveAs(path);

            //        var blobService = new BlobService();
            //        pictureFilename = blobService.UploadPictureToBlob(Server.MapPath("~/Images/"), fileName);
            //    }
            //}

            var id = contactService.AddContact(new Contact()
            {
                Name = model.Name,
                Address = model.Address,
                Email = model.Email,
                Phone = model.Phone,
                PictureName = pictureFilename
            });

            return RedirectToAction("index");
        }

        public ActionResult Details(int id)
        {
            var contact = contactService.GetContact(id);

            return View(new Contact()
            {
                Id = contact.Id,
                Name = contact.Name,
                Phone = contact.Phone,
                Email = contact.Email,
                Address = contact.Address,
                PictureName = null
            });
        }

        public ActionResult Delete(int id)
        {
            var success = contactService.DeleteContact(id);

            return RedirectToAction("index");
        }
    }
}