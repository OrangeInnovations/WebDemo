﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp
{
    public class BlogingSettings
    {
        public bool UseCustomizationData { get; set; }

        public string ConnectionString { get; set; }


        public int GracePeriodTime { get; set; }

        public int CheckUpdateTime { get; set; }
    }
}