using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ScrumProj.Models
{
    public class ValidateFileSizeAttribute : RequiredAttribute
    {
        public int MaxContentLength = 1*256*256;
   public override bool IsValid(object value)
        {

            var file = value as HttpPostedFileBase;

            //this should be handled by [Required]
            if (file == null)
                return true;

            if (file.ContentLength > MaxContentLength)
            {
                ErrorMessage = "Filen är för stor för att laddas upp.";
                return false;
            }

            return true;
        }
    }
}