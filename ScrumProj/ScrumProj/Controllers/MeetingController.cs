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
        public ActionResult Booking(MeetingViewModel model)
        {
          
            var listOfUsers = new List<SelectListItem>();
            var listOfMeetings = new List<Meeting>();
            var listProposedTimes = new List<string>();

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

            var userId = User.Identity.GetUserId();
            var activeUser = _context.Profiles.First(u => u.ID == userId);

            model.UsersToAdd = listOfUsers;
            model.Meetings = listOfMeetings;
            model.User = activeUser;
            return View(model);
        }
    }
}