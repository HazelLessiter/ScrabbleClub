using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Scrabble.Models;

namespace Scrabble.Controllers
{
    public class ProfileController : Controller
    {
        private DBEntities db = new DBEntities();

        // GET: Profile
        public ActionResult Index(string q)
        {

            var namesQuery = (from n in db.Members select n.Name).ToList();
            List<string> names = namesQuery;
            ViewBag.listOfNames = names;
            var members = db.Members.Where(a => a.Name.Contains(q));
            return View(members.ToList());
        }

        // GET: Profile/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member = db.Members.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }
    }
}
