﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.Domain.Services.ViewModels
{
    public class BlogVM
    {
        public string Url { get; set; }
        public int Rating { get; set; }

        public DateTime CreatedTimeUtc { get; set; }
    }
}
