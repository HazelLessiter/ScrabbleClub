using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Scrabble.Models;
using System.Data.Entity.Validation;
using System.Diagnostics;

namespace Scrabble.Controllers
{
    public class GamesController : Controller
    {
        private DBEntities db = new DBEntities();

        // GET: Games
        public ActionResult Index()
        {
            return View(db.Games.ToList());
        }

        // GET: Games/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Game game = db.Games.Find(id);
            if (game == null)
            {
                return HttpNotFound();
            }
            return View(game);
        }

        // GET: Games/Create
        public ActionResult Create()
        {
            var namesQuery = (from n in db.Members select n.Name).ToList();
            List<string> names = namesQuery;
            ViewBag.listOfNames = names;
            return View();
        }

        // POST: Games/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "GameID,Player1,Player2,Player1Score,Player2Score")] Game game)
        {
            if (ModelState.IsValid)
            {
                game.Player1 = Request.Form["names"].ToString();
                game.Player2 = Request.Form["names1"].ToString();
                Member member1 = db.Members.First(m => m.Name == game.Player1);
                Member member2 = db.Members.First(m => m.Name == game.Player2);
                if (game.Player1Score > game.Player2Score)
                {
                    //Player1 wins
                    member1.NumberOfWins++;
                    member2.NumberOfLosses++;
                }
                if (game.Player1Score < game.Player2Score)
                {
                    //Player2 wins
                    member1.NumberOfLosses++;
                    member2.NumberOfWins++;
                }
                else
                {
                    //Draw do nothing
                }
                if (game.Player1Score > member1.HighestScore)
                {
                    //If Player1 has a new highscore
                    member1.HighestScore = game.Player1Score;
                    member1.HighestScoreAgianst = game.Player2;
                    member1.HighestScoreDate = DateTime.Now;
                }
                else
                {
                    //Do nothing
                }
                if (game.Player2Score > member2.HighestScore)
                {
                    //If player 2 has new highscore
                    member2.HighestScore = game.Player2Score;
                    member2.HighestScoreAgianst = game.Player1;
                    member2.HighestScoreDate = DateTime.Now;
                }
                else
                {
                    //Do nothing
                }
                var player1games = (from g in db.Games where (game.Player1 == member1.Name) || (game.Player2 == member1.Name) select g);
                int averagecount = 1, playerscorecount = 1;
                foreach (Game gameLoop in player1games)
                {
                    playerscorecount = (playerscorecount + gameLoop.Player1Score);//Add all player 1 scores together
                    averagecount++;
                    member1.AverageScore = (playerscorecount / (averagecount));
                    playerscorecount = (playerscorecount + gameLoop.Player2Score);//Add all player 2 scores together
                    averagecount++;
                    member2.AverageScore = (playerscorecount / (averagecount));
                    Debug.WriteLine(member1.AverageScore);
                    Debug.WriteLine(member2.AverageScore);
                }
                if (member1.AverageScore == 0)
                {
                    member1.AverageScore = member1.HighestScore;
                }
                else
                {
                    //Do nothing
                }
                if (member2.AverageScore == 0)
                {
                    member2.AverageScore = member2.HighestScore;
                }
                else
                {
                    //Do nothing
                }
                db.Entry(member1).CurrentValues.SetValues(member1);
                db.Entry(member2).CurrentValues.SetValues(member2);
                db.Games.Add(game);
                try
                {
                    db.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    foreach (var prop in e.EntityValidationErrors)
                    {
                        Debug.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:", prop.Entry.Entity.GetType().Name, prop.Entry.State);
                        foreach (var pr in prop.ValidationErrors)
                        {
                            Debug.WriteLine("- Property: \"{0}\", Error: \"{1}\"", pr.PropertyName, pr.ErrorMessage);
                        }
                    }
                    throw;
                }
                return RedirectToAction("Index");
            }
            return View(game);
        }

        // GET: Games/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Game game = db.Games.Find(id);
            if (game == null)
            {
                return HttpNotFound();
            }
            return View(game);
        }

        // POST: Games/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "GameID,Player1,Player2,Player1Score,Player2Score")] Game game)
        {
            if (ModelState.IsValid)
            {
                db.Entry(game).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(game);
        }

        // GET: Games/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Game game = db.Games.Find(id);
            if (game == null)
            {
                return HttpNotFound();
            }
            return View(game);
        }

        // POST: Games/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Game game = db.Games.Find(id);
            db.Games.Remove(game);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}