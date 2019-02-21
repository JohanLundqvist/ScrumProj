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
            var UserId = User.Identity.GetUserId();
            var activeUser = db.Profiles.Single( u => u.ID == UserId);
            var participants = new List<ProfileModel>();
            var time = new List<string>();
            foreach (var user in meeting.MeetingParticipants)
            {
                var obj = new HasVotedOrNo
                {
                    UserId = user.ID,
                    Hasvoted = false,
                    MeetingId = meeting.MeetingId
                };
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
            var Time1 = "";
            var Time2 = "";
            var Time3 = "";
            var Time4 = "";

            foreach (var m in time)
            {
                if (Time1 == "")
                    Time1 = m;
                else if (Time2 == "")
                    Time2 = m;
                else if (Time3 == "")
                    Time3 = m;
                else if (Time4 == "")
                    Time4 = m;
            }
            db.Meetings.Add(meet);
            db.SaveChanges();
            var getLastId = db.Meetings.ToList();
            var i = getLastId.Count() - 1;

            //foreach(var user in meeting.MeetingParticipants)
            //{
            //    db.HasVotedOrNo.Add(new HasVotedOrNo
            //    {
            //        UserId = user.ID,
            //        Hasvoted = false,
            //        MeetingId = getLastId[i].MeetingId
            //    });
            //}


            var mt = new MeetingTimes();
            mt.MeetingId = getLastId[i].MeetingId;
            mt.Time1 = Time1;
            mt.Time2 = Time2;
            mt.Time3 = Time3;
            mt.Time4 = Time4;
            mt.Time2Votes = 0;
            mt.Time3Votes = 0;
            mt.Time4Votes = 0;
            mt.Time1Votes = 0;

            db.MeetingTimes.Add(mt);
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