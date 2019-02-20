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
using System.Web.Http.Results;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using ScrumProj.Models;

namespace ScrumProj.Controllers
{
    public class MeetingsAPIController : ApiController
    {
        private AppDbContext db = new AppDbContext();

        // GET: api/Meetings
        //public IQueryable<Meeting> GetMeetings()
        //{
        //    return db.Meetings;
        //}

        [System.Web.Http.HttpGet]
        public JsonResult<List<Meeting>> GetMeetingsJson()
        {
            var list = new List<Meeting>();

            foreach(var m in db.Meetings)
            {
                foreach(var mp in m.MeetingParticipants)
                {
                    if (mp.ID.Equals(User.Identity.GetUserId()))
                    {
                        list.Add(m);
                    }
                }
            }

            return Json(list);
        }


        // GET: api/Meetings/5
        [ResponseType(typeof(Meeting))]
        public IHttpActionResult GetMeeting(int id)
        {
            Meeting meeting = db.Meetings.Find(id);
            if (meeting == null)
            {
                return NotFound();
            }

            return Ok(meeting);
        }

        // PUT: api/Meetings/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutMeeting(int id, Meeting meeting)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != meeting.MeetingId)
            {
                return BadRequest();
            }

            db.Entry(meeting).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MeetingExists(id))
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

        // POST: api/Meetings
        [System.Web.Http.HttpPost]
        public void PostMeeting([FromBody]Meeting meeting)
        {
            var activeUser = db.Profiles.Single( u => u.ID == User.Identity.GetUserId());
            var participants = new List<ProfileModel>();
            var time = new List<string>();
            foreach (var user in meeting.MeetingParticipants)
            {
                 var Fulluser = db.Profiles.Single(u => u.ID == user.ID);
                participants.Add(Fulluser);
            }
            participants.Add(activeUser);
            var titlt = meeting.MeetingTitle;

            foreach (var proptime in meeting.ProposedTimes)
            {  
                time.Add(proptime);
            }
            var meet = new Meeting
            {
                MeetingTitle = titlt,
                MeetingParticipants = participants,
                Time = meeting.Time,
                ProposedTimes = time
            };
            
            db.Meetings.Add(meet);
            db.SaveChanges();


        }

        // DELETE: api/Meetings/5
        [ResponseType(typeof(Meeting))]
        public IHttpActionResult DeleteMeeting(int id)
        {
            Meeting meeting = db.Meetings.Find(id);
            if (meeting == null)
            {
                return NotFound();
            }

            db.Meetings.Remove(meeting);
            db.SaveChanges();

            return Ok(meeting);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MeetingExists(int id)
        {
            return db.Meetings.Count(e => e.MeetingId == id) > 0;
        }
    }
}