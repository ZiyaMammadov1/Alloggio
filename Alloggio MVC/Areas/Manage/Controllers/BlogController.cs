using Alloggio_MVC.Helpers.FileManager;
using Core_Layer.Entities;
using Data_Layer.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Alloggio_MVC.Areas.Manage.Controllers
{
    [Area("manage")]
    [Authorize(Roles = "Admin")]
    public class BlogController : Controller
    {
        private readonly BlogRepository _blogRepository;
        private readonly IWebHostEnvironment _env;

        public BlogController(BlogRepository blogRepository, IWebHostEnvironment env)
        {
            _blogRepository = blogRepository;
            _env = env;
        }

        public IActionResult Index()
        {
            return View(_blogRepository.GetAll());
        }

        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Create(Blog blog)
        {
            if (!ModelState.IsValid)
            {
                return View(blog);
            }
            if(blog.Photo == null)
            {
                ModelState.AddModelError("Photo","Photo is required");
                return View(blog);
            }
            string NewFileName = "";
            if (blog.Photo.ContentType == "image/jpeg" || blog.Photo.ContentType == "image/png" || blog.Photo.ContentType == "image/jpg")
            {

                if (blog.Photo.Length < 2097152)
                {
                    NewFileName = FileManager.Save(_env.WebRootPath, "assets/image/Blog/Images", blog.Photo);                   
                }
                else
                {
                    ModelState.AddModelError("", "Image size can't be larger than 2mb");
                    return View(blog);
                }

            }
            else
            {
                ModelState.AddModelError("", "Photo is not correct type Ex: (.png, .jpg)");
                return View(blog);
            }
            Blog newBlog = new Blog() { 
                Header = blog.Header,
                Content = blog.Content,
                Image = NewFileName,
                CreateAt = DateTime.Now
            };
            _blogRepository.Add(newBlog);
            _blogRepository.Commit();

            return RedirectToAction("index");
        }

        public IActionResult Edit(int id)
        {
            Blog blog = _blogRepository.Get(x=>x.id == id);
            if(blog == null)
            {
                return NotFound();
            }
            return View(blog);
        }

        [HttpPost]
        public IActionResult Edit(Blog blog)
        {
            var currentBlog = _blogRepository.Get(x=>x.id == blog.id);
            if(currentBlog == null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return View(blog);
            }
            if(blog.Photo != null)
            {
                string NewFileName = "";
                if (blog.Photo.ContentType == "image/jpeg" || blog.Photo.ContentType == "image/png" || blog.Photo.ContentType == "image/jpg")
                {

                    if (blog.Photo.Length < 2097152)
                    {
                        NewFileName = FileManager.Save(_env.WebRootPath, "assets/image/Blog/Images", blog.Photo);
                        FileManager.Delete(_env.WebRootPath, "assets/image/Blog/Images", currentBlog.Image);
                        currentBlog.Image = NewFileName;
                    }
                    else
                    {
                        ModelState.AddModelError("", "Image size can't be larger than 2mb");
                        return View(blog);
                    }

                }
                else
                {
                    ModelState.AddModelError("", "Photo is not correct type Ex: (.png, .jpg)");
                    return View(blog);
                }
            }
           

            currentBlog.Header = blog.Header;
            currentBlog.Content = blog.Content;
            _blogRepository.Update(currentBlog);
            _blogRepository.Commit();
            return RedirectToAction("index");
        }

        public IActionResult Delete(int id)
        {
            var currentBlog = _blogRepository.Get(x=>x.id == id);
            if(currentBlog == null)
            {
                return NotFound();
            }
            FileManager.Delete(_env.WebRootPath, "assets/image/Blog/Images", currentBlog.Image);

            _blogRepository.Delete(currentBlog);
            _blogRepository.Commit();
            return RedirectToAction("index");
        }
    }
}
