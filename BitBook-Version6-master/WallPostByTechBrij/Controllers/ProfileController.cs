using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WallPostByTechBrij.Models;

namespace WallPostByTechBrij.Controllers
{
    public class ProfileController : Controller
    {
        private WallEntities db = new WallEntities();

        //
        // GET: /Profile/

        public ActionResult Index()
        {
            if (Session["UserProfile"] != null)
            {
                return View();
            }

            return RedirectToAction("Index", "Home");


        }

      
        //
        // GET: /Profile/Create


        //
        // GET: /Profile/Edit/5

        public ActionResult Edit()
        {
            var v=(UserProfile)Session["UserProfile"];
            UserProfile userprofile = db.UserProfiles.Find(v.UserId);
            if (userprofile == null)
            {
                return HttpNotFound();
            }
            return View(userprofile);
        }

        //
        // POST: /Profile/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserProfile userprofile, string confirmpassword)
        {
            //if (ModelState.IsValid)
            //{
            //    db.Entry(userprofile).State = EntityState.Modified;
            //    db.SaveChanges();
            //    return RedirectToAction("Index");
            //}
           
           
                if (ModelState.IsValid)
                {
                    UserProfile bUser = (UserProfile)Session["UserProfile"];
                    userprofile.Country= bUser.Country;
                    Session["UserProfile"] = userprofile;
                    db.UserProfiles.AddOrUpdate(userprofile);
                    //db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

            return View(userprofile);
        }
        public ActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file)
        {
            if (file != null)
            {
                UserProfile aUser = (UserProfile)Session["UserProfile"];
                string pic = Path.GetFileName(file.FileName);
                var path = Path.Combine(Server.MapPath("~/Images/ProfileImages"), pic);
                file.SaveAs(path);
                path = Path.Combine("~/Images/ProfileImages", pic);
                aUser.Country = path;
                db.UserProfiles.AddOrUpdate(aUser);
                db.SaveChanges();
                Session["User"] = aUser;
                return View();
            }
            // after successfully uploading redirect the user
            return RedirectToAction("Index", "User");

        }
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}