﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScrumProj.Models
{
    public class File
    {
        public int FileId { get; set; }
        public string FileName { get; set; }
        public byte[] FileBytes { get; set; }
    }
}