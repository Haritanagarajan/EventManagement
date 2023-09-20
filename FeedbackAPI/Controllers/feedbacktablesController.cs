using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using FeedbackAPI.Models;

namespace FeedbackAPI.Controllers
{
    public class feedbacktablesController : ApiController
    {
        private EventManagement2Entities4 db = new EventManagement2Entities4();

        // GET: api/feedbacktables
        public IQueryable<feedbacktable> Getfeedbacktables()
        {
            return db.feedbacktables;
        }

        // GET: api/feedbacktables/5
        [ResponseType(typeof(feedbacktable))]
        public IHttpActionResult Getfeedbacktable(int id)
        {
            feedbacktable feedbacktable = db.feedbacktables.Find(id);
            if (feedbacktable == null)
            {
                return NotFound();
            }

            return Ok(feedbacktable);
        }

        // PUT: api/feedbacktables/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putfeedbacktable(int id, feedbacktable feedbacktable)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != feedbacktable.id)
            {
                return BadRequest();
            }

            db.Entry(feedbacktable).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!feedbacktableExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/feedbacktables
        [ResponseType(typeof(feedbacktable))]
        public IHttpActionResult Postfeedbacktable(feedbacktable feedbacktable)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.feedbacktables.Add(feedbacktable);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = feedbacktable.id }, feedbacktable);
        }

        // DELETE: api/feedbacktables/5
        [ResponseType(typeof(feedbacktable))]
        public IHttpActionResult Deletefeedbacktable(int id)
        {
            feedbacktable feedbacktable = db.feedbacktables.Find(id);
            if (feedbacktable == null)
            {
                return NotFound();
            }

            db.feedbacktables.Remove(feedbacktable);
            db.SaveChanges();

            return Ok(feedbacktable);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool feedbacktableExists(int id)
        {
            return db.feedbacktables.Count(e => e.id == id) > 0;
        }
    }
}