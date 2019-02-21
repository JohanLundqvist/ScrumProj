using Microsoft.AspNet.Identity;
using ScrumProj.Models;
using ScrumProj.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ScrumProj.Controllers
{
    public class MeetingController : Controller
    {
         AppDbContext _context = new AppDbContext();

        [Authorize]
        public ActionResult Booking()
        {
            var model = new MeetingViewModel();
            var listOfUsers = new List<SelectListItem>();
            var listOfMeetings = new List<Meeting>();
            var listProposedTimes = new List<string>();
            //model.UserNotVoted = new List<ProfileModel>();

            listProposedTimes.Add("08.00-09.00");
            listProposedTimes.Add("09.00-10.00");
            listProposedTimes.Add("10.00-11.00");
            listProposedTimes.Add("11.00-12.00");
            listProposedTimes.Add("12.00-13.00");
            listProposedTimes.Add("13.00-14.00");
            listProposedTimes.Add("14.00-15.00");
            listProposedTimes.Add("15.00-16.00");

            model.ProposedTimes = listProposedTimes;

            foreach (var m in _context.Meetings)
            {
                listOfMeetings.Add(m);
            }

            foreach (var user in _context.Profiles)
            {
                if (!user.ID.Equals(User.Identity.GetUserId()))
                {
                    var item = new SelectListItem
                    {
                        Text = user.FirstName + " " + user.LastName,
                        Value = user.ID,
                        Selected = false
                    };
                    listOfUsers.Add(item);
                }
            }
            var ListOfMeetings = _context.Meetings.ToList();
            //var DicInvitedToMeeting = new Dictionary<int, int>();
            //if (ListOfMeetings != null)
            //{
            //    var dt = new Dictionary<string, double>();
            //    foreach (var m in ListOfMeetings)
            //    {
            //        DicInvitedToMeeting.Add(m.MeetingId, m.MeetingParticipants.Count());
            //        var valueOfVote = 100 / m.MeetingParticipants.Count();
            //        var mt = new MeetingTimes();
            //        mt = _context.MeetingTimes.Where(n => n.MeetingId == m.MeetingId).Single();
            //        if (mt.Time1 != null)
            //            dt.Add(mt.Time1, mt.Time1Votes * valueOfVote);
            //        if (mt.Time2 != null)
            //            dt.Add(mt.Time2, mt.Time2Votes * valueOfVote);
            //        if (mt.Time3 != null)
            //            dt.Add(mt.Time3, mt.Time3Votes * valueOfVote);
            //        if (mt.Time4 != null)
            //            dt.Add(mt.Time4, mt.Time4Votes * valueOfVote);
            //        model.Times = mt;
            //    }
            //    model.DicTimes = dt;
            //}
            
            var userId = User.Identity.GetUserId();
            var activeUser = _context.Profiles.First(u => u.ID == userId);

            model.UsersToAdd = listOfUsers;
            model.Meetings = listOfMeetings;
            model.User = activeUser;

            return View(model);
        }
        //public ActionResult Vote(MeetingViewModel model, string SelectedTime = "")
        //{
        //    var currentUserId = User.Identity.GetUserId();
        //    if (SelectedTime == "")
        //        return RedirectToAction("Booking");
        //    var ctx = new AppDbContext();
        //    var theMeeting = ctx.MeetingTimes.Find(model.Times.Id);

        //    if (SelectedTime == theMeeting.Time1)
        //        theMeeting.Time1Votes++;
        //    else if (SelectedTime == theMeeting.Time2)
        //        theMeeting.Time2Votes++;
        //    else if (SelectedTime == theMeeting.Time3)
        //        theMeeting.Time3Votes++;
        //    else if (SelectedTime == theMeeting.Time4)
        //        theMeeting.Time4Votes++;

        //    ctx.SaveChanges();

        //    return RedirectToAction("Booking");
        //}
    }
}