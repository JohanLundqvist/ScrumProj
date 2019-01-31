using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScrumProj.Models
{
	public class EntryViewModel
	{
		public ProfileModel loggedInUser;
		public byte[] filebytes { get; set; }
		public string fileName { get; set; }
		public Entry content { get; set; }
	}
}