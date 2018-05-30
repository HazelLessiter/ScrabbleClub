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
    public class LeaderBoardController : Controller
    {
        private DBEntities db = new DBEntities();

        // GET: LeaderBoard
        public ActionResult Index()
        {
            var members = (from m in db.Members orderby m.AverageScore descending select m).Take(10); //Retrieves member data and sorts it by averagescore. Descending makes it sort from highest to lowest.
            return View(members.ToList());
        }
    }
}
